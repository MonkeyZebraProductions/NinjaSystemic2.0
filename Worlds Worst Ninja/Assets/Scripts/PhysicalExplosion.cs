using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalExplosion : MonoBehaviour
{

    public float force;

    public float FOI;

    public LayerMask hitLayer;

    public Vector2 ExplosionDirection;

    private PlayerMovement _pm;
    // Start is called before the first frame update
    void Start()
    {
        _pm = FindObjectOfType<PlayerMovement>();
        Destroy(this.gameObject, 0.2f);
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
            ExplosionDirection = obj.transform.position - transform.position;
            //_pm._isExplosion = true;
            //_pm.ExplosionMultiplier = 1;
            obj.GetComponent<Rigidbody2D>().AddForce(ExplosionDirection * force);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FOI);
    }
}
