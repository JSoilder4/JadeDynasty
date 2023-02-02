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

    public bool gunColliding = false;
    public GameObject playerGun;
    public playergun pGunScript;

    public Animator playerAnim;

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
        playerGun = GameObject.Find("playerGun");
        pGunScript = playerGun.GetComponent<playergun>();
        playerAnim = GetComponent<Animator>();
        
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
            if ((velocity.x != 0 || velocity.y != 0) && Input.GetKeyDown(KeyCode.Space) && !dodging)
            {
                if (sprite.flipX)
                {
                    playerAnim.SetTrigger("dodgebackwards");
                }
                else
                {
                    playerAnim.SetTrigger("dodge");
                    
                }

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

            if (velocity.x != 0 || velocity.y != 0)
            {
                playerAnim.SetInteger("walkState", 1);
            }
            else
            {
                playerAnim.SetInteger("walkState", 0);
            }

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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.CompareTag("Gun") && !pGunScript.reloading)
        {
            if(gunColliding){
                return;
            }
            gunColliding = true;
            
            

            if (Input.GetKeyDown(KeyCode.E))
            {
                RandomGun rGun = other.GetComponent<RandomGun>();
                gun rGunG = rGun.theGun;
                if (playergun.gunScript.equippedGuns.Count >= 4)
                {
                    print(GetComponent<Collider2D>());
                    GameManager.GM.swapGunAndOtherGun(playergun.gunScript.activeGun, rGunG, rGun);
                }
                else
                {
                    print("Double trouble? ");
                    GameManager.GM.gunPickup(other.gameObject, rGunG);
                }
                
            }
            StartCoroutine(annoying());
        }
    }
    IEnumerator annoying()
    {
        yield return new WaitForEndOfFrame();
        gunColliding = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
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
    private void OnTriggerExit2D(Collider2D other) {
        // if(other.CompareTag("Gun")){
        //     other.GetComponent<Collider2D>().enabled = true;
        // }
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
    
    public void die()
    {
        GameManager.GM.dead();
        //sprite.enabled = false;
        playergun.gunScript.sprite.enabled = false;
        //playergun.gunScript.enabled = false;
        //this.enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
    }
    public void repsawn()
    {

    }
}
