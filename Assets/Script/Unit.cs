using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//���{�a��
//�K�[�h�l�ǉ��������B

public class Unit : MonoBehaviour
{
    //�ő�/�ŏ��X�e�[�^�X
    const float Min_atkInterval = 0.1f;     //�U���Ԋu�ŏ��l
    const float Min_cost        = 1;        //�R�X�g�ŏ��l
    const float Max_speed       = 10;       //�X�s�[�h�ő�l
    const float Max_hp          = 9999;     //�ϋv�ő�l

    //�萔
    
    /// <summary>�C��/�C��/�󒆂̂����ꂩ</summary>
    public enum ClassType
    {
        Sea,
        InSea,
        Sky
    }
    public ClassType classType;

    //�Q�[���I�u�W�F�N�g
    public GameObject[] bullet;             //�e�I�u�W�F�N�g
    public Bullet[] bulletScript;           //�e�X�N���v�g
    Rigidbody2D   rb;                       //��������
    public Slider hpSlider;                 //HP�o�[
    public UnitSpawn unitSpawn;             //���j�b�g�X�|�i�[
    UnitSensor antiUnitSensor;              //���j�b�g�Z���T�[
    UnitSensor antiBaseSensor;              //�x�[�X�Z���T�[

    //��{�X�e�[�^�X
    public bool  enemyUnit     = false;     //�G�l�~�[���j�b�g�t���O
    public float b_atk         = 3;         //�U����/Bullet��
    public float cost          = 100;       //�����R�X�g
    public float speed         = 1.0f;      //���j�b�g�̐i�R���x
    public float atkInterval   = 5.0f;      //�e�̔��ˊԊu
    public float hp            = 30.0f;     //���j�b�g�̑ϋv�l
    public float spawnInterval = 3.0f;      //�X�|�[���Ԋu

    //�o�t�l
    public int   cost_Buff          = 0;     //�R�X�g����������ϐ�
    public float speed_Buff         = 0.0f;  //�i�R���x�o�t
    public float atkInterval_Buff   = 0.0f;  //���ˊԊu�Z�k
    public float hp_Buff            = 0.0f;  //�ϋv�l�A�b�v
    public float spawnInterval_Buff = 0.0f;  //�X�|�[���Ԋu�Z�k
    public float b_atk_Buff         = 0.0f;  //�U���̓v���X�l/Bullet��

    //����X�e�[�^�X���p
    public int Change_Bullet = 10;           //�����������狭���e�𔭎˂��邩�B
    public float StrongBulletBuff = 1.5f;    //�����e�̃o�t�l

    //����X�e�[�^�X��


    //�����p�ϐ�
    float setAtkInterval;               //�C���^�[�o��������

    //�O���A�N�Z�X�p
    public bool encounter = false;

    //�v���C�x�[�g�ϐ�
    int nowBullet  = 0;                  //���݂̔��˒e�Ǘ�


    /// <summary>
    /// �ړ�����
    /// </summary>
    void MoveUnit()
    {
        Vector3 now = rb.position;
        now += new Vector3((speed * Time.deltaTime), 0.0f, 0.0f);
        rb.position = now;
    }

    /// <summary>
    /// �e�𔭎Ԃ���֐��B
    /// 3��01������02����񔭎˂���B
    /// ���݂͐����őΉ����邪�A�N�e�B�u�őΉ��������B
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
    /// hp��0�ȉ��ɂȂ�����폜
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
    /// �ړG������/�U���Ԋu��0�ɂȂ�����U�����^�C�}�[������������B
    /// </summary>
    void Encounter() {
        atkInterval -= Time.deltaTime;
        if (atkInterval <= 0) {
            Atack();
            atkInterval = setAtkInterval;
        }
    }

    /// <summary>
    /// �O���A�N�Z�X�̂�/�_�[���[�W���󂯂鏈��/Bullet����A�N�Z�X�����B
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(float damage)
    {
        hp -= damage;

        hpSlider.value = hp;
    }

    /// <summary>
    /// �����p�̃X�N���v�g���Z�b�g
    /// </summary>
    void ScriptSet() {
        unitSpawn = GameObject.Find("UnitSpawn").GetComponent<UnitSpawn>();
        antiUnitSensor = transform.GetChild(1).gameObject.GetComponent<UnitSensor>();
        antiBaseSensor = transform.GetChild(2).gameObject.GetComponent<UnitSensor>();
    }

    /// <summary>
    /// �Z���T�[�̃^�[�Q�b�g���Z�b�g
    /// </summary>
    void SensorTarget() {
        antiUnitSensor.SensorTargetSet(0);      //���j�b�g�Z���T�[
        antiBaseSensor.SensorTargetSet(1);      //��n�Z���T�[
    }

    /// <summary>
    /// �����p�̕ϐ����Z�b�g
    /// </summary>
    void VariableSet() {
        //HP�o�[�����p
        hpSlider.minValue = 0;
        hpSlider.maxValue = hp;
        hpSlider.value = hp;

        //�����p�ϐ������l
        setAtkInterval = atkInterval;

        //�����������Z�b�g
        rb = this.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// �o�t�̐������`�F�b�N�y�уX�e�[�^�X�ω�/�f�[�^���������΍�
    /// </summary>
    void StatusCheck() {
        //�}�C�i�X�̒l���Ȃ����m�F
        if (speed            < 0) speed            *= -1;
        if (speed_Buff       < 0) speed_Buff       *= -1;
        if (cost             < 0) cost             *= -1;
        if (cost_Buff        < 0) cost_Buff        *= -1;
        if (hp               < 0) hp               *= -1;
        if (hp_Buff          < 0) hp_Buff          *= -1;
        if (atkInterval      < 0) atkInterval      *= -1;
        if (atkInterval_Buff < 0) atkInterval_Buff *= -1;

        
        //�U���Ԋu
        if ((atkInterval - atkInterval_Buff) <= Min_atkInterval)
        {
            atkInterval = Min_atkInterval;
            Debug.Log("�U���Ԋu���ŏ��̒l�ɐݒ肳��܂����B");
        }
        else {
            atkInterval -= atkInterval_Buff;
        }
        
        //�R�X�g
        if ((cost - cost_Buff) <= Min_cost)
        {
            cost = Min_cost;
            Debug.Log("�R�X�g���ŏ��̒l�ɐݒ肳��܂����B");
        }
        else
        {
            cost -= cost_Buff;
        }
        
        if (speed + speed_Buff >= Max_speed)
        {
            speed = Max_speed;
            Debug.Log("�X�s�[�h���ő�̒l�ɐݒ肳��܂����B");
        }
        else
        {
            speed += speed_Buff;
        }

        if (hp + hp_Buff >= Max_hp)
        {
            hp = Max_hp;
            Debug.Log("HP���ő�̒l�ɐݒ肳��܂����B");
        }
        else
        {
            hp += hp_Buff;
        }

        //�G�l�~�[�̃X�s�[�h�̂ݔ��]
        if (enemyUnit)
        {
            speed *= -1;
        }

    }

    /// <summary>
    /// �X�N���v�g�Z�b�g
    /// </summary>
    void BulletStatusSet() {
        for (int i = 0; i < 2; i++) {
            //�e�X�N���v�g���Z�b�g
            bulletScript[i] = bullet[i].GetComponent<Bullet>();
            //�e�Ƀ��j�b�g��2�{�̃X�s�[�h���Z�b�g
            bulletScript[i].speed = (speed * (2f));
            //�G���j�b�g�ł���ꍇ�e�̃t���O���ύX
            bulletScript[i].enemyBullet = enemyUnit;
            if (i == 1)
            {
                //���U���l���Z�b�g
                bulletScript[i].atk = (b_atk * StrongBulletBuff);
            }
            else
            {
                //�ʏ�U���l���Z�b�g
                bulletScript[i].atk = b_atk;
            }
            bulletScript[i].atk_Buff = b_atk_Buff;
        }
    }

    void Start()
    {
        //�X�e�[�^�X�������`�F�b�N
        StatusCheck();

        //�e�I�u�W�F�N�g/�X�e�[�^�X�Z�b�g
        BulletStatusSet();

        //�X�N���v�g���Z�b�g
        ScriptSet();

        //�Z���T�[�̃^�[�Q�b�g���Z�b�g
        SensorTarget();

        //�ϐ����Z�b�g
        VariableSet();
    }


    void Update()
    {
        //���̂��J�����𓮂����ƃo�O��̂ŉ�����/UnitSpawn�ɂ�����
        //hpSlider.value = hp;

        if (encounter == true)
        {
            //�ړG����
            Encounter();
        }
        else
        {
            //�ړ�����
            MoveUnit();
        }

        if (hp <= 0)
        {
            //HP��0�ɂȂ�����폜(�I�u�W�F�N�g�v�[���ɑΉ��\��)
            DestroyUnit();
        }
    }
}