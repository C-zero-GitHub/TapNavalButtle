using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//���{�a��
/// <summary>
/// �e�L�X�g�\��
/// �R�X�g�Ǘ�
/// �v���C���[����
/// �G�l�~�[��������
/// 
/// </summary>
public class UnitSpawn : MonoBehaviour
{
    //�萔
    const int   UnitObjectLimit         = 3;       //�Ґ����(�ő�l��8�������j�b�g��3�̈׌��݂�3)
    const int   SpawnLimit              = 50;      //�������
    const float IntervalSpeed           = 5;       //�C���^�[�o���X�s�[�h
    const float TextTimer               = 10;      //�e�L�X�g�\������/�萔

    //�v���C�x�[�g�����p�ϐ�
    float PlayerCostUpSpeed = 10;             //�v���C���[�R�X�g�A�b�v�Ԋu
    float EnemyCostUpSpeed  = 7;              //�G�l�~�[�R�X�g�A�b�v�Ԋu
    float enemySpawnCost;                     //�G�l�~�[�c��R�X�g
    float intervalTextTimer;                  //�e�L�X�g�\������/�����p
    float noCostTextTimer;                    //�e�L�X�g�\������/�����p
    float warningTextTimer = 0;               //���ꏢ���e�L�X�g�\������/�����p
    float spSpawnEnemyBaseHp = 100;           //�G�l�~�[�̊�n�ő�HP�̔���/���ꏢ���������Ɏg�p
    bool  spSpawnFlg         = false;         //�G�l�~�[���ꏢ���������ׂ̃t���O
    int   spSpawnUnitNum     = 3;             //�G�l�~�[���ꏢ�����郆�j�b�g�̐�
    int   antiSpSpawnPlayerCostSpeedUp = 3;   //���ꏢ���ɑ΍R���邽�߂̃R�X�g�A�b�v
    float enemyAutoSpawnInterval = 20;        //�G�l�~�[�̏����Ԋu/�^�C�����C���g�����炢��Ȃ��Ȃ邩��
    float enemyAutoSpawnTimer = 0;            //�G�l�~�[�����Ԋu/��Ɠ���

    //�p�u���b�N�����p�ϐ�
    [System.NonSerialized]
    public float playerSpawnCnt = 0;          //�v���C���[������
    [System.NonSerialized]
    public float enemySpawnCnt = 0;           //�G�l�~�[������
    [Header("�v���C���[�c��R�X�g")]
    public float playerSpawnCost;             //�v���C���[�c��R�X�g 

    //�O���擾�ϐ�
    [System.NonSerialized]
    public float[] playerSpawnInterval        //�v���C���[���j�b�g�����Ԋu 
        = new float[UnitObjectLimit];
    [System.NonSerialized]
    public float[] enemySpawnInterval         //�G�l�~�[���j�b�g�����Ԋu
        = new float[UnitObjectLimit];

    //�Q�[���I�u�W�F�N�g
    GameObject playerBase;                   //�v���C���[��n
    Base       playerBaseScript;             //�v���C���[��n�X�N���v�g
    GameObject enemyBase;                    //�G�l�~�[��n
    Base       enemyBaseScript;              //�G�l�~�[��n�X�N���v�g
    [Header("��������v���C���[���j�b�g�v���t�@�u")]
    public GameObject[] spawnPlayerObject;   //�v���C���[���j�b�g
    [Header("��������G�l�~�[���j�b�g�v���t�@�u")]
    public GameObject[] spawnEnemyObject;    //�G�l�~�[���j�b�g
    [System.NonSerialized]
    public Unit[] playerUnitScript           //�v���C���[���j�b�g�X�N���v�g
        = new Unit[UnitObjectLimit];
    [System.NonSerialized]
    public Unit[] enemyUnitScript            //�G�l�~�[���j�b�g�X�N���v�g
        = new Unit[UnitObjectLimit];


    /*
    �X�^�[�g��Get�R���|�[�l���g����B
    GameObjectSet()�ł��B
    */
    public Slider[] spawnIntervalSliders     //���j�b�g���̏����Ԋu�V�����_�[
        = new Slider[UnitObjectLimit];
    public Text       costText;              //�R�X�g�\���e�L�X�g
    public GameObject spawnIntervalText;     //���j�b�g�����Ԋu�s�o�߃e�L�X�g
    public GameObject spawnLimitText;        //���j�b�g��������e�L�X�g
    public GameObject noCostText;            //�R�X�g�s���e�L�X�g
    public GameObject warningText;           //���ꏢ���x��
    public GameObject warningDisplayTimeText;//�x�����ԕ\���K�e�L�X�g

    public AudioSource audioSource;
    public AudioClip   warningSe;

    //���W
    Vector3 playerSkySpawnPos;              //�v���C���[�󃆃j�b�g�����ʒu
    Vector3 playerSeaSpawnPos;              //�v���C���[�C�テ�j�b�g�����ʒu
    Vector3 playerInSeaSpawnPos;            //�v���C���[�C�����j�b�g�����ʒu
    Vector3 enemySkySpawnPos;               //�G�l�~�[�󃆃j�b�g�����ʒu
    Vector3 enemySeaSpawnPos;               //�G�l�~�[�C�テ�j�b�g�����ʒu
    Vector3 enemyInSeaSpawnPos;             //�G�l�~�[�C�����j�b�g������

    Vector3 playerSpawnPos;                 //�v���C���[���������W
    Vector3 enemySpawnPos;                  //�G�l�~�[���������W

    /// <summary>
    /// ����
    /// �������W�⏈���Ɏg���x�[�X�ƃX�N���v�g���Z�b�g
    /// </summary>
    void GameObjectSet() {
        playerBase = GameObject.Find("Base_Player");
        playerBaseScript = playerBase.GetComponent<Base>();
        enemyBase = GameObject.Find("Base_Enemy");
        enemyBaseScript = enemyBase.GetComponent<Base>();
    }

    /// <summary>
    /// �v���C���[
    /// ���j�b�g�����ʒu�Z�b�g
    /// </summary>
    void PlayerUnitSpawnPosSet() 
    {
        if (playerBase == null)
        {
            Debug.Log("�v���C���[�x�[�X��������܂���");
        }
        playerSkySpawnPos = playerBase.transform.GetChild(0).position;
        playerSeaSpawnPos = playerBase.transform.GetChild(1).position;
        playerInSeaSpawnPos = playerBase.transform.GetChild(2).position;
        //print(playerSkySpawnPos);
        //print(playerSeaSpawnPos);
        //print(playerInSeaSpawnPos);
    }

    /// <summary>
    /// �G�l�~�[
    /// ���j�b�g�����ʒu�Z�b�g
    /// </summary>
    void EnemyUnitSpawnPosSet()
    {
        if (enemyBase == null)
        {
            Debug.Log("�G�l�~�[�x�[�X��������܂���");
        }
        enemySkySpawnPos = enemyBase.transform.GetChild(0).position;
        enemySeaSpawnPos = enemyBase.transform.GetChild(1).position;
        enemyInSeaSpawnPos = enemyBase.transform.GetChild(2).position;
        //print(enemySkySpawnPos);
        //print(enemySeaSpawnPos);
        //print(enemyInSeaSpawnPos);
    }

    /// <summary>
    /// ����
    /// �Q�[���I�u�W�F�N�g��Null�`�F�b�N
    /// �X�N���v�g�̃`�F�b�N/�ϐ��̊i�[
    /// </summary>
    void NullCheck() {
        for (int i = 0; i < UnitObjectLimit; i++)
        {
            //�v���C���[
            if (spawnPlayerObject[i] == null)
            {
                Debug.Log("PlayerUnit��" + i + "�Ԗڂ�null�ł��B");
            }
            else
            {
                playerUnitScript[i] = spawnPlayerObject[i].GetComponent<Unit>();
            }

            //�G�l�~�[
            if (spawnEnemyObject[i] == null)
            {
                Debug.Log("EnemyUnit��" + i + "�Ԗڂ�null�ł��B");
            }
            else
            {
                enemyUnitScript[i]    = spawnEnemyObject[i].GetComponent<Unit>();
                enemySpawnInterval[i] = spawnEnemyObject[i].GetComponent<Unit>().spawnInterval;
            }
        }
    }

    /// <summary>
    /// �v���C���[
    /// �Z�b�g���ꂽ���j�b�g�̐����ʒu���N���X�^�C�v�ɉ����ĕύX
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
    /// �v���C���[��p
    /// �����{�^����������
    /// </summary>
    /// <param name="num"></param>
    public void PushSpawnButton(int num)
    {
        //���������艺��
        if (playerSpawnCnt < SpawnLimit )
        {
            //�����Ԋu���߂��Ă��邩
            if (playerSpawnInterval[num] <= 0)
            {
                //�R�X�g�͑���Ă��邩
                if (playerUnitScript[num].cost < playerSpawnCost)
                {
                    //�v���C���[����
                    PlayerSpawnPosSet(num);
                    //�����Ԋu���Z�b�g
                    playerSpawnInterval[num] = playerUnitScript[num].spawnInterval;
                    //�V�����_�[�Ə����Ԋu�𓯊�
                    spawnIntervalSliders[num].value = playerSpawnInterval[num];
                    //�R�X�g���}�C�i�X
                    playerSpawnCost -= playerUnitScript[num].cost;
                    //���j�b�g�𐶐�(�I�u�W�F�N�g�v�[���ɑΉ�����)
                    Instantiate(spawnPlayerObject[num], playerSpawnPos, Quaternion.Euler(0, 0, 0));
                    //�v���C���[�����������v���X
                    playerSpawnCnt++;
                }
                else 
                {
                    //�R�X�g�s���x��
                    noCostTextTimer = TextTimer;
                    noCostText.SetActive(true);
                }
            }
            else
            {
                //�����Ԋu���o�ߌx��
                intervalTextTimer = TextTimer;
                spawnIntervalText.SetActive(true);
            }
        }
    }

    /// <summary>
    /// �G�l�~�[
    /// �Z�b�g���ꂽ���j�b�g�̐����ʒu���N���X�^�C�v�ɉ����ĕύX
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
    /// �G�l�~�[
    /// �����{�^����������
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
    /// �G�l�~�[��p/�^�C�����C���ɐ؂�ւ��\��
    /// �����ɉ����ăG�l�~�[������
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
    /// HP��spSpawnEnemyBaseHp�ȉ��ɂȂ������񂾂��Ăяo���B
    /// ���ꏢ������
    /// </summary>
    void EnemySpSpawn()
    {
        spSpawnFlg = true;
        audioSource.PlayOneShot(warningSe);
        enemyAutoSpawnInterval /= 3;
        PlayerCostUpSpeed += antiSpSpawnPlayerCostSpeedUp;
        warningText.SetActive(true);
        warningTextTimer = TextTimer;
        //�Q�[����x�����鏈��/�����Ȃ̂ň�[���O
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
    /// ����
    /// �����C���^�[�o���J�E���g
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
    /// ����
    /// �R�X�g���Z����
    /// </summary>
    void AddCost() {
        playerSpawnCost += (PlayerCostUpSpeed * IntervalSpeed * Time.deltaTime);
        enemySpawnCost  += (EnemyCostUpSpeed * IntervalSpeed * Time.deltaTime);
    }

    /// <summary>
    /// �e�L�X�g��ʕ\������
    /// </summary>
    void TextDisplay() 
    {
        //�R�X�g
        costText.text = "�R����:" + (int)playerSpawnCost;
        
        //�����C���^�[�o��
        if (intervalTextTimer <= 0)
        {
            spawnIntervalText.SetActive(false);
        }
        else
        {
            intervalTextTimer -= (Time.deltaTime * IntervalSpeed);
        }

        //������
        if (playerSpawnCnt >= SpawnLimit)
        {
            spawnLimitText.SetActive(true);
        }
        else 
        {
            spawnLimitText.SetActive(false);
        }

        //�R�X�g�s��
        if (noCostTextTimer <= 0) 
        {
            noCostText.SetActive(false);
        }
        else 
        {
            noCostTextTimer -= (Time.deltaTime * IntervalSpeed);
        }

        //���ꏢ���x��
        if (warningTextTimer < 0)
        {
            warningText.SetActive(false);
            //Time.timeScale = 1.0f;
        }
        else
        {
            warningTextTimer -= (Time.deltaTime * (IntervalSpeed / 5));
            warningDisplayTimeText.GetComponent<Text>().text = "�\������:" + (int)warningTextTimer;
        }
    }

    void VariableSet() {
        //���ꏢ�������^�C�~���O
        spSpawnEnemyBaseHp = (enemyBaseScript.hp / 2);

        //�X�|�[���Ԋu�̃V�����_�[
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
        //�R�X�g��������
        AddCost();

        //�C���^�[�o��
        SpawnInterval();
        
        //UI�\��
        TextDisplay();

        //�G��������
        AutoEnemySpawn();

        if (Input.GetKeyDown(KeyCode.K))
        {
            //Time.timeScale = 0.0f;
        }

        /*
        �R�X�g�X�s�[�h����������/�f�o�b�O�p
        if (Input.GetKeyDown(KeyCode.I)) {
            if (PlayerCostUpSpeed < 100)
            {
                PlayerCostUpSpeed += 5;
            }
        }
        */

        /*�G�l�~�[�f�o�b�O
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