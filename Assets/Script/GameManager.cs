using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//福本和也

public class GameManager : MonoBehaviour
{
    //ゲームオブジェクト
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

    //変数
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
        //プレイヤー勝利
        if (num == 0)
        {
            
            ScoreCalculation();
            scoreTimeText.  GetComponent<Text>().text = "掛かった時間:" + (int)gameTimeCnt + "秒";
            scoreBaseHpText.GetComponent<Text>().text = "基地残存HP:" + (int)((playerBaseScript.hp / playerBaseMaxHp)*100) + "%";
            gameScoreText.  GetComponent<Text>().text = "スコア:" + gameScore;

            gameBgm.SetActive(false);
            winText.SetActive(true);
            winBgm.SetActive(true);
            Time.timeScale = 0.0f;
        }
        //エネミー勝利
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
