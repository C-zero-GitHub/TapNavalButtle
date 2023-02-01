using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���{�a��
public class Camera : MonoBehaviour
{
    //�萔
    const float X_Offset = 6.31f;       //��n���J�����Ɏ��܂�悤�ɒ���

    //�ϐ�
    float speed = 10.0f;                //�J�����̃X�s�[�h
    
    //�������肳���ϐ�
    float enemyPosX = 0;
    float playerPosX = 0;
    float Min_X = 0;
    float Max_X = 0;

    GameObject enemyBase;
    GameObject playerBase;

    /// <summary>
    /// �x�[�X�̈ʒu���Q�Ƃ��J�����̈ړ��͈͂����肷��B
    /// </summary>
    void SetCameraMoveRange() {
        //�G�����̃x�[�X��u���ƃJ�����̗��[�����܂�B
        enemyBase = GameObject.Find("Base_Enemy");
        playerBase = GameObject.Find("Base_Player");
        enemyPosX = enemyBase.transform.position.x;
        playerPosX = playerBase.transform.position.x;
        Max_X = (enemyPosX - X_Offset);
        Min_X = (playerPosX + X_Offset);

        //Min��Max�����킩���肷��B����Ă����ꍇ�A���̒l��������B
        if(Min_X >= Max_X)
        {
            Debug.Log("��n�̍��W���ُ�ł��B");
            Min_X = 0;
            Max_X = 10;
        }
    }

    /// <summary>
    /// �L�[���͂ŃJ�����𓮂����B
    /// </summary>
    void CameraMove() {         
        //���E�L�[����
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
