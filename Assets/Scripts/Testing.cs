using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private Transform pfDamagePopup;

    // Start is called before the first frame update
    void Start()
    {
        //DamagePopup.Create(Vector3.zero, 100);
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    bool isCriticalHit = Random.Range(0, 100) < 30;
        //    DamagePopup.Create(UtilsClass.GetMouseWorldPosition(), 100, isCriticalHit);
        //}
    }
}
