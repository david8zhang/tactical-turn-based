using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneUnit : MonoBehaviour
{

    [SerializeField]
    HealthBar healthbar;
    public GameObject unitObjectRef;


    // Attack animation references
    Vector3 oldAttackerPosition;
    Vector3 attackerWindUp;
    bool isAttacking;
    float attackCountdown = 2.0f;
    GameObject defender;

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            PlayAttackAnimation();
        }
    }

    internal void PlayAttackAnimation()
    {
        // Move Attacker forward
        attackCountdown -= Time.deltaTime;
        Vector3 targetPosition = defender.transform.position;
        float forwardStep = 35 * Time.deltaTime;
        float backwardStep = 5 * Time.deltaTime;

        Vector3 currentPosition = gameObject.transform.position;

        // Wind up animation
        if (attackCountdown > 1.0f && attackCountdown <= 2.0f)
        {
            gameObject.transform.position = Vector3.MoveTowards(currentPosition, attackerWindUp, backwardStep);
        }

        // Overshoot and pullback
        if (attackCountdown > 0.8f && attackCountdown <= 1.0f)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, forwardStep);
        }
        else if (attackCountdown > 0.5f && attackCountdown <= 0.7f)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, oldAttackerPosition, forwardStep);
        }
        if (attackCountdown <= 0.5f)
        {
            gameObject.GetComponent<Animator>().SetBool("isAttacking", false);
        }
    }

    public void ResetAttacker()
    {
        oldAttackerPosition = new Vector3();
        attackerWindUp = new Vector3();
        isAttacking = false;
        attackCountdown = 2.0f;
        defender = null;
    }

    public void Attack(CutsceneUnit defenderUnit)
    {
        gameObject.GetComponent<Animator>().SetBool("isAttacking", true);
        oldAttackerPosition = gameObject.transform.position;
        float xPos = gameObject.GetComponent<SpriteRenderer>().flipX ? 1f : -1f;
        attackerWindUp = gameObject.transform.position + new Vector3(xPos, 0, 0);
        defender = defenderUnit.gameObject;
        isAttacking = true;
    }

    public void SetUnitObjRef(GameObject unitObj)
    {
        unitObjectRef = unitObj;
        gameObject.GetComponent<Animator>().runtimeAnimatorController = unitObj.GetComponent<Animator>().runtimeAnimatorController;
        healthbar.SetMaxHealth(unitObj.GetComponent<Unit>().maxHealth);
        healthbar.SetHealth(unitObj.GetComponent<Unit>().health);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (isAttacking)
        {
            col.gameObject.GetComponent<Animator>().SetBool("isHit", true);
            col.gameObject.GetComponent<CutsceneUnit>().TakeDamage(50);
        }
    }

    public void TakeDamage(int damage)
    {
        unitObjectRef.GetComponent<Unit>().TakeDamage(damage);
        healthbar.SetHealth(unitObjectRef.GetComponent<Unit>().health);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (isAttacking)
        {
            other.gameObject.GetComponent<Animator>().SetBool("isHit", false);
        }
    }
}
