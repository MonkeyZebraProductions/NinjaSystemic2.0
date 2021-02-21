using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private Controls inputs;

    public Transform LookTarget;

    private float maxDirX, maxDirY;

    public float angle;

    public LayerMask WhatIsGround;

    public Vector2 dir;

    private bool _hitground,_hitWall;

    public GameObject Debris,Sound;

    private RaycastHit2D _hit;

    private void Awake()
    {
        inputs = new Controls();
        inputs.Player.Fire.started += context => CreateDebris();
    }
    // Start is called before the first frame update
    void Start()
    {
        maxDirX = 10f;
        maxDirY = 10f;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = inputs.Player.Look.ReadValue<Vector2>();

        
        var pos = Camera.main.WorldToScreenPoint(transform.position);
        dir =  (mousePosition- pos)*0.1f;
        
        if(dir.y>maxDirY)
        {
            dir.y = maxDirY;
        }
        if (dir.y < -maxDirY)
        {
            dir.y = -maxDirY;
        }
        if (dir.x > maxDirX)
        {
            dir.x = maxDirX;
        }
        if (dir.x < -maxDirX)
        {
            dir.x = -maxDirX;
        }
        angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg)-90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        _hitground = Physics2D.Raycast(transform.position, dir,10f,WhatIsGround);
        _hit= Physics2D.Raycast(transform.position, dir, 10f, WhatIsGround);

        Debug.DrawRay(transform.position, dir, Color.green);
        
    }


    private void CreateDebris()
    {
        Instantiate(Sound, transform.position, Quaternion.identity);
        if(_hitground==true)
        {
            Instantiate(Debris, _hit.point, Quaternion.identity);
            Instantiate(Sound, _hit.point, Quaternion.identity);
        }
        
        
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }
}
