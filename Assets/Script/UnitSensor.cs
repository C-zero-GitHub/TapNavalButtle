using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���{�a��
public class UnitSensor : MonoBehaviour
{
    GameObject unitObject;
    Unit       unitScript;
    public bool enemyUnit = false;


    /// <summary>
    /// �Z���T�[�^�C�v
    /// ���j�b�g����Ǘ�
    /// </summary>
    public enum SensorType
    {
        Unit,
        Base
    }
    public SensorType sensorType;

    /// <summary>
    /// Unit����A�N�Z�X�B�^�[�Q�b�g���Z�b�g�B
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
    /// �x�[�X�܂ōs������Unit��Speed��0�ɂ���B
    /// </summary>
    void MoveEnd() {
        unitScript.speed = 0;
    }

    /// <summary>
    /// �ΖʂŒ�~����OnTriggerEnter
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerStay2D(Collider2D col)
    {
        switch (sensorType) {
            case SensorType.Unit:
                if (enemyUnit == false)
                {
                    //�G���j�b�g�ɓ����������̏���
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
                    //�������j�b�g�ɓ����������̏���
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
                    //�G�x�[�X�ɓ����������̏���
                    if (col.gameObject.tag == "Enemy_Base")
                    {
                        unitScript.encounter = true;
                        MoveEnd();
                    }
                }

                if (enemyUnit == true)
                {
                    //�����x�[�X�ɓ����������̏���
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
    /// ���j�ňړ��ĊJ
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerExit2D(Collider2D col)
    {
        //���j�b�g��j�󂵂����̏���
        if (col.gameObject.GetComponent<Unit>())
        {
            unitScript.encounter = false;
        }

        //��n�𗣂ꂽ���̏���
        if (col.gameObject.GetComponent<Base>())
        {
            unitScript.encounter = false;
        }
    }

    void Start()
    {
        //�e�I�u�W�F�N�g����
        unitObject = transform.root.gameObject;
        //�e�I�u�W�F�N�g�̃X�N���v�g
        unitScript = unitObject.GetComponent<Unit>();
    }
}
