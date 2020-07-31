using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitPreview : MonoBehaviour
{
    [SerializeField]
    internal HealthBar healthbar;

    [SerializeField]
    internal GameObject attackValue;

    [SerializeField]
    internal GameObject defenseValue;

    [SerializeField]
    internal Image image;

    public void SetSprite(Unit reference)
    {
        Sprite sprite = reference.gameObject.GetComponent<SpriteRenderer>().sprite;
        image.sprite = sprite;
    }

    public void SetHealth(int healthMinusDamage, int maxHealth) {
        healthbar.SetMaxHealth(maxHealth);
        healthbar.SetHealth(healthMinusDamage);
    }

    public void SetDamagePreview(int healthMinusDamage, int maxHealth)
    {
        healthbar.SetMaxHealth(maxHealth);
        healthbar.SetDamagePreview(healthMinusDamage);
    }

    public void SetAttackValue(int value)
    {
        attackValue.GetComponent<TextMeshProUGUI>().SetText(value.ToString());
    }

    public void SetDefenseValue(int value)
    {
        defenseValue.GetComponent<TextMeshProUGUI>().SetText(value.ToString());
    }
}
