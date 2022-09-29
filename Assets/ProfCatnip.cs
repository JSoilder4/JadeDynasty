using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfCatnip : enemy
{
    public Vector3 velocity;
    public LayerMask theLayer;// = LayerMask.GetMask("Player");


    public float timer;
    public float timerOG;
    public GameObject arrow;
    public GameObject shootPoint;

    public bool shooting;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        theLayer = LayerMask.GetMask("Player");
        shootPoint = GetComponentInChildren<GameObject>();
        // sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //facePlayer();
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
            print("FIRE!");
            shoot();
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
