using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//福本和也
/// <summary>
/// テキスト表示
/// コスト管理
/// プレイヤー召喚
/// エネミー自動召喚
/// 
/// </summary>
public class UnitSpawn : MonoBehaviour
{
    //定数
    const int   UnitObjectLimit         = 3;       //編成上限(最大値は8実装ユニットが3の為現在は3)
    const int   SpawnLimit              = 50;      //召喚上限
    const float IntervalSpeed           = 5;       //インターバルスピード
    const float TextTimer               = 10;      //テキスト表示時間/定数

    //プライベート処理用変数
    float PlayerCostUpSpeed = 10;             //プレイヤーコストアップ間隔
    float EnemyCostUpSpeed  = 7;              //エネミーコストアップ間隔
    float enemySpawnCost;                     //エネミー残りコスト
    float intervalTextTimer;                  //テキスト表示時間/処理用
    float noCostTextTimer;                    //テキスト表示時間/処理用
    float warningTextTimer = 0;               //特殊召喚テキスト表示時間/処理用
    float spSpawnEnemyBaseHp = 100;           //エネミーの基地最大HPの半分/特殊召喚発動時に使用
    bool  spSpawnFlg         = false;         //エネミー特殊召喚を一回やる為のフラグ
    int   spSpawnUnitNum     = 3;             //エネミー特殊召喚するユニットの数
    int   antiSpSpawnPlayerCostSpeedUp = 3;   //特殊召喚に対抗するためのコストアップ
    float enemyAutoSpawnInterval = 20;        //エネミーの召喚間隔/タイムライン使ったらいらなくなるかも
    float enemyAutoSpawnTimer = 0;            //エネミー召喚間隔/上と同じ

    //パブリック処理用変数
    [System.NonSerialized]
    public float playerSpawnCnt = 0;          //プレイヤー召喚数
    [System.NonSerialized]
    public float enemySpawnCnt = 0;           //エネミー召喚数
    [Header("プレイヤー残りコスト")]
    public float playerSpawnCost;             //プレイヤー残りコスト 

    //外部取得変数
    [System.NonSerialized]
    public float[] playerSpawnInterval        //プレイヤーユニット召喚間隔 
        = new float[UnitObjectLimit];
    [System.NonSerialized]
    public float[] enemySpawnInterval         //エネミーユニット召喚間隔
        = new float[UnitObjectLimit];

    //ゲームオブジェクト
    GameObject playerBase;                   //プレイヤー基地
    Base       playerBaseScript;             //プレイヤー基地スクリプト
    GameObject enemyBase;                    //エネミー基地
    Base       enemyBaseScript;              //エネミー基地スクリプト
    [Header("召喚するプレイヤーユニットプレファブ")]
    public GameObject[] spawnPlayerObject;   //プレイヤーユニット
    [Header("召喚するエネミーユニットプレファブ")]
    public GameObject[] spawnEnemyObject;    //エネミーユニット
    [System.NonSerialized]
    public Unit[] playerUnitScript           //プレイヤーユニットスクリプト
        = new Unit[UnitObjectLimit];
    [System.NonSerialized]
    public Unit[] enemyUnitScript            //エネミーユニットスクリプト
        = new Unit[UnitObjectLimit];


    /*
    スタートでGetコンポーネントする。
    GameObjectSet()でやる。
    */
    public Slider[] spawnIntervalSliders     //ユニット毎の召喚間隔シリンダー
        = new Slider[UnitObjectLimit];
    public Text       costText;              //コスト表示テキスト
    public GameObject spawnIntervalText;     //ユニット召喚間隔不経過テキスト
    public GameObject spawnLimitText;        //ユニット召喚上限テキスト
    public GameObject noCostText;            //コスト不足テキスト
    public GameObject warningText;           //特殊召喚警告
    public GameObject warningDisplayTimeText;//警告時間表示適テキスト

    public AudioSource audioSource;
    public AudioClip   warningSe;

    //座標
    Vector3 playerSkySpawnPos;              //プレイヤー空ユニット生成位置
    Vector3 playerSeaSpawnPos;              //プレイヤー海上ユニット生成位置
    Vector3 playerInSeaSpawnPos;            //プレイヤー海中ユニット生成位置
    Vector3 enemySkySpawnPos;               //エネミー空ユニット生成位置
    Vector3 enemySeaSpawnPos;               //エネミー海上ユニット生成位置
    Vector3 enemyInSeaSpawnPos;             //エネミー海中ユニット生成位

    Vector3 playerSpawnPos;                 //プレイヤー召喚時座標
    Vector3 enemySpawnPos;                  //エネミー召喚時座標

    /// <summary>
    /// 両方
    /// 召喚座標や処理に使うベースとスクリプトをセット
    /// </summary>
    void GameObjectSet() {
        playerBase = GameObject.Find("Base_Player");
        playerBaseScript = playerBase.GetComponent<Base>();
        enemyBase = GameObject.Find("Base_Enemy");
        enemyBaseScript = enemyBase.GetComponent<Base>();
    }

    /// <summary>
    /// プレイヤー
    /// ユニット召喚位置セット
    /// </summary>
    void PlayerUnitSpawnPosSet() 
    {
        if (playerBase == null)
        {
            Debug.Log("プレイヤーベースが見つかりません");
        }
        playerSkySpawnPos = playerBase.transform.GetChild(0).position;
        playerSeaSpawnPos = playerBase.transform.GetChild(1).position;
        playerInSeaSpawnPos = playerBase.transform.GetChild(2).position;
        //print(playerSkySpawnPos);
        //print(playerSeaSpawnPos);
        //print(playerInSeaSpawnPos);
    }

    /// <summary>
    /// エネミー
    /// ユニット召喚位置セット
    /// </summary>
    void EnemyUnitSpawnPosSet()
    {
        if (enemyBase == null)
        {
            Debug.Log("エネミーベースが見つかりません");
        }
        enemySkySpawnPos = enemyBase.transform.GetChild(0).position;
        enemySeaSpawnPos = enemyBase.transform.GetChild(1).position;
        enemyInSeaSpawnPos = enemyBase.transform.GetChild(2).position;
        //print(enemySkySpawnPos);
        //print(enemySeaSpawnPos);
        //print(enemyInSeaSpawnPos);
    }

    /// <summary>
    /// 両方
    /// ゲームオブジェクトのNullチェック
    /// スクリプトのチェック/変数の格納
    /// </summary>
    void NullCheck() {
        for (int i = 0; i < UnitObjectLimit; i++)
        {
            //プレイヤー
            if (spawnPlayerObject[i] == null)
            {
                Debug.Log("PlayerUnitの" + i + "番目がnullです。");
            }
            else
            {
                playerUnitScript[i] = spawnPlayerObject[i].GetComponent<Unit>();
            }

            //エネミー
            if (spawnEnemyObject[i] == null)
            {
                Debug.Log("EnemyUnitの" + i + "番目がnullです。");
            }
            else
            {
                enemyUnitScript[i]    = spawnEnemyObject[i].GetComponent<Unit>();
                enemySpawnInterval[i] = spawnEnemyObject[i].GetComponent<Unit>().spawnInterval;
            }
        }
    }

    /// <summary>
    /// プレイヤー
    /// セットされたユニットの生成位置をクラスタイプに応じて変更
    /// </summary>
    /// <param name="num"></param>
    void PlayerSpawnPosSet(int num) {
        if (playerUnitScript[num].classType == Unit.ClassType.Sky)   {
            playerSpawnPos = playerSkySpawnPos;
        }
        if (playerUnitScript[num].classType == Unit.ClassType.Sea)   {
            playerSpawnPos = playerSeaSpawnPos;
        }
        if (playerUnitScript[num].classType == Unit.ClassType.InSea) {
            playerSpawnPos = playerInSeaSpawnPos;
        }
    }

    /// <summary>
    /// プレイヤー専用
    /// 召喚ボタン押下処理
    /// </summary>
    /// <param name="num"></param>
    public void PushSpawnButton(int num)
    {
        //召喚上限より下か
        if (playerSpawnCnt < SpawnLimit )
        {
            //召喚間隔を過ぎているか
            if (playerSpawnInterval[num] <= 0)
            {
                //コストは足りているか
                if (playerUnitScript[num].cost < playerSpawnCost)
                {
                    //プレイヤー召喚
                    PlayerSpawnPosSet(num);
                    //召喚間隔をセット
                    playerSpawnInterval[num] = playerUnitScript[num].spawnInterval;
                    //シリンダーと召喚間隔を同期
                    spawnIntervalSliders[num].value = playerSpawnInterval[num];
                    //コストをマイナス
                    playerSpawnCost -= playerUnitScript[num].cost;
                    //ユニットを生成(オブジェクトプールに対応する)
                    Instantiate(spawnPlayerObject[num], playerSpawnPos, Quaternion.Euler(0, 0, 0));
                    //プレイヤー召喚総数をプラス
                    playerSpawnCnt++;
                }
                else 
                {
                    //コスト不足警告
                    noCostTextTimer = TextTimer;
                    noCostText.SetActive(true);
                }
            }
            else
            {
                //召喚間隔未経過警告
                intervalTextTimer = TextTimer;
                spawnIntervalText.SetActive(true);
            }
        }
    }

    /// <summary>
    /// エネミー
    /// セットされたユニットの生成位置をクラスタイプに応じて変更
    /// </summary>
    /// <param name="num"></param>
    void EnemySpawnPosSet(int num)
    {
        if (enemyUnitScript[num].classType == Unit.ClassType.Sky)
        {
            enemySpawnPos = enemySkySpawnPos;
        }
        if (enemyUnitScript[num].classType == Unit.ClassType.Sea)
        {
            enemySpawnPos = enemySeaSpawnPos;
        }
        if (enemyUnitScript[num].classType == Unit.ClassType.InSea)
        {
            enemySpawnPos = enemyInSeaSpawnPos;
        }
    }

    /// <summary>
    /// エネミー
    /// 召喚ボタン押下処理
    /// </summary>
    /// <param name="num"></param>
    public void EnemySpawn(int num)
    {
        if (enemySpawnCnt < SpawnLimit)
        {
            if (enemySpawnInterval[num] <= 0)
            {
                if (enemyUnitScript[num].cost < enemySpawnCost)
                {
                    EnemySpawnPosSet(num);
                    enemySpawnInterval[num] = enemyUnitScript[num].spawnInterval;
                    enemySpawnCost -= enemyUnitScript[num].cost;
                    Instantiate(spawnEnemyObject[num], enemySpawnPos, Quaternion.Euler(0, 180, 0));
                    enemySpawnCnt++;
                }
            }
        }
    }

    /// <summary>
    /// エネミー専用/タイムラインに切り替え予定
    /// 条件に応じてエネミーを召喚
    /// </summary>
    void AutoEnemySpawn() {
        if (enemyBaseScript.hp <= spSpawnEnemyBaseHp && spSpawnFlg == false)
        {
            EnemySpSpawn();
        }
        enemyAutoSpawnTimer -= (Time.deltaTime * IntervalSpeed);
        if (enemyAutoSpawnTimer <= 0)
        {
            enemyAutoSpawnTimer = enemyAutoSpawnInterval;
            var ran = Random.Range(0, UnitObjectLimit);
            EnemySpawn(ran);
        }
    }

    /// <summary>
    /// HPがspSpawnEnemyBaseHp以下になったら一回だけ呼び出す。
    /// 特殊召喚処理
    /// </summary>
    void EnemySpSpawn()
    {
        spSpawnFlg = true;
        audioSource.PlayOneShot(warningSe);
        enemyAutoSpawnInterval /= 3;
        PlayerCostUpSpeed += antiSpSpawnPlayerCostSpeedUp;
        warningText.SetActive(true);
        warningTextTimer = TextTimer;
        //ゲームを遅くする処理/未完なので一端除外
        //Time.timeScale = 0.3f;
        for (int j = 0; j < UnitObjectLimit; j++)
        {
            EnemySpawnPosSet(j);

            for (int i = 0; i < spSpawnUnitNum; i++)
            {
                enemySpawnPos.x += i;
                Instantiate(spawnEnemyObject[j], enemySpawnPos, Quaternion.Euler(0, 180, 0));
                enemySpawnPos.x -= i;
            }

        }
    }

    /// <summary>
    /// 両方
    /// 召喚インターバルカウント
    /// </summary>
    void SpawnInterval() {
        for (int i = 0; i < UnitObjectLimit; i++) {
            if (playerSpawnInterval[i] > 0) {
                playerSpawnInterval[i] -= (Time.deltaTime * IntervalSpeed);
                spawnIntervalSliders[i].value = playerSpawnInterval[i];
            }
            if (enemySpawnInterval[i] > 0)
            {
                enemySpawnInterval[i] -= (Time.deltaTime * IntervalSpeed);
            }
        }
    }

    /// <summary>
    /// 両方
    /// コスト加算処理
    /// </summary>
    void AddCost() {
        playerSpawnCost += (PlayerCostUpSpeed * IntervalSpeed * Time.deltaTime);
        enemySpawnCost  += (EnemyCostUpSpeed * IntervalSpeed * Time.deltaTime);
    }

    /// <summary>
    /// テキスト画面表示処理
    /// </summary>
    void TextDisplay() 
    {
        //コスト
        costText.text = "軍事費:" + (int)playerSpawnCost;
        
        //召喚インターバル
        if (intervalTextTimer <= 0)
        {
            spawnIntervalText.SetActive(false);
        }
        else
        {
            intervalTextTimer -= (Time.deltaTime * IntervalSpeed);
        }

        //召喚数
        if (playerSpawnCnt >= SpawnLimit)
        {
            spawnLimitText.SetActive(true);
        }
        else 
        {
            spawnLimitText.SetActive(false);
        }

        //コスト不足
        if (noCostTextTimer <= 0) 
        {
            noCostText.SetActive(false);
        }
        else 
        {
            noCostTextTimer -= (Time.deltaTime * IntervalSpeed);
        }

        //特殊召喚警告
        if (warningTextTimer < 0)
        {
            warningText.SetActive(false);
            //Time.timeScale = 1.0f;
        }
        else
        {
            warningTextTimer -= (Time.deltaTime * (IntervalSpeed / 5));
            warningDisplayTimeText.GetComponent<Text>().text = "表示時間:" + (int)warningTextTimer;
        }
    }

    void VariableSet() {
        //特殊召喚発動タイミング
        spSpawnEnemyBaseHp = (enemyBaseScript.hp / 2);

        //スポーン間隔のシリンダー
        for (int i = 0;i<UnitObjectLimit;i++) {
            spawnIntervalSliders[i].minValue = 0;
            spawnIntervalSliders[i].maxValue = playerUnitScript[i].spawnInterval;
            spawnIntervalSliders[i].value    = 0;
            //print(i);
        }
    }

    void Start()
    {
        GameObjectSet();
        PlayerUnitSpawnPosSet();
        EnemyUnitSpawnPosSet();

        NullCheck();

        VariableSet();

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //コスト増加処理
        AddCost();

        //インターバル
        SpawnInterval();
        
        //UI表示
        TextDisplay();

        //敵自動召喚
        AutoEnemySpawn();

        if (Input.GetKeyDown(KeyCode.K))
        {
            //Time.timeScale = 0.0f;
        }

        /*
        コストスピード増加仮処理/デバッグ用
        if (Input.GetKeyDown(KeyCode.I)) {
            if (PlayerCostUpSpeed < 100)
            {
                PlayerCostUpSpeed += 5;
            }
        }
        */

        /*エネミーデバッグ
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                EnemySpawn(0);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                EnemySpawn(1);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                EnemySpawn(2);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                EnemySpawn(3);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                EnemySpawn(4);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                EnemySpawn(5);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                EnemySpawn(6);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                EnemySpawn(7);
            }
            print(enemySpawnCost);
            
        }*/
    }
}