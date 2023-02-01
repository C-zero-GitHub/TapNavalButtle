using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//福本和也
//ガード値追加したい。

public class Unit : MonoBehaviour
{
    //最大/最小ステータス
    const float Min_atkInterval = 0.1f;     //攻撃間隔最小値
    const float Min_cost        = 1;        //コスト最小値
    const float Max_speed       = 10;       //スピード最大値
    const float Max_hp          = 9999;     //耐久最大値

    //定数
    
    /// <summary>海上/海中/空中のいずれか</summary>
    public enum ClassType
    {
        Sea,
        InSea,
        Sky
    }
    public ClassType classType;

    //ゲームオブジェクト
    public GameObject[] bullet;             //弾オブジェクト
    public Bullet[] bulletScript;           //弾スクリプト
    Rigidbody2D   rb;                       //物理挙動
    public Slider hpSlider;                 //HPバー
    public UnitSpawn unitSpawn;             //ユニットスポナー
    UnitSensor antiUnitSensor;              //ユニットセンサー
    UnitSensor antiBaseSensor;              //ベースセンサー

    //基本ステータス
    public bool  enemyUnit     = false;     //エネミーユニットフラグ
    public float b_atk         = 3;         //攻撃力/Bulletへ
    public float cost          = 100;       //召喚コスト
    public float speed         = 1.0f;      //ユニットの進軍速度
    public float atkInterval   = 5.0f;      //弾の発射間隔
    public float hp            = 30.0f;     //ユニットの耐久値
    public float spawnInterval = 3.0f;      //スポーン間隔

    //バフ値
    public int   cost_Buff          = 0;     //コストを安くする変数
    public float speed_Buff         = 0.0f;  //進軍速度バフ
    public float atkInterval_Buff   = 0.0f;  //発射間隔短縮
    public float hp_Buff            = 0.0f;  //耐久値アップ
    public float spawnInterval_Buff = 0.0f;  //スポーン間隔短縮
    public float b_atk_Buff         = 0.0f;  //攻撃力プラス値/Bulletへ

    //特殊ステータス共用
    public int Change_Bullet = 10;           //何発撃ったら強い弾を発射するか。
    public float StrongBulletBuff = 1.5f;    //強い弾のバフ値

    //特殊ステータス個別


    //処理用変数
    float setAtkInterval;               //インターバル初期化

    //外部アクセス用
    public bool encounter = false;

    //プライベート変数
    int nowBullet  = 0;                  //現在の発射弾管理


    /// <summary>
    /// 移動処理
    /// </summary>
    void MoveUnit()
    {
        Vector3 now = rb.position;
        now += new Vector3((speed * Time.deltaTime), 0.0f, 0.0f);
        rb.position = now;
    }

    /// <summary>
    /// 弾を発車する関数。
    /// 3発01を撃つと02を一回発射する。
    /// 現在は生成で対応するがアクティブで対応したい。
    /// </summary>
    void Atack() {
        if(nowBullet < Change_Bullet) {
            Instantiate(bullet[0], transform.GetChild(0).position, transform.rotation);
            nowBullet++;
        }
        else{
            Instantiate(bullet[1], transform.GetChild(0).position, transform.rotation);
            nowBullet = 0;
        }
    }

    /// <summary>
    /// hpが0以下になったら削除
    /// </summary>
    void DestroyUnit()
    {
        if (enemyUnit)
        {
            unitSpawn.enemySpawnCnt--;
        }
        else
        {
            unitSpawn.playerSpawnCnt--;
        }
        Destroy(gameObject);

    }

    /// <summary>
    /// 接敵時処理/攻撃間隔が0になったら攻撃しタイマーを初期化する。
    /// </summary>
    void Encounter() {
        atkInterval -= Time.deltaTime;
        if (atkInterval <= 0) {
            Atack();
            atkInterval = setAtkInterval;
        }
    }

    /// <summary>
    /// 外部アクセスのみ/ダーメージを受ける処理/Bulletからアクセスされる。
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(float damage)
    {
        hp -= damage;

        hpSlider.value = hp;
    }

    /// <summary>
    /// 処理用のスクリプトをセット
    /// </summary>
    void ScriptSet() {
        unitSpawn = GameObject.Find("UnitSpawn").GetComponent<UnitSpawn>();
        antiUnitSensor = transform.GetChild(1).gameObject.GetComponent<UnitSensor>();
        antiBaseSensor = transform.GetChild(2).gameObject.GetComponent<UnitSensor>();
    }

    /// <summary>
    /// センサーのターゲットをセット
    /// </summary>
    void SensorTarget() {
        antiUnitSensor.SensorTargetSet(0);      //ユニットセンサー
        antiBaseSensor.SensorTargetSet(1);      //基地センサー
    }

    /// <summary>
    /// 処理用の変数をセット
    /// </summary>
    void VariableSet() {
        //HPバー処理用
        hpSlider.minValue = 0;
        hpSlider.maxValue = hp;
        hpSlider.value = hp;

        //処理用変数初期値
        setAtkInterval = atkInterval;

        //物理挙動をセット
        rb = this.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// バフの整合性チェック及びステータス変化/データ書き換え対策
    /// </summary>
    void StatusCheck() {
        //マイナスの値がないか確認
        if (speed            < 0) speed            *= -1;
        if (speed_Buff       < 0) speed_Buff       *= -1;
        if (cost             < 0) cost             *= -1;
        if (cost_Buff        < 0) cost_Buff        *= -1;
        if (hp               < 0) hp               *= -1;
        if (hp_Buff          < 0) hp_Buff          *= -1;
        if (atkInterval      < 0) atkInterval      *= -1;
        if (atkInterval_Buff < 0) atkInterval_Buff *= -1;

        
        //攻撃間隔
        if ((atkInterval - atkInterval_Buff) <= Min_atkInterval)
        {
            atkInterval = Min_atkInterval;
            Debug.Log("攻撃間隔が最小の値に設定されました。");
        }
        else {
            atkInterval -= atkInterval_Buff;
        }
        
        //コスト
        if ((cost - cost_Buff) <= Min_cost)
        {
            cost = Min_cost;
            Debug.Log("コストが最小の値に設定されました。");
        }
        else
        {
            cost -= cost_Buff;
        }
        
        if (speed + speed_Buff >= Max_speed)
        {
            speed = Max_speed;
            Debug.Log("スピードが最大の値に設定されました。");
        }
        else
        {
            speed += speed_Buff;
        }

        if (hp + hp_Buff >= Max_hp)
        {
            hp = Max_hp;
            Debug.Log("HPが最大の値に設定されました。");
        }
        else
        {
            hp += hp_Buff;
        }

        //エネミーのスピードのみ反転
        if (enemyUnit)
        {
            speed *= -1;
        }

    }

    /// <summary>
    /// スクリプトセット
    /// </summary>
    void BulletStatusSet() {
        for (int i = 0; i < 2; i++) {
            //弾スクリプトをセット
            bulletScript[i] = bullet[i].GetComponent<Bullet>();
            //弾にユニットの2倍のスピードをセット
            bulletScript[i].speed = (speed * (2f));
            //敵ユニットである場合弾のフラグも変更
            bulletScript[i].enemyBullet = enemyUnit;
            if (i == 1)
            {
                //強攻撃値をセット
                bulletScript[i].atk = (b_atk * StrongBulletBuff);
            }
            else
            {
                //通常攻撃値をセット
                bulletScript[i].atk = b_atk;
            }
            bulletScript[i].atk_Buff = b_atk_Buff;
        }
    }

    void Start()
    {
        //ステータス整合性チェック
        StatusCheck();

        //弾オブジェクト/ステータスセット
        BulletStatusSet();

        //スクリプトをセット
        ScriptSet();

        //センサーのターゲットをセット
        SensorTarget();

        //変数をセット
        VariableSet();
    }


    void Update()
    {
        //何故かカメラを動かすとバグるので仮処理/UnitSpawnにもある
        //hpSlider.value = hp;

        if (encounter == true)
        {
            //接敵処理
            Encounter();
        }
        else
        {
            //移動処理
            MoveUnit();
        }

        if (hp <= 0)
        {
            //HPが0になったら削除(オブジェクトプールに対応予定)
            DestroyUnit();
        }
    }
}