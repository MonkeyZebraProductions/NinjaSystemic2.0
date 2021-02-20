using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalExplosion : MonoBehaviour
{

    public float force;

    public float FOI;

    public LayerMask hitLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Explode();
    }

    void Explode()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, FOI, hitLayer);

        foreach(Collider2D obj in objects)
        {
            Vector2 dir = obj.transform.position - transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(dir * force);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FOI);
    }
}
