using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelnest.BulletML;

public class ProfCatnip : enemy
{

    public enum state
    {
        idle,
        shooting,
        dead
    }
    public state myState = state.shooting;

    //public TextAsset badcatSurpriseXML;

    public Vector3 velocity;
    public LayerMask theLayer;// = LayerMask.GetMask("Player");


    public float timer;
    public float timerOG;
    public GameObject arrow;
    public GameObject shootPoint;
    public float shootpointX;

    public bool shooting;

    public TextAsset pattern;

    public BulletSourceScript bml;
    public GameObject surpriseCatObject;
    //public BulletSourceScript surpriseBml;

    public AudioClip dyingSFX;
    public AudioClip shootSFX;
    public AudioClip hitSFX;
    public AudioClip hitSFX2;

    

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        timer = timerOG;
        theLayer = LayerMask.GetMask("Player");
        shootPoint = transform.GetChild(0).gameObject;
        bml = shootPoint.GetComponent<BulletSourceScript>();
        shootpointX = shootPoint.transform.localPosition.x;
        anim = GetComponent<Animator>();
        //surpriseBml = GetComponent<BulletSourceScript>();
        //sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(!stasisFrozen || myState != state.dead)
        {
            if(sprite.flipX)
            {
                shootPoint.transform.localPosition = new Vector3(shootpointX, shootPoint.transform.localPosition.y, 0);
            }
            else
            {
                shootPoint.transform.localPosition = new Vector3(-shootpointX, shootPoint.transform.localPosition.y, 0);
            }
            if (visable && !dead &&  myState != state.dead)
            {
                //if(!earthed)
                attackPlayer();
                //checkHealth();
            }
        }
        

    }
    public void attackPlayer()
    {
        timer -= Time.deltaTime;
        velocity = player.transform.position - transform.position;
        if (velocity.magnitude > 5.2f)
        {
            rb.MovePosition(transform.position + Vector3.Normalize(velocity) * speed);
           // print("hewwo");
        }
        else if (velocity.magnitude < 4.8f)
        {
            rb.MovePosition(transform.position - Vector3.Normalize(velocity) * speed);
            //print("gwoodbwye");
        }

        RaycastHit2D raycast = Physics2D.Raycast(shootPoint.transform.position, player.transform.position - shootPoint.transform.position, 10f, theLayer);
        Debug.DrawRay(shootPoint.transform.position, player.transform.position - shootPoint.transform.position, Color.red);
        if (raycast && raycast.transform.CompareTag("Player"))
        {

            setShooting();
            // timer -= Time.deltaTime;
            // if(timer <= 0)
            // {
            //     print("FIRE!");
            //     shoot();
            //     timer = timerOG;
            // }
            
        }
        else
        {
            setIdle();
        }


    }

    public void setIdle(){
        myState = state.idle;
        anim.SetInteger("state", (int)myState);
    }
    public void setShooting(){
        myState = state.shooting;
        anim.SetInteger("state", (int)myState);
    }
    // public void shoot()
    // {
    //     Vector3 lookDir = player.transform.position - shootPoint.transform.position;
    //     float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
    //     if (timer <= timerOG / 4)
    //     {
    //         shooting = true;
    //     }
    //     if (timer <= 0)
    //     {
    //         GameObject g = Instantiate(arrow, shootPoint.transform.position, Quaternion.Euler(0, 0, angle));
    //         // GameManager.GM.addSpawnedObject(g);
    //         shooting = false;
    //         timer = timerOG;
    //     }
    // }
    public void shoot() //anim i think
    {

        bml.xmlFile = pattern;
        bml.Reset();
    }
    public void playShootingSound()//anim definitely
    {
        mySource.PlayOneShot(shootSFX);
    }
    public void playAHitSound()
    {
        int carl = Random.Range(0, 2);
        if(carl == 1){
            mySource.PlayOneShot(hitSFX2, 0.15f);
        }
        else{
            mySource.PlayOneShot(hitSFX, 0.15f);
        }
    }





    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if(collision.CompareTag("shot") && !stasisFrozen){
            playAHitSound();
        }
    }
    public override void OnBecameVisible()
    {
        base.OnBecameVisible();
    }
    public override void OnBecameInvisible()
    {
        base.OnBecameInvisible();
    }


    // public override void die()
    // {
    //     dead = true;
    //     this.enabled = false; //temp
    //     col.enabled = false; 
    //     sprite.enabled = false;
    //     death(); //temp
    //     Destroy(gameObject); //temp
    //     throw new System.NotImplementedException();
    // }
    public override void die()
    {

        myState = state.dead;
        dead = true;
        anim.SetInteger("state", (int)myState);
        StopAllCoroutines();
        anim.speed = 1f;
        anim.SetTrigger("die");
        StartCoroutine(dieKnockback());
        //StartCoroutine(deathCoroutine(2f));
    }
    public void playDyingSFX() //anim
    {
        mySource.PlayOneShot(dyingSFX);
    }
    public void spawnCatSurprise() //anim
    {
        
        GameObject g = Instantiate(surpriseCatObject, transform.position, Quaternion.identity);
        GameManager.GM.addSpawnedObject(g);








        //surpriseBml.xmlFile = badcatSurpriseXML;
        //surpriseBml.Reset();

        //Debug.Break();
    }
    public void dieForReal() //anim
    {
        Destroy(gameObject);
    }

}
