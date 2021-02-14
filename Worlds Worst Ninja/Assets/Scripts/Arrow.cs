using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private Controls inputs;

    public Transform LookTarget;

    public float angle;

    public Vector2 dir;

    

    private void Awake()
    {
        inputs = new Controls();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = inputs.Player.Look.ReadValue<Vector2>();

        Debug.Log(mousePosition);
        var pos = Camera.main.WorldToScreenPoint(transform.position);
        dir =  mousePosition- pos;
        angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg)-90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
