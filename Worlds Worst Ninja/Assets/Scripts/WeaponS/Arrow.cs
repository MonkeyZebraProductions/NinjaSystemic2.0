using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private Controls inputs;

    public Transform LookTarget;

    private float maxDirX, maxDirY, maxRadius;

    public float angle;

    public LayerMask WhatIsGround;

    public Vector2 dir, point;

    private bool _hitground, _hitWall;

    public GameObject Debris, Sound;

    private RaycastHit2D _hit;

    private bool _create;

    private PlayerMovement _pm;

    private WeaponStat _WS;

    private void Awake()
    {
        inputs = new Controls();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        _pm = FindObjectOfType<PlayerMovement>();
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _WS = FindObjectOfType<WeaponStat>();
        maxRadius = _WS.WeaponRange;
        Sound = _WS.Sound;
        Vector2 mousePosition = inputs.Player.Look.ReadValue<Vector2>();


        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        dir = (mousePosition - pos);
        dir.Normalize();



        angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        _hitground = Physics2D.Raycast(transform.position, dir, maxRadius, WhatIsGround);
        _hit = Physics2D.Raycast(transform.position, dir, maxRadius, WhatIsGround);

        Debug.DrawRay(transform.position, dir * maxRadius, Color.green);



    }


    public void CreateDebris()
    {

        Instantiate(Sound, transform.position, Quaternion.identity);
        if (_hitground == true)
        {
            Instantiate(Debris, _hit.point, Quaternion.identity);
            Instantiate(Sound, _hit.point, Quaternion.identity);
        }

        if (_WS.IsExplosive)
        {
            Instantiate(_WS.Rocket, transform.position, Quaternion.identity);
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
