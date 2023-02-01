using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
//�萔
�e�o���b�g�̓����ƂȂ邽�ߒ萔�Ő錾�B
    Bullet_Sea   = 1.0f;    �ʏ푬�x
    Bullet_InSea = 0.5f;    �C���̂��ߌ���
    Bullet_Sky   = 1.5f;    �󒆂̈׍���
*/
public class Bullet : MonoBehaviour
{
    //�萔


    /// <summary>�C��/�C��/�󒆂̂����ꂩ</summary>
    public enum ClassType
    {
        Sea,
        InSea,
        Sky
    }
    [System.NonSerialized]
    public ClassType classType;

    //�����p�ϐ�
    Vector3 destroyPos;           //�������̍��W.x+DestroyPosPlus.x

    //��{�X�e�[�^�X/Unit���猈��
    //[System.NonSerialized]
    public bool enemyBullet;      //�G�̏ꍇtrue
    //[System.NonSerialized]
    public float speed;           //�O�i���x
    //[System.NonSerialized]
    public float atk;             //�U����
    //[System.NonSerialized]
    public float destroyDistance; //���̋����i�񂾂�폜
    //�o�t/Unit���猈��
    //[System.NonSerialized]
    public float atk_Buff;        //�U���̓v���X�l

    //�Q�[���I�u�W�F�N�g
    Rigidbody2D rb;

    /// <summary>
    /// �����蔻�菈��
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D col)
    {
        //���j�b�g�ɓ����������̏���
        if (col.gameObject.GetComponent<Unit>())
        {
            col.gameObject.GetComponent<Unit>().Damage(atk);
            Destroy(gameObject);
        }

        //��n�ɓ����������̏���
        if (col.gameObject.GetComponent<Base>())
        {
            col.gameObject.GetComponent<Base>().Damage(atk);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g��i�܂��鏈��
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
        //�G�l�~�[�Ȃ�t�����ɐi�ށB
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
            case ClassType.Sea:     //�C��
                MoveBullet();
                break;

            case ClassType.InSea:   //�C��
                MoveBullet();
                break;

            case ClassType.Sky:     //��
                MoveBullet();
                break;
        }


        //�펞����
        DestroyBullet();

    }
}