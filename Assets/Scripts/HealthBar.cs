using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    Slider slider;

    [SerializeField]
    Gradient gradient;

    [SerializeField]
    Image fill;

    [SerializeField]
    GameObject background;

    public void Start()
    {
        if (background != null)
        {
            background.SetActive(false);
        }
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        //fill.color = gradient.Evaluate(1f);
    }

    public void SetDamagePreview(int healthPreview)
    {
        background.SetActive(true);
        slider.value = healthPreview;
    }
}
