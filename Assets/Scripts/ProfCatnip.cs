using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelnest.BulletML;

public class ProfCatnip : enemy
{
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


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        timer = timerOG;
        theLayer = LayerMask.GetMask("Player");
        shootPoint = transform.GetChild(0).gameObject;
        bml = shootPoint.GetComponent<BulletSourceScript>();
        shootpointX = shootPoint.transform.localPosition.x;
        //sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(sprite.flipX)
        {
            shootPoint.transform.localPosition = new Vector3(shootpointX, shootPoint.transform.localPosition.y, 0);
        }
        else
        {
            shootPoint.transform.localPosition = new Vector3(-shootpointX, shootPoint.transform.localPosition.y, 0);
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
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                print("FIRE!");
                shoot(true);
                timer = timerOG;
            }
            
        }


    }
    public void shoot()
    {
        Vector3 lookDir = player.transform.position - shootPoint.transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        if (timer <= timerOG / 4)
        {
            shooting = true;
        }
        if (timer <= 0)
        {
            GameObject g = Instantiate(arrow, shootPoint.transform.position, Quaternion.Euler(0, 0, angle));
            // GameManager.GM.addSpawnedObject(g);
            shooting = false;
            timer = timerOG;
        }
    }
    public void shoot(bool gay)
    {
        bml.xmlFile = pattern;
        bml.Reset();
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
        dead = true;
        this.enabled = false; //temp
        col.enabled = false; 
        sprite.enabled = false;
        death(); //temp
        Destroy(gameObject); //temp
        throw new System.NotImplementedException();
    }

}
