using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageAndKnockback : MonoBehaviour
{

    private Rigidbody2D _rb;
    private Arrow arrow;
    private WeaponStat _WS;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        arrow = FindObjectOfType<Arrow>();
       
    }

    private void Update()
    {
        _WS = FindObjectOfType<WeaponStat>();
    }
    // Update is called once per frame
    public void HitEnemy()
    {
        _rb.AddForceAtPosition(arrow.dir*100f*_WS.WeaponForce,arrow.hitEnemy.point);
        Debug.Log("Hi");
    }
}
