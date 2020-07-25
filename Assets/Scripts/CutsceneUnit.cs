using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneUnit : MonoBehaviour
{

    [SerializeField]
    HealthBar healthbar;
    GameObject unitObjectRef;


    // Attack animation references
    Vector3 oldAttackerPosition;
    Vector3 windupPosition;
    bool isAttacking;
    float countDown = 2.0f;
    GameObject defender;

    // Start is called before the first frame update
    void Start()
    {
    }

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
        countDown -= Time.deltaTime;
        Vector3 targetPosition = defender.transform.position;
        float forwardStep = 35 * Time.deltaTime;
        float backwardStep = 5 * Time.deltaTime;

        Vector3 currentPosition = gameObject.transform.position;

        // Wind up animation
        if (countDown > 1.0f && countDown <= 2.0f)
        {
            gameObject.transform.position = Vector3.MoveTowards(currentPosition, windupPosition, backwardStep);
        }

        // Overshoot and pullback
        if (countDown > 0.8f && countDown <= 1.0f)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, forwardStep);
        }
        else if (countDown > 0.5f && countDown <= 0.7f)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, oldAttackerPosition, forwardStep);
        }
    }

    public void Reset()
    {
        oldAttackerPosition = new Vector3();
        windupPosition = new Vector3();
        isAttacking = false;
        countDown = 2.0f;
        defender = null;
    }

    public void Attack(CutsceneUnit defenderUnit)
    {
        oldAttackerPosition = gameObject.transform.position;
        windupPosition = gameObject.transform.position + new Vector3(-1, 0, 0);
        defender = defenderUnit.gameObject;
        isAttacking = true;
    }

    public void SetUnitObjRef(GameObject unitObj)
    {
        unitObjectRef = unitObj;
        gameObject.GetComponent<Animator>().runtimeAnimatorController = unitObj.GetComponent<Animator>().runtimeAnimatorController;
        if (gameObject.name == "Attacker")
        {
            gameObject.GetComponent<Animator>().SetBool("isAttacking", true);
        }
        healthbar.SetMaxHealth(unitObj.GetComponent<Unit>().maxHealth);
        healthbar.SetHealth(unitObj.GetComponent<Unit>().health);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Attacker")
        {
            gameObject.GetComponent<Animator>().SetBool("isHit", true);
            unitObjectRef.GetComponent<Unit>().TakeDamage(50);
            healthbar.SetHealth(unitObjectRef.GetComponent<Unit>().health);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Attacker")
        {
            gameObject.GetComponent<Animator>().SetBool("isHit", false);
        }
    }
}
