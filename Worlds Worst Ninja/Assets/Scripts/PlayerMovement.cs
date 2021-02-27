using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{

    private Controls inputs;

    [SerializeField] public float MovementSpeed = 5f;
    [SerializeField] public float JumpSpeed = 5f;
    [SerializeField] public float HangTimer = 0.1f;
    [SerializeField] public int MaxJumps = 1;

    public float ExplosionMultiplier = 1;
    public float WallJumpTimer,_wallJumpTime=1;

    public GameObject SelectedWeapon;

    public Transform LootTarget;

    public Camera camera;

    private Vector2 move,look;

    private bool _isGrounded,_canMove,_firingSingle;

    public bool IsRightWalled, IsLeftWalled, IsVisable, _canFire, _isAuto, _isFiring,_isExplosion, _isJumping;

    private float _jumpMultiplyer = 1;

    private float _jumpMultiplyerRate,_reloadTime,_hangTime;

    private int _jumps;

    private Rigidbody2D _rb2D;

    private Arrow arrow;

    private PhysicalExplosion Expl;

    public SpriteRenderer sprite;
    private Color colour;

    private WeaponStat _WS;

    void Awake()
    {
        inputs = new Controls();
        
        inputs.Player.Jump.started += context => Jump();
        inputs.Player.Jump.canceled += context => JumpCancel();
        inputs.Player.Fire.started += context => Fire();
        inputs.Player.Fire.canceled += context => FireCancel();
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        
        arrow = FindObjectOfType<Arrow>();
        
        Expl = FindObjectOfType<PhysicalExplosion>();

        _jumps = MaxJumps;
        _canFire = false;
        _isFiring = false;
        colour = sprite.color;
        _WS = FindObjectOfType<WeaponStat>();
        _reloadTime = _WS.ReloadTime;
        _hangTime = HangTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGrounded)
        {
            _jumps = MaxJumps;
            _wallJumpTime = WallJumpTimer;
        }
        _WS = FindObjectOfType<WeaponStat>();
        _isAuto = _WS.IsAuto;
        if(IsLeftWalled&&_isJumping||IsRightWalled && _isJumping)
        {
            _canMove = false;
        }
        else
        {
            _canMove = true;
        }
        Debug.Log(_firingSingle);
        Debug.Log(_hangTime);
    }

    private void FixedUpdate()
    {
        move = inputs.Player.Move.ReadValue<Vector2>();
        Vector3 mousePosition = inputs.Player.Look.ReadValue<Vector2>();
        //LootTarget.transform.position = new Vector3(look.x, look.y, 0);


        mousePosition.z = 20;
        mousePosition = camera.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;
        LootTarget.position = mousePosition;

        
        //SelectedWeapon.transform.LookAt(LootTarget.transform, Vector2.up);
        if(_canMove)
        {
            _rb2D.velocity = new Vector2(move.x * MovementSpeed, 0);
        }

        //checks if jump button was pressed
        if (_isJumping == true)
        {
            if(IsLeftWalled && _wallJumpTime>0)
            {
               _rb2D.AddForce(new Vector2(JumpSpeed/ 1.5f, JumpSpeed/2 ));
                _wallJumpTime -= Time.deltaTime;
                
            }
            else if (IsRightWalled && _wallJumpTime > 0)
            {
                _rb2D.AddForce(new Vector2(-JumpSpeed/ 1.5f, JumpSpeed/2));
                _wallJumpTime -= Time.deltaTime;
                
            }
            else if (!IsRightWalled && !IsLeftWalled)
            {
                _rb2D.AddForce(new Vector2(0, JumpSpeed * _jumpMultiplyer));
                _jumpMultiplyer *= _jumpMultiplyerRate;
            }
            
            
            if (_jumpMultiplyer <= 0.1f || _wallJumpTime<=0)
            {
                _isJumping = false;
            }
        }


        //Checks if holding fire button
        if(_canFire==true && _isAuto == true)
        {
            _rb2D.AddForce(new Vector2(arrow.dir.x, arrow.dir.y) * 500f *_WS.WeaponForce* -1f);
            arrow.CreateDebris();
        }
        if(_firingSingle)
        {
            _rb2D.AddForce(new Vector2(arrow.dir.x, arrow.dir.y) * _WS.WeaponForce * 500f * -1f);
            _hangTime -= Time.deltaTime;
            if (_hangTime < 0)
            {
                
                _firingSingle = false;
            }
        }
        
        Reload();
        
    }

    private void Jump()
    {
        if(IsRightWalled || IsLeftWalled)
        {
            _jumps += 1;
        }
        if(_jumps>0)
        {
            _isJumping = true;
            _jumpMultiplyer = 1f;
            _jumpMultiplyerRate = 0.9f;
            _jumps -= 1;           
        }
       
    }


    //Checks if Jump button is let go
    private void JumpCancel()
    {
        _jumpMultiplyerRate = 0.5f;
    }

    private void Fire()
    {
        _isFiring = true;
        if(_reloadTime>= _WS.ReloadTime)
        {
            _canFire = true;
        }
        if (_canFire == true && _isAuto == false)
        {
            _hangTime = HangTimer;
            _firingSingle = true;
            arrow.CreateDebris();
            _canFire = false;
            _reloadTime = 0;
        }
        else if(_isFiring)
        {
            _canFire = true;
        }


    }

    //Checks if Fire button is let go
    private void FireCancel()
    {
        _canFire = false;
        _isFiring = false;
    }

    void Reload()
    {
        if(_reloadTime<_WS.ReloadTime)
        {
            _reloadTime += Time.deltaTime;
        }
    }
    


    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.layer == 8)
        {
            _isGrounded = true;
            _jumpMultiplyer = 1f;
        }
        if (collider2D.gameObject.layer == 10)
        {
            IsVisable = true;
            colour = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
            sprite.color = colour;
        }
    }

    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.layer == 10)
        {
            IsVisable = true;
            colour = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
            sprite.color = colour;
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.layer == 8)
        {
            _isGrounded = false;
            if(_isJumping==false)
            {
                _jumps -= 1;
            }
            
        }
        if (collider2D.gameObject.layer == 10)
        {
            IsVisable = false;
            colour = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
            sprite.color = colour;
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
