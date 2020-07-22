using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneUnit : MonoBehaviour
{

    [SerializeField]
    HealthBar healthbar;
    GameObject unitObjectRef;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
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
            unitObjectRef.GetComponent<Unit>().TakeDamage(10);
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
