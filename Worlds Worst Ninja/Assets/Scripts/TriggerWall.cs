using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWall : MonoBehaviour
{

    private PlayerMovement _pm;
    public bool IsLeftWall;
    // Start is called before the first frame update

    private void Start()
    {
        _pm = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==9)
        {
            if (IsLeftWall)
            {
                _pm.IsLeftWalled = true;
            }
            else
            {
                _pm.IsRightWalled = true;
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer==9)
        {
            if (IsLeftWall)
            {
                _pm.IsLeftWalled = false;
            }
            else
            {
                _pm.IsRightWalled = false;
            }
        }
        
    }
}
