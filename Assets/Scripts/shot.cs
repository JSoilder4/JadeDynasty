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

    public float shotspeed;

    public bool bSciShot, bBounce, bExplode, bComet;
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
    public GameObject explosionGO;

    public gunEnumScript.element element;

    public float bigBulletTimer;

    public float damage;

    public float invulnTimer;
    public bool invuln;

    public List<Collider2D> colliderBlacklist;


    public GameObject objectThatSpawnedMe;
    public bool effectUsed;
    public bool dead;
    public CircleCollider2D CC2D;

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
        damage = playergun.gunScript.activeGun.damage;
        mySource = GetComponent<AudioSource>();
        element = playergun.gunScript.activeGun.element;
        rb = GetComponent<Rigidbody2D>();
        lookDirection = playerDirScript.playerDirectionScript.lookDir;
        sprite = GetComponent<SpriteRenderer>();
        shotspeed = playergun.gunScript.activeGun.shotSpeed;
        player = playergun.gunScript.GetComponentInParent<playerMove>().gameObject;
        sprite.color = playergun.elementalColors[(int)element];
        
        timer = timerOG;
        shotSpeedTimer = 0.0f;
        speedOffset = shotspeed / 64;
        startPos = transform.position;
        OGshotspeed = shotspeed;

        bigBulletTimer = 0.25f;

        invulnTimer = 0.5f;

        CC2D = GetComponent<CircleCollider2D>();
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
    private void FixedUpdate() {
        if(invuln && !dead){
            GetComponent<Collider2D>().enabled = false; 
        }
        if(!invuln && !dead){
            GetComponent<Collider2D>().enabled = true; 
        }
        while(invuln){
            invulnTimer -= Time.deltaTime;
            if(invulnTimer <= 0){
                invuln = false;
            }
        }
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
            case gunEnumScript.effect.Bounce: //split
                {
                    Bounce();
                    break;
                }
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
            case gunEnumScript.effect.AIBullets:
            {
                AIBullets();
                break;
            }
        }
    }
    public void AIBullets(){
        CC2D.enabled = true;
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

    public void Bounce()
    {
        bBounce = true;

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
        dead = true;
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject, 0.25f);
    }
    public void playHitSound()
    {
        mySource.clip = hitSound;
        mySource.Play();
    }

    // public void figureOutSide(Collider2D collision){
    //     // RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.forward);
    //     // ContactPoint2D[] CP2D = new ContactPoint2D[4];
    //     // int i = hit.collider.GetContacts(CP2D);
    //     // Vector3 theHitPos = CP2D[0].normal;
    //         //Vector3 hit = col.contacts[0].normal;

    //         Vector2 closestPoint = collision.ClosestPoint(transform.position);

    //         Debug.Log(closestPoint);

    //         float angle = Vector3.Angle(closestPoint, Vector3.up);

    //         if (Mathf.Approximately(angle, 0))
    //         {
    //             //Down
    //             Debug.Log("Down");
    //         }
    //         if(Mathf.Approximately(angle, 180))
    //         {
    //             //Up
    //             Debug.Log("Up");
    //         }
    //         if(Mathf.Approximately(angle, 90)){
    //             // Sides
    //             Vector3 cross = Vector3.Cross(Vector3.forward, closestPoint);
    //             if (cross.y > 0)
    //             { // left side of the player
    //                 Debug.Log("Left");
    //             }
    //             else
    //             { // right side of the player
    //                 Debug.Log("Right");
    //             }
    //         }
    //         Debug.Break();
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Destructable"))
        {
            Destroy(collision.gameObject);
        }
        // if(collision.transform.CompareTag("Enemy"))
        // {
        //     killTheShot();
        // }
        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Destructable") || collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("Dummy"))
        {
                if (!invuln &&!bBounce && !bExplode && !bSciShot)
                {
                        //Debug.Break();
                        if(!collision.CompareTag("Enemy")){
                            playHitSound();
                        }
                        killTheShot();
                }
                else if (bSciShot)
                {
                    shotspeed = 0;
                    if (bReturn)
                    {
                        if(!collision.CompareTag("Enemy")){
                            playHitSound();
                        }
                        killTheShot();
                    }
                }
                else if (bBounce)
                    {
                        //figureOutSide(collision);
                        if(!effectUsed){
                            GameObject first = Instantiate(gameObject, transform.position, Quaternion.identity);
                            //first.GetComponent<shot>().colliderBlacklist.Add(collision);
                            first.GetComponent<AudioSource>().enabled = false;
                            first.GetComponent<shot>().bBounce = false;
                            first.GetComponent<shot>().invuln = true;
                            first.GetComponent<shot>().objectThatSpawnedMe = gameObject;
                            first.GetComponent<shot>().effect = gunEnumScript.effect.Nothing;
                            Vector3 newDir = Vector3.Reflect(transform.right, collision.bounds.ClosestPoint(transform.position));
                            //print(newDir);
                            float angle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg;
                            first.transform.rotation = Quaternion.Euler(0,0,angle);
                            //print();
                            //first.transform.rotation = Quaternion.Euler(0,0, transform.rotation.eulerAngles.z + 180);
                            effectUsed = true;
                            if(!collision.CompareTag("Enemy")){
                                playHitSound();
                            }
                            
                            killTheShot();
                            //Debug.Break();
                            
                        }

                    }
                else if (bExplode)
                    {
                        GameObject g = Instantiate(explosionGO, transform.position, Quaternion.identity);
                        g.GetComponent<exploding>().element = element;
                        //g.GetComponent<exploding>().damage = gun.gunScript.damage;
                        killTheShot();
                    }


                
            
            
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Destructable") || collision.transform.CompareTag("Enemy"))
        {
            print("THISISBEINGCALLED");
            if(bBounce){
                
                //figureOutSide(collision.contacts[0].normal);
                // GameObject first = Instantiate(gameObject, transform.position, Quaternion.identity);
                // first.GetComponent<shot>().effect = gunEnumScript.effect.Nothing;
                // first.GetComponent<shot>().bBounce = false;
                // first.transform.Rotate(new Vector3(0,0,90));

                // //Debug.Break();
                // playHitSound();
                // killTheShot();
            }




            // print("OnCollisionEnter2D");
            // if (bSplit)
            // {
            //     if (copy)
            //     {
            //         Destroy(gameObject);
            //     }
            //     Collider2D collider = collision.collider;

            //     foreach (ContactPoint2D contact in collision.contacts)
            //     {
            //         //coming from top or bottom?]
            //         if (Mathf.Abs(contact.normal.y) > Mathf.Abs(contact.normal.x))
            //         {

            //             if (contact.normal.y >= 0)
            //             { //am i hitting top or bottom?

            //                 GameObject g1 = Instantiate(gameObject, new Vector2(transform.position.x, transform.position.y - 0.1f), transform.rotation);
            //                 g1.GetComponent<shot>().copy = true;
            //                 g1.transform.Rotate(collision.transform.forward, 90f);
            //                 GameObject g2 = Instantiate(gameObject, new Vector2(transform.position.x, transform.position.y - 0.1f), transform.rotation);
            //                 g2.GetComponent<shot>().copy = true;
            //                 g2.transform.Rotate(collision.transform.forward, 270f);



            //             }
            //             if (contact.normal.y < 0)
            //             {

            //             }

            //         }
            //         else
            //         {
            //             if (contact.normal.x >= 0)
            //             {

            //             }
            //             if (contact.normal.x < 0)
            //             {

            //             }
            //         }
            //     }




            //     Destroy(gameObject);
            // }
    }
}


}
