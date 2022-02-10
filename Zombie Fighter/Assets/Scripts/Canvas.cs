using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    public Text goneText;

    private void Awake() {
        goneText.text = "0";
    }

    public void GoneUpdate() {
        int getText = int.Parse(goneText.text);
        getText++;
        goneText.text = getText.ToString();    
    }
}
