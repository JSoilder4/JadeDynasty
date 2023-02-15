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
        slam,
        reload,

        dead
    }
    [SerializeField]
    private state myState = state.idle;




    public Vector3 velocity;
    public float velX;
    public float velY;
    public int walkCounter;
    public int walkCounterGoal;
    public Vector3 playerDirection;
    public LayerMask theLayer;// = LayerMask.GetMask("Player");


    public float timer;
    public float timerOG;
    //public float walkTimer;
    //public float walkTimerOG;
    public float attackTimer;
    public float attackTimerOG;
    public bool firstAttackShot;
    public bool attacking;
    public float slamDur;


    public GameObject arrow;
    public GameObject shootPointL;
    public GameObject shootPointR;
    
    public GameObject shootPointSlamL;
    public GameObject shootPointSlamR;

   // public GameObject walkpointObjectPlsIgnore;
    //public float shootpointX;

    public bool shooting;

    //public TextAsset pattern;

    public BulletSourceScript L, R, SlamL, SlamR;

    //public Animator anim;




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

        attackTimer = attackTimerOG;

        
        //walkpointObjectPlsIgnore = transform.GetChild(4).gameObject;

        //walkTimerOG = walkTimer;
        //shootpointX = shootPoint.transform.localPosition.x;
        //sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!attacking && myState != state.dead)
        {
            base.Update();
        }
        if(!stasisFrozen)
        {
            if(myState != state.dead)
            {
                attackPlayer();
            }
        }
        else
        {
            
        }
        // if(sprite.flipX)
        // {
        //     shootPoint.transform.localPosition = new Vector3(shootpointX, shootPoint.transform.localPosition.y, 0);
        // }
        // else
        // {
        //     shootPoint.transform.localPosition = new Vector3(-shootpointX, shootPoint.transform.localPosition.y, 0);
        // }
        
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
            //if(!earthed)

            
            //checkHealth();

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
        
        //print(velocity);
        //print(attackTimer);
        if(myState == state.walk)
        {
            //walkTimer -= Time.deltaTime;
            Vector3 posMoveTo = (transform.position + velocity * speed);
            //print(posMoveTo.magnitude);
            if(posMoveTo.magnitude >= 0.75f)
            {
                print("Yes: "+name);
//                walkpointObjectPlsIgnore.transform.position = transform.position + velocity;// * speed;
                //rb.MovePosition(transform.position + velocity * speed);

            }
            else
            {
                print("No: "+name);
                //setIdle();
            }
            rb.MovePosition(transform.position + velocity * speed);
            
        }
        // Debug.DrawLine(new Vector3(transform.position.x - 5f, transform.position.y+50f, 1), new Vector3(transform.position.x - 5f, transform.position.y-50f, 1), Color.red);
        // Debug.DrawLine(new Vector3(transform.position.x + 5f, transform.position.y+50f, 1), new Vector3(transform.position.x + 5f, transform.position.y-50f, 1), Color.red);

        // Debug.DrawLine(new Vector3(playerDirection.x - 0.5f, playerDirection.y+50f, 1), new Vector3(playerDirection.x - 0.5f, playerDirection.y-50f, 1), Color.blue);
        // Debug.DrawLine(new Vector3(playerDirection.x + 0.5f, playerDirection.y+50f, 1), new Vector3(playerDirection.x + 0.5f, playerDirection.y-50f, 1), Color.blue);
        // Debug.DrawLine(new Vector3(playerDirection.x - 1f, playerDirection.y+50f, 1), new Vector3(playerDirection.x - 1f, playerDirection.y-50f, 1), Color.blue);
        // Debug.DrawLine(new Vector3(playerDirection.x + 1f, playerDirection.y+50f, 1), new Vector3(playerDirection.x + 1f, playerDirection.y-50f, 1), Color.blue);

        // Debug.DrawLine(new Vector3(playerDirection.x + 50f, playerDirection.y + 0.25f, 1), new Vector3(playerDirection.x - 50f, playerDirection.y + 0.25f, 1), Color.blue);
        // Debug.DrawLine(new Vector3(playerDirection.x + 50f, playerDirection.y + 0.5f, 1), new Vector3(playerDirection.x - 50f, playerDirection.y + 0.5f, 1), Color.blue);
        if ((player.transform.position.x >= transform.position.x - 5f && player.transform.position.x <= transform.position.x + 5f) && player.transform.position.y <= transform.position.y+0.35f && myState != state.slam)
        {
            print("IM IN RANGE");
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0 && myState == state.idle)
            {
                setShoot();
                attackTimer = attackTimerOG;
            }
        }
        else
        {
            attackTimer = attackTimerOG;
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
        anim.SetInteger("state", (int)myState);
    }
    public void setWalk() //anim
    {
        //walkTimer = walkTimerOG;
        if(myState != state.dead)
        {
        myState = state.walk;
        randomizeWalkpoint();
        anim.SetInteger("state", (int)myState);
        }
    }
    public void setShoot(){
        myState = state.shoot;
        anim.SetInteger("state", (int)myState);
    }
    public void setReload(){
        myState = state.reload;
        anim.SetInteger("state", (int)myState);
    }

    public void walkFinished() //anim
    {
        if(myState != state.dead){
        walkCounter++;
        randomizeWalkpoint();
        if ((walkCounter >= walkCounterGoal))
        {
            //walkTimer = walkTimerOG;
            setIdle();
            walkCounter = 0;
            walkCounterGoal = Random.Range(1, 4);
        }
        }
        
    }
    public void randomizeWalkpoint()
    {
        if (sprite.flipX)
        {
            velX = Random.Range(playerDirection.x+0.5f, playerDirection.x + 1f);
        }
        else
        {
            velX = Random.Range(playerDirection.x - 1f, playerDirection.x-0.5f);
        }
        //velX = Random.Range(playerDirection.x - 2.5f, playerDirection.x + 2.5f);
        velY = Random.Range(playerDirection.y + 0.25f, playerDirection.y + 0.5f);
    }

    public void shootL()
    {
        firstAttackShot = true;
        L.Reset();
    }
    public void shootR()
    {
        firstAttackShot = true;
        R.Reset();
    }

    public void shoot() //anim
    {
        attacking = true;

        if (firstAttackShot)
        {
            if (sprite.flipX)
            {
                shootR();
            }
            else if (!sprite.flipX)
            {
                shootL();
            }
        }
        else
        {
            if (sprite.flipX)
            {
                shootL();
            }
            else if (!sprite.flipX)
            {
                shootR();
            }
        }



    }
    public void slamShot() //anim
    {
        myState = state.slam;
        anim.SetInteger("state", (int)myState);

        StartCoroutine(theSlam());
        


    }
    public IEnumerator theSlam()
    {
        //yield return new WaitForSeconds(slamDur / 3);
        //rb.MovePosition(transform.position + Vector3.up * speed * 5);
        //yield return new WaitForSeconds(slamDur / 2);

        rb.angularDrag = 0.05f;
        rb.drag = 0f;

        SlamL.Reset();
        SlamR.Reset();
        rb.velocity = Vector3.up * 4;
        yield return new WaitForSeconds(slamDur/3);
        rb.velocity = Vector3.up * 2;
        yield return new WaitForSeconds(slamDur/3);
        rb.velocity = Vector3.up * 1;
        yield return new WaitForSeconds(slamDur/3);


        //rb.MovePosition(transform.position + Vector3.up * speed * 5);
        
        setReload();
        rb.angularDrag = 200f;
        rb.drag = 200f;
        yield return null;
    }

    public void reload() //anim
    {
        

        firstAttackShot = false;
        attacking = false;
        setIdle();
        rb.velocity = Vector3.zero;
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



    public override void die()
    {
        //Debug.Break();
        myState = state.dead;
        dead = true;
        anim.SetInteger("state", (int)myState);
        StopAllCoroutines();
        anim.SetTrigger("die");
        StartCoroutine(dieKnockback());
        StartCoroutine(deathCoroutine(2f));
        
    }
    
    
}
