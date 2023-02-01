using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelnest.BulletML;

public class SnakeEnemy : enemy
{
private enum state
    {
       // idle,
        walk,
        shoot,

        dead
    }
    [SerializeField]
    private state myState = state.walk;
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
    public bool attacking;
    public GameObject fangs;

   // public GameObject walkpointObjectPlsIgnore;
    //public float shootpointX;

    public bool shooting;

    //public TextAsset pattern;

    public BulletSourceScript fangScript;

    public Animator anim;


    //Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        timer = timerOG;
        theLayer = LayerMask.GetMask("Player");
        fangs = transform.GetChild(0).gameObject;
        fangScript = fangs.GetComponent<BulletSourceScript>();
        anim = GetComponent<Animator>();

        attackTimer = attackTimerOG;
        setWalk();
        //walkpointObjectPlsIgnore = transform.GetChild(4).gameObject;

        //walkTimerOG = walkTimer;
        //shootpointX = shootPoint.transform.localPosition.x;
        //sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!attacking && !dead)
        {
            base.Update();
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
            // case state.idle:
            // break;

            case state.walk:
            break;

            case state.shoot:
            break;
        }
        if(!dead)
        {
            attackPlayer();
        }
            

    }
    public void attackPlayer()
    {
        timer -= Time.deltaTime;
        playerDirection = player.transform.position - transform.position;
        playerDirection = Vector3.Normalize(playerDirection);

        velocity = new Vector3(velX,velY,0);
        
        //print(velocity);
        //print(attackTimer);
        if(myState == state.walk)
        {
            rb.MovePosition(transform.position + velocity * speed);
            
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
    // public void setIdle(){
    //     myState = state.idle;
    //     anim.SetInteger("state", (int)myState);
    // }
    public void setWalk() //anim
    {
        //walkTimer = walkTimerOG;
        attacking = false;
        myState = state.walk;
        randomizeWalkpoint();
        anim.SetInteger("state", (int)myState);
    }
    public void setShoot(){
        myState = state.shoot;
        anim.SetInteger("state", (int)myState);
    }

    public void walkFinished() //anim
    {
        walkCounter++;
        randomizeWalkpoint();
        if (!dead && walkCounter >= walkCounterGoal)
        {
            //walkTimer = walkTimerOG;
            setShoot();
            //walkCounter = 0;
            walkCounterGoal = Random.Range(1, 4);
        }
    }
    public void randomizeWalkpoint()
    {
        // if (sprite.flipX)
        // {
        //     velX = Random.Range(playerDirection.x+0.5f, playerDirection.x + 1f);
        // }
        // else
        // {
        //     velX = Random.Range(playerDirection.x - 1f, playerDirection.x-0.5f);
        // }
        velX = Random.Range(playerDirection.x - 1f, playerDirection.x + 1f);
        velY = Random.Range(playerDirection.y - 1f, playerDirection.y + 1f);
    }

    public void shoot() //anim
    {
        attacking = true;
        walkCounter--;
        fangScript.Reset();
        if(walkCounter <= 0)
        {
            setWalk();
        }

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
        myState = state.dead;
        dead = true;
        anim.SetInteger("state", (int)myState);
        StopAllCoroutines();
        anim.SetTrigger("die");
        StartCoroutine(dieKnockback());
        StartCoroutine(deathCoroutine(2f));
    }
}
