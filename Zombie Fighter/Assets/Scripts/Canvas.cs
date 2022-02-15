using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    public Text goneText;
    public Text castleText;
    public float lastTime;

    
    void Start() {
        lastTime = Time.time;
        
    }

    private void Awake() {
        
    }

    public void GoneUpdate() {
        int getText = int.Parse(goneText.text);
        getText++;
        goneText.text = getText.ToString();   
    }
    public void debug(object minutes) {
        goneText.text = minutes.ToString();
    }
}
