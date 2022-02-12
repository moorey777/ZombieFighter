using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Text fillText;


    public float GetHealth() {
        return slider.value;
    }

    public float GetMaxHealth() {
        return slider.maxValue;
    }

    public void SetMaxHealth(float health) {
        slider.maxValue = health;
        slider.value = 0.0f;
        fill.color = gradient.Evaluate(1f);
        fillText.text = "0/" + GetMaxHealth().ToString();

    }

    public void SetHealth(float health) {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        fillText.text = health.ToString() + "/" + GetMaxHealth().ToString();
    }
}
