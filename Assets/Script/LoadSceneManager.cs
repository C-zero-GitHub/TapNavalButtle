using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//•Ÿ–{˜a–ç
public class LoadSceneManager : MonoBehaviour
{
    public void PushStartButton() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Stage");
    }

    public void PushGameEndButton()
    {
        Application.Quit();
    }
}
