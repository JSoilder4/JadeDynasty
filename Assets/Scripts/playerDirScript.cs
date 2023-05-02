using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDirScript : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Vector3 lookDir;

    public SpriteRenderer gunSprite;

    public Transform gunRotato;

    public Vector2 mousePos;

    public Camera cam;

    Rigidbody2D rb;

    public static playerDirScript playerDirectionScript;
    public enum direction
    {
        up,
        right,
        down,
        left
    }
    public direction dir;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerDirectionScript = this;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > transform.localPosition.x && !GameManager.GM.playerdead)
        {
            sprite.flipX = false; //replace later; moving to animation system
            gunSprite.flipY = false;
        }
        else if(!GameManager.GM.playerdead)
        {
            sprite.flipX = true; //replace later
            gunSprite.flipY = true;
        }

        lookDir = mousePos - rb.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        gunRotato.eulerAngles = new Vector3(0, 0, angle);


       // print(angle);

        
        if (angle > -45 && angle < 45) // right 45 - 0 / -0 - -45
        {
            dir = direction.right;
        }
        else if (angle < 135 && angle > 45) // up 45 - 135
        {
            dir = direction.up;
        }
        else if (angle > -135 && angle < -45) // down -45 - -135
        {
            dir = direction.down;
        }
        else if ((angle > 135 || angle < -135)) // left 135 - 180 / -180 - -135
        {
            dir = direction.left;
        }



    }
    private void FixedUpdate()
    {
        
        switch (dir)
        {
            case direction.up:
                //print(dir);
                break;
            case direction.right:
                //print(dir);
                break;
            case direction.down:
                //print(dir);
                break;
            case direction.left:
                //print(dir);
                break;
        }
    }
}
