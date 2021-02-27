using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public SpriteRenderer WeaponSprite;
    public List<Sprite> WeapSprites;
    public List<GameObject> Weapons;

    private int index;

    private Controls inputs;

    void Awake()
    {
        inputs = new Controls();
        inputs.Player.SwitchLeft.started+= context => SwitchLeft();
        inputs.Player.SwichRight.started += context => SwitchRight();
    }

        // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponSprite.sprite = WeapSprites[index];
    }

    void SwitchLeft()
    {
        Weapons[index].SetActive(false);
        index -= 1;
        if(index<0)
        {
            index = Weapons.Count - 1;
        }
        Weapons[index].SetActive(true);
    }

    void SwitchRight()
    {
        Weapons[index].SetActive(false);
        index += 1;
        if (index > Weapons.Count - 1)
        {
            index = 0;
        }
        Weapons[index].SetActive(true);
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
