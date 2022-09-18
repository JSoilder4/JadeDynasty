using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shot : MonoBehaviour
{
    //public static shot shotScript;

    public Rigidbody2D rb;
    public Transform firePoint;
    public GameObject player;
    public Vector3 lookDirection;
    public Vector3 overrideDirection;
    public SpriteRenderer sprite;
    public AudioClip hitSound;
    public AudioSource mySource;

    [Header("Shot Modifiers")]
    public bool shotgun, sniper, smg;
    public Color fire = new Color(255, 0, 0);
    public Color water = new Color(0, 130, 255);
    public Color earth = new Color(0, 255, 0);
    public Color air = new Color(255, 255, 0);

    public float shotspeed;

    public bool bSciShot, bSplit, bExplode, bComet;
    public gunEnumScript.effect effect;
    [Header("SciShot")]
    public Vector3 startPos;
    public float speedOffset;
    public float shotSpeedTimer;
    public float timer;
    public float timerOG;
    public Vector3 vectorToPlayer;
    public float angle;
    public float OGshotspeed;
    public bool bReturn;

    [Header("Split")]
    public bool copy;

    [Header("Comet")]
    public Vector3 vectorToMouse;
    public Vector3 mousePos;

    [Header("Explode")]
    public GameObject explosion;

    public gunEnumScript.element element;

    public float bigBulletTimer;

    public int damage;


    /*
     shotSpeedTimer -= speedOffset;
        if (shotSpeedTimer <= 0)
        {
            UIManager.UIM.science(true);
            playSound();
            spriteR.sprite = reverse;
            speed -= speedOffset;
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if ((transform.position.x < startPos.x && transform.position.y < startPos.y) || (transform.position.x > startPos.x && transform.position.y < startPos.y))
            {
                UIManager.UIM.science(false);
                Destroy(gameObject);
            }
        }
         */

    // Start is called before the first frame update
    void Start()
    {
        damage = playergun.gunScript.theGun.damage;
        mySource = GetComponent<AudioSource>();
        element = playergun.gunScript.theGun.element;
        rb = GetComponent<Rigidbody2D>();
        lookDirection = playerDirScript.playerDirectionScript.lookDir;
        sprite = GetComponent<SpriteRenderer>();
        shotspeed = playergun.gunScript.theGun.shotSpeed;
        player = playergun.gunScript.GetComponentInParent<playerMove>().gameObject;

        switch (playergun.gunScript.theGun.element)
        {
            case gunEnumScript.element.Nothing:
                //sprite.color = clense;
                break;
            case gunEnumScript.element.Fire:
                sprite.color = fire;
                break;
            case gunEnumScript.element.Water:
                sprite.color = water;
                break;
            case gunEnumScript.element.Earth:
                sprite.color = earth;
                break;
            case gunEnumScript.element.Air:
                sprite.color = air;
                break;
        }
        //if (gun.gunScript.element == 0)
        //{

        //}
        //else if (gun.gunScript.element == 1)
        //    sprite.color = fire;
        //else if(gun.gunScript.element == 2)
        //    sprite.color = water;
        //else if(gun.gunScript.elementIndex == 3)
        //    sprite.color = earth;
        //else if(gun.gunScript.elementIndex == 4)
        //    sprite.color = air;

        //shotScript = this;
        timer = timerOG;
        shotSpeedTimer = 0.0f;
        speedOffset = shotspeed / 64;
        startPos = transform.position;
        OGshotspeed = shotspeed;

        bigBulletTimer = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!shotgun) 
        //{
        //    rb.MovePosition(transform.position + Vector3.Normalize(lookDirection) * shotspeed);
        //}
        //else
        //{
        //    rb.MovePosition(transform.position + Vector3.Normalize(overrideDirection) * shotspeed);
        //}
        shotEffect();
        rb.MovePosition(transform.position + transform.right *shotspeed);
    }

    public void shotEffect()
    {
        switch(effect)
        {
            case gunEnumScript.effect.Nothing:
                {
                    break;
                }
            case gunEnumScript.effect.Boomerang: //sciShot
                {
                    sciShot();
                    break;
                }
            /*case 2: //split
                {
                    split();
                    break;
                }*/
            case gunEnumScript.effect.EXPLOSION: //explode
                {
                    explode();
                    break;
                }
            case gunEnumScript.effect.Comet: //comet
                {
                    comet();
                    break;
                }
            case gunEnumScript.effect.BigBullets:
                {
                    bigBullets();
                    break;
                }
        }
    }
    
    public void sciShot()
    {
        bSciShot = true;
        shotSpeedTimer -= speedOffset;
        if (shotSpeedTimer <= 0 && !bReturn)
        {
           //shotspeed -= speedOffset;
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (shotspeed <= 0 || bReturn)//transform.position.Equals(startPos))//(transform.position.x < startPos.x && transform.position.y < startPos.y) || (transform.position.x > startPos.x && transform.position.y < startPos.y))
            {
                bReturn = true;
                vectorToPlayer = player.transform.position - transform.position;
                angle = Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg;
                rb.rotation = angle;
                if (shotspeed != OGshotspeed)
                    shotspeed += speedOffset;
                else if (shotspeed > OGshotspeed)
                    shotspeed = OGshotspeed;
            }
        }
    }

    public void split()
    {
        if(!copy)
        bSplit = true;
    }
    public void explode()
    {
        bExplode = true;
    }
    public void comet()
    {
        bComet = true;
        mousePos = playerMove.pms.cam.ScreenToWorldPoint(Input.mousePosition);
        vectorToMouse = mousePos - transform.position;
        angle = Mathf.Atan2(vectorToMouse.y, vectorToMouse.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }
    public void bigBullets() //turn into binding of isaac coal? change color instead of size?
    {
        bigBulletTimer -= Time.deltaTime;
        transform.localScale = new Vector3(transform.localScale.x+shotspeed*1.5f,transform.localScale.y+shotspeed*1.5f,0);
        if(bigBulletTimer <= 0)
        {
            damage += 1;
            bigBulletTimer = 0.25f;
        }
        //print(damage);
    }
    public void killTheShot()
    {
        sprite.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject, 2f);
    }
    public void playHitSound()
    {
        mySource.clip = hitSound;
        mySource.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Destructable"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Destructable") || collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("Dummy"))
        {
           if (!bSplit && !bExplode && !bSciShot)
           {
                playHitSound();
                killTheShot();
           }
            else if (bSciShot)
            {
                shotspeed = 0;
                if (bReturn)
                {
                    playHitSound();
                    killTheShot();
                }
            }
           else if (bSplit)
            {
                if(copy)
                {
                    playHitSound();
                    killTheShot();
                }
                playHitSound();
                killTheShot();
            }
           else if (bExplode)
            {
                GameObject g = Instantiate(explosion, transform.position, Quaternion.identity);
                g.GetComponent<exploding>().shotElement = element;
                //g.GetComponent<exploding>().damage = gun.gunScript.damage;
                killTheShot();
            }
        }
        
    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Destructable") || collision.transform.CompareTag("Enemy"))
        {
            print("OnCollisionEnter2D");
            if (bSplit)
        {
            if (copy)
            {
                Destroy(gameObject);
            }
            Collider2D collider = collision.collider;

            foreach (ContactPoint2D contact in collision.contacts)
            {
                //coming from top or bottom?]
                if (Mathf.Abs(contact.normal.y) > Mathf.Abs(contact.normal.x))
                {

                    if (contact.normal.y >= 0)
                    { //am i hitting top or bottom?

                        GameObject g1 = Instantiate(gameObject, new Vector2(transform.position.x, transform.position.y - 0.1f), transform.rotation);
                        g1.GetComponent<shot>().copy = true;
                        g1.transform.Rotate(collision.transform.forward, 90f);
                        GameObject g2 = Instantiate(gameObject, new Vector2(transform.position.x, transform.position.y - 0.1f), transform.rotation);
                        g2.GetComponent<shot>().copy = true;
                        g2.transform.Rotate(collision.transform.forward, 270f);



                    }
                    if (contact.normal.y < 0)
                    {

                    }

                }
                else
                {
                    if (contact.normal.x >= 0)
                    {

                    }
                    if (contact.normal.x < 0)
                    {

                    }
                }
            }




            Destroy(gameObject);
        }
    }
}*/


}
