using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AttackMenu : MonoBehaviour
{

    [SerializeField]
    internal UnitPreview attackerPreview;

    [SerializeField]
    internal UnitPreview defenderPreview;

    public void Show(Unit attacker, Unit defender)
    {
        SetSprites(attacker, defender);
        SetStats(attacker, defender);
        SetHealth(attacker, defender);
        gameObject.SetActive(true);
    }

    public void SetSprites(Unit attacker, Unit defender)
    {
        attackerPreview.SetSprite(attacker);
        defenderPreview.SetSprite(defender);
    }

    public void SetStats(Unit attacker, Unit defender)
    {
        attackerPreview.SetAttackValue(attacker.attack);
        attackerPreview.SetDefenseValue(attacker.defense);

        defenderPreview.SetAttackValue(defender.attack);
        defenderPreview.SetDefenseValue(defender.defense);
    }

    public void SetHealth(Unit attacker, Unit defender)
    {
        attackerPreview.SetHealth(attacker.health, attacker.maxHealth);

        int healthWithDamage = defender.health - 50;
        defenderPreview.SetDamagePreview(healthWithDamage, defender.health);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
