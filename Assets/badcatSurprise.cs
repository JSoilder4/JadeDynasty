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
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        speed = 0.0001f;
        speedOG = 0.0001f;

        movepointTimerOG = 0.25f;
        movepointTimer = movepointTimerOG;

        anim = GetComponent<Animator>();

        lookDir = (Vector2)player.transform.position - rb.position;
        velocity = lookDir.normalized;


        //   bml = GetComponent<BulletSourceScript>();
    }

    // Update is called once per frame
    public override void Update()
    {
        //base.Update();
        timer -= Time.deltaTime;
        
        
        
    }
    public void FixedUpdate()
    {
        if (!stasisFrozen && !dead)
        {
            attackPlayer();
        }
    }
    public void attackPlayer()
    {
        if (speedOG < maxSpeed)
        {

            speedOG += Time.fixedDeltaTime/10;
            print(speed);
            //speed = 10000f;
        }
        movepointTimer -= Time.fixedDeltaTime;
        if (movepointTimer <= 0)
        {
            velocity = lookDir.normalized;

            movepointTimer = movepointTimerOG;
        }
        else if ((player.transform.position - transform.position).magnitude > 0.5f)
        {
            print("magnitudinal!");
            velocity = lookDir.normalized;

            movepointTimer = movepointTimerOG;
        }



        lookDir = (Vector2)player.transform.position - rb.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

       transform.eulerAngles = new Vector3(0, 0, angle);

        rb.MovePosition((Vector2)transform.position + velocity * speedOG);// * Time.fixedDeltaTime);
        //rb.velocity = velocity * speed * Time.fixedDeltaTime;
        //print(rb.velocity);
    }

    public override void die()
    {
        //myState = state.dead;
        dead = true;
        //anim.SetInteger("state", (int)myState);
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
