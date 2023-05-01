using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoPickupScript : MonoBehaviour
{
    public Sprite[] ammoSprites;
    public SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(playergun.gunScript.activeGun.gunType){
            case gunEnumScript.gunType.Pistol:
                sprite.sprite = ammoSprites[0];
            break;
            case gunEnumScript.gunType.Shotgun:
                sprite.sprite = ammoSprites[1];
            break;
            case gunEnumScript.gunType.Sniper:
                sprite.sprite = ammoSprites[2];
            break;
            case gunEnumScript.gunType.SMG:
                sprite.sprite = ammoSprites[0];
            break;
        }
    }
}
