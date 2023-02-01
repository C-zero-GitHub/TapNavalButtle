using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
//定数
各バレットの特徴となるため定数で宣言。
    Bullet_Sea   = 1.0f;    通常速度
    Bullet_InSea = 0.5f;    海中のため減速
    Bullet_Sky   = 1.5f;    空中の為高速
*/
public class Bullet : MonoBehaviour
{
    //定数


    /// <summary>海上/海中/空中のいずれか</summary>
    public enum ClassType
    {
        Sea,
        InSea,
        Sky
    }
    [System.NonSerialized]
    public ClassType classType;

    //処理用変数
    Vector3 destroyPos;           //生成時の座標.x+DestroyPosPlus.x

    //基本ステータス/Unitから決定
    //[System.NonSerialized]
    public bool enemyBullet;      //敵の場合true
    //[System.NonSerialized]
    public float speed;           //前進速度
    //[System.NonSerialized]
    public float atk;             //攻撃力
    //[System.NonSerialized]
    public float destroyDistance; //この距離進んだら削除
    //バフ/Unitから決定
    //[System.NonSerialized]
    public float atk_Buff;        //攻撃力プラス値

    //ゲームオブジェクト
    Rigidbody2D rb;

    /// <summary>
    /// 当たり判定処理
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D col)
    {
        //ユニットに当たった時の処理
        if (col.gameObject.GetComponent<Unit>())
        {
            col.gameObject.GetComponent<Unit>().Damage(atk);
            Destroy(gameObject);
        }

        //基地に当たった時の処理
        if (col.gameObject.GetComponent<Base>())
        {
            col.gameObject.GetComponent<Base>().Damage(atk);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// オブジェクトを進ませる処理
    /// </summary>
    /// <param name="_bulletClassSpeed"></param>
    void MoveBullet() {
        Vector3 now = rb.position;
        now += new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
        rb.position = now;
    }

    void DestroyBullet() {
        if (!enemyBullet && transform.position.x > destroyPos.x) {
            Destroy(gameObject);
        }
        if ( enemyBullet && transform.position.x < destroyPos.x)
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        //エネミーなら逆方向に進む。
        if (enemyBullet)
        {
            destroyPos.x = transform.position.x - destroyDistance;
        }
        else
        {
            destroyPos.x = transform.position.x + destroyDistance;
        }
        rb = this.GetComponent<Rigidbody2D>();

    }


    void Update()
    {
        switch (classType)
        {
            case ClassType.Sea:     //海上
                MoveBullet();
                break;

            case ClassType.InSea:   //海中
                MoveBullet();
                break;

            case ClassType.Sky:     //空
                MoveBullet();
                break;
        }


        //常時処理
        DestroyBullet();

    }
}