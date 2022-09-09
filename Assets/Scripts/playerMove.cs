using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    Vector3 velocity;
    Rigidbody2D rb;

    public int[] coords = new int[2];

    public Camera cam;
    public SpriteRenderer sprite;

    public float speed;

    public Vector2 mousePos;

    public static playerMove pms; 
    public Vector3 lookDir;

    public SpriteRenderer gunSprite;

    public Transform gunRotato;

    // Start is called before the first frame update
    void Start()
    {
        coords = new int[2];
        pms = this;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        //gunSprite = GetComponentInChildren<SpriteRenderer>();
        //gunRotatoRB = GetComponentInChildren<Rigidbody2D>();

        coords[0] = 0;
        coords[1] = 0;
    }

    // Update is called once per frame
    void Update()
    {


            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x > transform.localPosition.x)
            {
                sprite.flipX = false; //replace later; moving to animation system
                gunSprite.flipY = false;
            }
            else
            {
                sprite.flipX = true; //replace later
                gunSprite.flipY = true;
            }

        if (Input.GetKeyDown(KeyCode.Z))// dev key remove later
        {
            GameManager.GM.rerollGuns();
        }



            lookDir = mousePos - rb.position;

            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        gunRotato.eulerAngles = new Vector3(0,0,angle);
        
        
    }
    private void FixedUpdate()
    {
        if (!GameManager.GM.playerdead)
        {
            velocity.x = Input.GetAxisRaw("Horizontal");// * speed;
            velocity.y = Input.GetAxisRaw("Vertical");// * speed;

            velocity = Vector3.Normalize(velocity);
            velocity *= speed;
        }
            rb.velocity = velocity * Time.fixedDeltaTime;
            //rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
    }
    private void LateUpdate()
    {
        //rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
        //rb.velocity = velocity * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Gun"))
        {
            randomGun rGun = collision.GetComponent<randomGun>();

            GameManager.GM.swapGunAndRGun(gun.gunScript, rGun);

        }
        if (collision.transform.CompareTag("Enemy") || collision.CompareTag("arrow"))
        {
            die();
        }
        if (collision.transform.CompareTag("shot"))
        {
            if(collision.name != "Explosion")
            {
                if (collision.GetComponent<shot>().bReturn)
                {
                    Destroy(collision.gameObject);
                }
            }
            
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            die();
        }
    }
    public void die()
    {
        GameManager.GM.dead();
        sprite.enabled = false;
        gun.gunScript.sprite.enabled = false;
        gun.gunScript.enabled = false;
        this.enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
    }
    public void repsawn()
    {

    }
}
