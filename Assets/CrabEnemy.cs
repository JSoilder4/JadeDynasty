using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelnest.BulletML;

public class CrabEnemy : enemy
{
    private enum state
    {
        idle,
        walk,
        shoot,
        reload,
    }
    private state myState = state.idle;




    public Vector3 velocity;
    public float velX;
    public float velY;
    public Vector3 playerDirection;
    public LayerMask theLayer;// = LayerMask.GetMask("Player");


    public float timer;
    public float timerOG;
    public float walkTimer;
    public float walkTimerOG;
    public GameObject arrow;
    public GameObject shootPointL;
    public GameObject shootPointR;
    
    public GameObject shootPointSlamL;
    public GameObject shootPointSlamR;
    //public float shootpointX;

    public bool shooting;

    //public TextAsset pattern;

    public BulletSourceScript L, R, SlamL, SlamR;

    public Animator anim;


    //Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        timer = timerOG;
        theLayer = LayerMask.GetMask("Player");
        shootPointL = transform.GetChild(0).gameObject;
        shootPointR = transform.GetChild(1).gameObject;
        shootPointSlamL = transform.GetChild(2).gameObject;
        shootPointSlamR = transform.GetChild(3).gameObject;
        L = shootPointL.GetComponent<BulletSourceScript>();
        R = shootPointR.GetComponent<BulletSourceScript>();
        SlamL = shootPointSlamL.GetComponent<BulletSourceScript>();
        SlamR = shootPointSlamR.GetComponent<BulletSourceScript>();
        anim = GetComponent<Animator>();
        walkTimerOG = walkTimer;
        //shootpointX = shootPoint.transform.localPosition.x;
        //sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        // if(sprite.flipX)
        // {
        //     shootPoint.transform.localPosition = new Vector3(shootpointX, shootPoint.transform.localPosition.y, 0);
        // }
        // else
        // {
        //     shootPoint.transform.localPosition = new Vector3(-shootpointX, shootPoint.transform.localPosition.y, 0);
        // }
        anim.SetInteger("state",(int)myState);
        switch(myState)
        {
            case state.idle:
                //anim.SetTrigger("idle");
            break;

            case state.walk:
                //anim.SetTrigger("walk");
            break;

            case state.shoot:

            break;

            case state.reload:

            break;
        }


        if (visable)
        {
            //if(!earthed)
            attackPlayer();
            //checkHealth();
        }

    }
    public void attackPlayer()
    {
        timer -= Time.deltaTime;
        playerDirection = player.transform.position - transform.position;
        playerDirection = Vector3.Normalize(playerDirection);
        // if (velocity.magnitude > 5.2f)
        // {
        //     rb.MovePosition(transform.position + Vector3.Normalize(velocity) * speed);
        //    // print("hewwo");
        // }
        // else if (velocity.magnitude < 4.8f)
        // {
        //     rb.MovePosition(transform.position - Vector3.Normalize(velocity) * speed);
        //     //print("gwoodbwye");
        // }

        velocity = new Vector3(velX,velY,0);
        print(velocity);
        if(myState == state.walk)
        {
            walkTimer -= Time.deltaTime;
            rb.MovePosition(transform.position + velocity * speed);
        }
        if(walkTimer <= 0)
        {
            walkTimer = walkTimerOG;
            myState = state.idle;
            velX = Random.Range(-1f, 1f);
            velY = Random.Range(playerDirection.y-0.15f, playerDirection.y+0.15f);
        }





        //RaycastHit2D raycast = Physics2D.Raycast(shootPoint.transform.position, player.transform.position - shootPoint.transform.position, 10f, theLayer);
        //Debug.DrawRay(shootPoint.transform.position, player.transform.position - shootPoint.transform.position, Color.red);
        // if (raycast && raycast.transform.CompareTag("Player"))
        // {
        //     timer -= Time.deltaTime;
        //     if(timer <= 0)
        //     {
        //         print("FIRE!");
        //         shoot(true);
        //         timer = timerOG;
        //     }
            
        // }


    }
    // public void shoot(bool gay)
    // {
    //     bml.xmlFile = pattern;
    //     bml.Reset();
    // }



    public void setIdle(){
        myState = state.idle;
    }
    public void setWalk(){
        walkTimer = walkTimerOG;
        myState = state.walk;
    }
    public void setShoot(){
        myState = state.shoot;
    }
    public void setReload(){
        myState = state.reload;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);


    }
    public override void OnBecameVisible()
    {
        base.OnBecameVisible();
    }
    public override void OnBecameInvisible()
    {
        base.OnBecameInvisible();
    }
}
