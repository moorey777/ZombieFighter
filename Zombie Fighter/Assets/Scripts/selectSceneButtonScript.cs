using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class selectSceneButtonScript : MonoBehaviour
{
    public Sprite buttonSprite;

    Image btn1,btn2,btn3;

    private void Awake()
    {
      btn1 = GameObject.Find("Canvas/SafeAreaPanel/selectPanelBG/Level1Button").GetComponent<Image>();
      btn2 = GameObject.Find("Canvas/SafeAreaPanel/selectPanelBG/Level2Button").GetComponent<Image>();
      btn3 = GameObject.Find("Canvas/SafeAreaPanel/selectPanelBG/Level3Button").GetComponent<Image>();

      int clearLvl = 2;

      if(clearLvl == 0)
      {
        btn1.sprite = buttonSprite;
      }
      else if(clearLvl <= 1)
      {
        btn1.sprite = buttonSprite;
        btn2.sprite = buttonSprite;
      }
      else if(clearLvl >= 2)
      {
        btn1.sprite = buttonSprite;
        btn2.sprite = buttonSprite;
        btn3.sprite = buttonSprite;
      }
    }

    public void GoToLv1()
    {
      SceneManager.LoadScene("Level1");
    }

    public void GoToLv2()
    {
      SceneManager.LoadScene("Level2");
    }

    public void GoToLv3()
    {
      SceneManager.LoadScene("Level3");
    }

    public void GoToMain()
    {
      SceneManager.LoadScene("MainMenu");
    }
}
