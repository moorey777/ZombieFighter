using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    public Text goneText;
    public Text castleText;
    

    void Start() {
        
    }

    private void Awake() {
        
    }

    public void GoneUpdate() {
        int getText = int.Parse(goneText.text);
        getText++;
        goneText.text = getText.ToString();   
    }
}
