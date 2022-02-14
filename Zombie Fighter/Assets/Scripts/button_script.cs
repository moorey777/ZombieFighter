using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class button_script : MonoBehaviour
{
  public GameObject selectPanel,stopButton,LevelSelectBtn,mainBtn,replayBtn;

  public void setSelectPanelOn()
  {
    selectPanel.SetActive(true);
    Time.timeScale = 0f;
  }

  public void setSelectPanelOff()
  {
    selectPanel.SetActive(false);
    Time.timeScale = 1.0f;
  }

  public void setStopButtonOn()
  {
    stopButton.SetActive(true);
  }

  public void setStopButtonOff()
  {
    stopButton.SetActive(false);
  }

  public void playButton()
  {
    GameObject playB = GameObject.Find("Canvas/SafeAreaPanel/PlayButton");
    playB.SetActive(false);

    SceneManager.LoadScene("LevelSelect");
  }

  public void replay()
  {
    string sceneName = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene(sceneName);
    Time.timeScale = 1.0f;
  }

  public void levelMenu()
  {
    SceneManager.LoadScene("LevelSelect");
    Time.timeScale = 1.0f;
  }

  public void GoToMain()
  {
    SceneManager.LoadScene("MainMenu");
    Time.timeScale = 1.0f;
  }
}
