using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//福本和也
public class Camera : MonoBehaviour
{
    //定数
    const float X_Offset = 6.31f;       //基地がカメラに収まるように調整

    //変数
    float speed = 10.0f;                //カメラのスピード
    
    //自動決定される変数
    float enemyPosX = 0;
    float playerPosX = 0;
    float Min_X = 0;
    float Max_X = 0;

    GameObject enemyBase;
    GameObject playerBase;

    /// <summary>
    /// ベースの位置を参照しカメラの移動範囲を決定する。
    /// </summary>
    void SetCameraMoveRange() {
        //敵味方のベースを置くとカメラの両端が決まる。
        enemyBase = GameObject.Find("Base_Enemy");
        playerBase = GameObject.Find("Base_Player");
        enemyPosX = enemyBase.transform.position.x;
        playerPosX = playerBase.transform.position.x;
        Max_X = (enemyPosX - X_Offset);
        Min_X = (playerPosX + X_Offset);

        //MinとMaxが正常か判定する。違っていた場合、仮の値を代入する。
        if(Min_X >= Max_X)
        {
            Debug.Log("基地の座標が異常です。");
            Min_X = 0;
            Max_X = 10;
        }
    }

    /// <summary>
    /// キー入力でカメラを動かす。
    /// </summary>
    void CameraMove() {         
        //左右キー入力
        var hor = Input.GetAxis("Horizontal");

        transform.position = new Vector3(
            Mathf.Clamp(
            transform.position.x + (hor * speed * Time.deltaTime), Min_X, Max_X),
            -2.8f, -10);
    }

    void Start()
    {
        SetCameraMoveRange();
    }

    void Update()
    {
        CameraMove();
    }
}
