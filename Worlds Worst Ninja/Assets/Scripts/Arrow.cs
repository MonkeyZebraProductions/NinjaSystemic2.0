using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private Controls inputs;

    public Transform LookTarget;

    private float maxDirX, maxDirY,maxRadius;

    public float angle;

    public LayerMask WhatIsGround;

    public Vector2 dir,point;

    private bool _hitground,_hitWall;

    public GameObject Debris,Sound;

    private RaycastHit2D _hit;

    private bool _create;

    private PlayerMovement _pm;

    private WeaponStat _WS;

    private void Awake()
    {
        inputs = new Controls();
        inputs.Player.Fire.started += context => CreateDebris();
        inputs.Player.Fire.canceled += context => Stop();
    }
    // Start is called before the first frame update
    void Start()
    {
        _pm = FindObjectOfType<PlayerMovement>();
        _WS = FindObjectOfType<WeaponStat>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        maxRadius = _WS.WeaponRange;

        Vector2 mousePosition = inputs.Player.Look.ReadValue<Vector2>();


        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        dir =  (mousePosition- pos);
        dir.Normalize();

        
        
        angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg)-90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        _hitground = Physics2D.Raycast(transform.position, dir, maxRadius, WhatIsGround);
        _hit= Physics2D.Raycast(transform.position, dir, maxRadius, WhatIsGround);

        Debug.DrawRay(transform.position, dir*maxRadius, Color.green);
        Debug.Log(point);
        if(_create==true)
        {
            Instantiate(Sound, transform.position, Quaternion.identity);
            if (_hitground == true)
            {
                Instantiate(Debris, _hit.point, Quaternion.identity);
                Instantiate(Sound, _hit.point, Quaternion.identity);
            }
        }
        
    }


    private void CreateDebris()
    {
        if(_pm._canFire==true&&_pm._isAuto==true)
        {
            _create = true;
        }
        else if(_pm._isFiring)
        {
            Instantiate(Sound, transform.position, Quaternion.identity);
            if (_hitground == true)
            {
                Instantiate(Debris, _hit.point, Quaternion.identity);
                Instantiate(Sound, _hit.point, Quaternion.identity);
            }
        }
    }

    private void Stop()
    {
        _create = false;
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
