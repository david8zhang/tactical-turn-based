using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneUnit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Attacker")
        {
            gameObject.GetComponent<Animator>().SetBool("isHit", true);
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
