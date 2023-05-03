using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelnest.BulletML;

public class badcatSurprise : enemy
{


    //public Animator anim;
   // public BulletSourceScript bml;
   // public TextAsset bmlXML;
    public float timer = 1f;
    //public float speed = 0.0625f;
    public float maxSpeed = 0.1f;///08333333333f;

    public Vector2 velocity;
    public Vector2 lookDir;

    public float movepointTimer;
    public float movepointTimerOG;

    public GameObject explosion;

    public RectTransform myHPCanvas;
    public GameObject shadow;
    public SpriteRenderer shadowSpriteRenderer;
    public AudioClip hitSFX;
    public AudioClip hitSFX2;
    public AudioClip explosionSFX;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        speed = 0.1f;
        speedOG = 0.1f;

        movepointTimerOG = 0.25f;
        movepointTimer = movepointTimerOG;

        anim = GetComponent<Animator>();

        lookDir = (Vector2)player.transform.position - rb.position;
        velocity = lookDir.normalized;

        try{
myHPCanvas = hp.HPCanvas.GetComponent<RectTransform>();
        }
        catch{
print("lmao, lol even.");
        }
        
        shadowSpriteRenderer = shadow.GetComponent<SpriteRenderer>();
        //   bml = GetComponent<BulletSourceScript>();
    }

    // Update is called once per frame
    public override void Update()
    {
        //base.Update();
        timer -= Time.deltaTime;
        if(!dead && !stasisFrozen)
        {
            if (player.transform.position.x >= transform.position.x)
            {
                sprite.flipY = false;
                myHPCanvas.transform.localPosition = new Vector3(myHPCanvas.transform.localPosition.x,0.07f,0);
                myHPCanvas.transform.localRotation = Quaternion.Euler(0,0,0);
                shadow.transform.localPosition = new Vector3(shadow.transform.localPosition.x,0.14f,0 );//shadow.transform.localPosition.y, 0);
                //shadow.transform.localRotation = Quaternion.Euler(0, 0, 0);
                //shadowSpriteRenderer.flipY = false;
            }
            else if (player.transform.position.x <= transform.position.x)
            {
                
                sprite.flipY = true;
                myHPCanvas.transform.localPosition = new Vector3(myHPCanvas.transform.localPosition.x,-0.07f,0);
                myHPCanvas.transform.localRotation = Quaternion.Euler(0,0,180);
                shadow.transform.localPosition = new Vector3(shadow.transform.localPosition.x, 0.35f, 0);
                //shadow.transform.localRotation = Quaternion.Euler(0, 0, 180);
                //shadowSpriteRenderer.flipY = true;
            }
        }
        
        
        
    }
    public void FixedUpdate()
    {
        if (!stasisFrozen && !dead)
        {
            attackPlayer();
        }
    }
    public void playAHitSound()
    {
        int carl = Random.Range(0, 2);
        if(carl == 1){
            mySource.PlayOneShot(hitSFX2);
        }
        else{
            mySource.PlayOneShot(hitSFX);
        }
    }
    public void attackPlayer()
    {
        // if (speedOG < maxSpeed)
        // {

        //     speedOG += Time.fixedDeltaTime/10;
        //     //print(speed);
        //     //speed = 10000f;
        // }
        movepointTimer -= Time.fixedDeltaTime;
        if (movepointTimer <= 0)
        {
            velocity = lookDir.normalized;

            movepointTimer = movepointTimerOG;
        }
        else if ((player.transform.position - transform.position).magnitude > 0.5f)
        {
            //print("magnitudinal!");
            velocity = lookDir.normalized;

            movepointTimer = movepointTimerOG;
        }



        lookDir = (Vector2)player.transform.position - rb.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, angle);

        rb.MovePosition((Vector2)transform.position + velocity * speed);// * Time.fixedDeltaTime);
        //rb.velocity = velocity * speed * Time.fixedDeltaTime;
        //print(rb.velocity);
    }

    public override void die()
    {
        mySource.Stop();
        //myState = state.dead;
        dead = true;
        //anim.SetInteger("state", (int)1);
        StopAllCoroutines();
        anim.SetTrigger("die");
        StartCoroutine(dieKnockback());
        StartCoroutine(deathCoroutine(2f));
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") && timer <= 0)
        {

            //spawn explosion
            mySource.PlayOneShot(explosionSFX);
            Instantiate(explosion, transform.position, Quaternion.identity);


                print("ExplodeIt");
                Destroy(gameObject);
            //if (hp.currentHP > 25)
            //{
            // //bml.xmlFile = bmlXML;
            // //bml.Reset();



            //    print("SpawnIt");
            //    Destroy(gameObject);
            //}
            //else if (hp.currentHP <= 25)
            //{
                
            //}
        }
        if(collision.CompareTag("shot")){
            playAHitSound();
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && timer <= 0)
        {
            if (hp.currentHP > 25)
            {
                //bml.xmlFile = bmlXML;
                //bml.Reset();
                print("SpawnItCol");
                Destroy(gameObject);
            }
            else if (hp.currentHP <= 25)
            {
                //spawn explosion
                print("ExplodeItCol");
                Destroy(gameObject);
            }
        }
    }
}
