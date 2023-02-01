using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//福本和也
public class UnitSensor : MonoBehaviour
{
    GameObject unitObject;
    Unit       unitScript;
    public bool enemyUnit = false;


    /// <summary>
    /// センサータイプ
    /// ユニットから管理
    /// </summary>
    public enum SensorType
    {
        Unit,
        Base
    }
    public SensorType sensorType;

    /// <summary>
    /// Unitからアクセス。ターゲットをセット。
    /// </summary>
    /// <param name="num"></param>
    public void SensorTargetSet(int num) {
        if (num == 0) {
            sensorType = SensorType.Unit;
        }
        if (num == 1) {
            sensorType = SensorType.Base;
        }
    }

    /// <summary>
    /// ベースまで行ったらUnitのSpeedを0にする。
    /// </summary>
    void MoveEnd() {
        unitScript.speed = 0;
    }

    /// <summary>
    /// 対面で停止処理OnTriggerEnter
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerStay2D(Collider2D col)
    {
        switch (sensorType) {
            case SensorType.Unit:
                if (enemyUnit == false)
                {
                    //敵ユニットに当たった時の処理
                    if (col.gameObject.tag == "Enemy_Unit")
                    {
                        unitScript.encounter = true;

                    }
                    else 
                    {
                        unitScript.encounter = false;
                    }
                }

                if (enemyUnit == true)
                {
                    //味方ユニットに当たった時の処理
                    if (col.gameObject.tag == "Player_Unit")
                    {
                        unitScript.encounter = true;
                    }
                    else
                    {
                        unitScript.encounter = false;
                    }
                }
                break;
            case SensorType.Base:
                if (enemyUnit == false)
                {
                    //敵ベースに当たった時の処理
                    if (col.gameObject.tag == "Enemy_Base")
                    {
                        unitScript.encounter = true;
                        MoveEnd();
                    }
                }

                if (enemyUnit == true)
                {
                    //味方ベースに当たった時の処理
                    if (col.gameObject.tag == "Player_Base")
                    {
                        unitScript.encounter = true;
                        MoveEnd();
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 撃破で移動再開
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerExit2D(Collider2D col)
    {
        //ユニットを破壊した時の処理
        if (col.gameObject.GetComponent<Unit>())
        {
            unitScript.encounter = false;
        }

        //基地を離れた時の処理
        if (col.gameObject.GetComponent<Base>())
        {
            unitScript.encounter = false;
        }
    }

    void Start()
    {
        //親オブジェクトを代入
        unitObject = transform.root.gameObject;
        //親オブジェクトのスクリプト
        unitScript = unitObject.GetComponent<Unit>();
    }
}
