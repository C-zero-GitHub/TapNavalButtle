using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//���{�a��

public class GameManager : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g
    public GameObject playerBase;
    public GameObject enemyBase;
    public Base playerBaseScript;
    public Base enemyBaseScript;
    public GameObject scoreTimeText;
    public GameObject scoreBaseHpText;
    public GameObject gameScoreText;
    public GameObject gameBgm;
    public GameObject winBgm;
    public GameObject winText;
    public GameObject loseText;

    //�ϐ�
    float gameTimeCnt;
    float playerBaseMaxHp;
    bool  gameEnd = false;
    int   gameScore;

    void ObjectSet() {
        playerBase = GameObject.Find("Base_Player");
        playerBaseScript = playerBase.GetComponent<Base>();
        enemyBase = GameObject.Find("Base_Enemy");
        enemyBaseScript = enemyBase.GetComponent<Base>();
    }

    void ScoreCnt() {
        gameTimeCnt += Time.deltaTime;
    }

    void ScoreCalculation() {
        gameScore = (int)((playerBaseScript.hp / playerBaseMaxHp) * 10000);
        gameScore -= ((int)gameTimeCnt * 10);
        if ((int)gameTimeCnt < 200)
        {
            gameScore += 500;
        }
        else if ((int)gameTimeCnt < 150)
        {
            gameScore += 1500;
        }
        else if ((int)gameTimeCnt < 100) {
            gameScore += 3000;
        }
    }

    void GameEnd(int num) {
        gameEnd = true;
        //�v���C���[����
        if (num == 0)
        {
            
            ScoreCalculation();
            scoreTimeText.  GetComponent<Text>().text = "�|����������:" + (int)gameTimeCnt + "�b";
            scoreBaseHpText.GetComponent<Text>().text = "��n�c��HP:" + (int)((playerBaseScript.hp / playerBaseMaxHp)*100) + "%";
            gameScoreText.  GetComponent<Text>().text = "�X�R�A:" + gameScore;

            gameBgm.SetActive(false);
            winText.SetActive(true);
            winBgm.SetActive(true);
            Time.timeScale = 0.0f;
        }
        //�G�l�~�[����
        else 
        {
            loseText.SetActive(true);
            Time.timeScale = 0.0f;
        }


    }

    void VariableSet() {
        gameScore = 0;
        playerBaseMaxHp = playerBaseScript.hp;
    }

    void Start()
    {
        ObjectSet();
        VariableSet();
        gameEnd = false;
        winBgm.SetActive(false);
        winText.SetActive(false);
        loseText.SetActive(false);
    }

    void Update()
    {

        ScoreCnt();
        if (enemyBaseScript.hp <= 0)
        {
            GameEnd(0);
        }
        if (playerBaseScript.hp <= 0)
        {
            GameEnd(1);
        }
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Title");
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
