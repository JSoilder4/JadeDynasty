using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    Vector3 velocity;
    Rigidbody2D rb;

    public int[] coords = new int[2];

    public Camera cam;

    public float speed;

    public static playerMove pms; 
    

    public bool dodging;
    public float dodgeSpeed;
    public float dodgeSpeedOG;
    public float dodgeSpeedDropMulti = 10f;
    public float minDodgeSpeed = 50f;

    public float dodgeTime;
    public float dodgeTimeOG; //0.7, 0.5 dodge, 0.2 roll

    public hp hp;

    public bool invuln;
    public SpriteRenderer sprite;
    public float invulnTime = 0.5f;
    public bool invulnFrameRunning;

    private void Awake()
    {
        hp = GetComponent<hp>();
    }

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        coords = new int[2];
        pms = this;
        rb = GetComponent<Rigidbody2D>();
        
        //gunSprite = GetComponentInChildren<SpriteRenderer>();
        //gunRotatoRB = GetComponentInChildren<Rigidbody2D>();
        dodgeSpeed = dodgeSpeedOG;
      //  dodgeCDTimer = dodgeCDTimerOG;
        dodgeTime = dodgeTimeOG;

        coords[0] = 0;
        coords[1] = 0;
    }

    // Update is called once per frame
    void Update()
    {

            

        if (Input.GetKeyDown(KeyCode.Z))// dev key remove later
        {
            GameManager.GM.rerollGuns();
        }
        if (!GameManager.GM.playerdead)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !dodging)
            {
                //print("fucking kill me");
                dodging = true;
               
            }
        }



        
        
        
    }
    public void checkHealth()
    {
        if (hp.currentHP <= 0)
        {
            die();
        }
    }
    private void FixedUpdate()
    {
        checkHealth();
        if (!GameManager.GM.playerdead && !dodging)
        {
            velocity.x = Input.GetAxisRaw("Horizontal");// * speed;
            velocity.y = Input.GetAxisRaw("Vertical");// * speed;

            velocity = Vector3.Normalize(velocity);
            //print(velocity);
            //velocity *= speed;

        }
        if (!dodging)
        {
            rb.velocity = velocity * speed * Time.fixedDeltaTime;
        }
        else if (dodging)
        {
            dodgeChecking();
        }
        if (!invuln)
        {
            StopCoroutine(flash());
        }



        //rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
    }
    public void dodgeChecking()
    {
        dodgeTime -= Time.fixedDeltaTime;

        if (dodgeTime > 0.2)
        {
            rb.velocity = velocity * dodgeSpeed * Time.fixedDeltaTime;
            invuln = true;
        }
        else if (dodgeTime <= 0.2)
        {
            rb.velocity = velocity * (dodgeSpeed / 2) * Time.fixedDeltaTime;
            if (!invulnFrameRunning)
            {
                invuln = false;
            }
            
        }

            //dodgeSpeed -= dodgeSpeed * dodgeSpeedDropMulti * Time.fixedDeltaTime;
            //rb.velocity = velocity * dodgeSpeed;
        if (dodgeTime <= 0)
         {
            dodging = false;
            dodgeSpeed = dodgeSpeedOG;
            //dodgeCDTimer = dodgeCDTimerOG;
            dodgeTime = dodgeTimeOG;
         }
        
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
            gun rGunG = rGun.theGun;
            if (playergun.gunScript.equippedGuns.Count >= 4)
            {
                GameManager.GM.swapGunAndOtherGun(playergun.gunScript.activeGun, rGunG, rGun);
            }
            else
            {
                GameManager.GM.gunPickup(collision.gameObject, rGunG);

            }

           
            
            
            
            
            //GameManager.GM.swapGunAndRGun(playergun.gunScript, rGun);

        }
        if (collision.transform.CompareTag("Enemy") || collision.CompareTag("arrow"))
        {
            //collision.GetComponent<enemy>().hp = 1000;
            //die();
            if (!invuln)
            {
                hp.takeDamage(1);
                StartCoroutine(invulnFrame());
            }
            
            if (collision.CompareTag("arrow") && !invuln)
            {
                Destroy(collision.gameObject);
            }
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
    
    public IEnumerator invulnFrame()
    {
        // StopAllCoroutines();
        invulnFrameRunning = true;
       // print("haha");
        StartCoroutine(flash());
        invuln = true;
        yield return new WaitForSeconds(invulnTime);
       // print("hoho");
        StopCoroutine(flash());
        invuln = false;
        invulnFrameRunning = false;
        yield return null;
    }
    public IEnumerator flash()
    {

        for (int i = 0; i < 6; i++)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
            yield return new WaitForSeconds(invulnTime/12);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 255);
            yield return new WaitForSeconds(invulnTime/12);
        }



        yield return null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy") && !invuln)
        {
            //collision.transform.GetComponent<enemy>().hp = 1000;
            //die();
            hp.takeDamage(1);
            StartCoroutine(invulnFrame());
            //print("yeah?:");
            //collision.gameObject.GetComponent<Knockback>().knockback(collision.transform.position - transform.position, 1000);
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - transform.position) * 5000, ForceMode2D.Impulse);

        }
        if (collision.transform.CompareTag("Dummy"))
        {
            collision.transform.GetComponent<enemy>().hp.currentHP = 1000;
            
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy") && !invuln)
        {
            hp.takeDamage(1);
            StartCoroutine(invulnFrame());

        }
    }
    public void die()
    {
        GameManager.GM.dead();
        //sprite.enabled = false;
        playergun.gunScript.sprite.enabled = false;
        playergun.gunScript.enabled = false;
        this.enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
    }
    public void repsawn()
    {

    }
}
