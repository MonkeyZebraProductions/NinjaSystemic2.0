using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{

    private Controls inputs;

    [SerializeField] public float MovementSpeed = 5f;
    [SerializeField] public float JumpSpeed = 5f;
    [SerializeField] public int MaxJumps = 1;

    public GameObject SelectedWeapon;

    public Transform LootTarget;

    public Camera camera;

    private Vector2 move,look;

    private bool _isGrounded,_isJumping;

    private float _jumpMultiplyer = 1;

    private float _jumpMultiplyerRate;

    private int _jumps;

    private Rigidbody2D _rb2D;

    void Awake()
    {
        inputs = new Controls();
        
        inputs.Player.Jump.started += context => Jump();
        inputs.Player.Jump.canceled += context => JumpCancel();
       
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _jumps = MaxJumps;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(look);

        if(_isGrounded)
        {
            _jumps = MaxJumps;
        }
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

        SelectedWeapon.transform.LookAt(LootTarget.transform, Vector2.up);
        _rb2D.velocity = new Vector2(move.x * MovementSpeed, 0);

        if (_isJumping == true)
        {

            _rb2D.AddForce(new Vector2(0, JumpSpeed*_jumpMultiplyer));
            _jumpMultiplyer *= _jumpMultiplyerRate;
            if (_jumpMultiplyer <= 0.1f)
            {
                _isJumping = false;
            }
        }
    }

    private void Jump()
    {
        if(_jumps>0)
        {
            _isJumping = true;
            _jumpMultiplyer = 1f;
            _jumpMultiplyerRate = 0.9f;

            
                _jumps -= 1;
            
        }
       
    }

    private void JumpCancel()
    {
        _jumpMultiplyerRate = 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.layer == 8)
        {
            _isGrounded = true;
            _jumpMultiplyer = 1f;
        }
        //if (collider2D.gameObject.tag == "RightWall")
        //{
        //    _isRightWalled = true;
        //}
        //if (collider2D.gameObject.tag == "Left Wall")
        //{
        //    _isLeftWalled = true;
        //}
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.layer == 8)
        {
            _isGrounded = false;
            _jumps-=1;
        }
        //if (collider2D.gameObject.tag == "RightWall")
        //{
        //    _isRightWalled = true;
        //}
        //if (collider2D.gameObject.tag == "Left Wall")
        //{
        //    _isLeftWalled = true;
        //}
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
