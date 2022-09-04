using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archer : enemy
{
    public float timer;
    public float timerOG;

    public bool shooting;

    public GameObject arrow;

    public float distanceRadius;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        timer = timerOG;
        //sprite = GetComponent<SpriteRenderer>();



        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //facePlayer();
        if (visable)
        {
            if (!earthed)
            {
                timer -= Time.deltaTime;
                attackPlayer();
                if (!shooting && distanceFromPlayer() < distanceRadius)
                {
                    move();
                }
                   
            }
            
            //checkHealth();
        }
        
    }
    public void attackPlayer()
    {
        Vector3 lookDir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        if (timer <= timerOG/4)
        {
            shooting = true;
        }
        if (timer <= 0)
        {
            GameObject g = Instantiate(arrow, transform.position, Quaternion.Euler(0,0,angle));
           // GameManager.GM.addSpawnedObject(g);
            shooting = false;
            timer = timerOG;
        }
    }
    public void move()
    {
        Vector3 lookDir = player.transform.position - transform.position;
        rb.MovePosition(transform.position + -Vector3.Normalize(lookDir) * speed);
    }
    public float distanceFromPlayer()
    {
        float dist = 0;

        dist = Vector2.Distance(transform.position, player.transform.position);
        //print(dist);


        return dist;
    }


    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);


    }
    public override void OnBecameVisible()
    {
        base.OnBecameVisible();
    }
    /*public override void OnBecameInvisible()
    {
        base.OnBecameInvisible();
    }*/
}
