using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AttackMenu : MonoBehaviour
{

    [SerializeField]
    internal GameObject attackerPreview;

    [SerializeField]
    internal GameObject defenderPreview;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(Unit attacker, Unit defender)
    {
        Sprite attackerSprite = attacker.gameObject.GetComponent<SpriteRenderer>().sprite;
        Sprite defenderSprite = defender.gameObject.GetComponent<SpriteRenderer>().sprite;

        Image attackerImage = attackerPreview.transform.Find("Image").GetComponent<Image>();
        Image defenderImage = defenderPreview.transform.Find("Image").GetComponent<Image>();
        attackerImage.sprite = attackerSprite;
        defenderImage.sprite = defenderSprite;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
