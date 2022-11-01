using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHit : MonoBehaviour
{

    public hp hp;
    public playerMove playerMove;

    // Start is called before the first frame update
    void Start()
    {
        hp = GetComponentInParent<hp>();
        playerMove = GetComponentInParent<playerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy") || collision.CompareTag("arrow"))
        {
            //collision.GetComponent<enemy>().hp = 1000;
            //die();
            if (!playerMove.invuln)
            {
                hp.takeDamage(1);
                StartCoroutine(playerMove.invulnFrame());
            }
            
            if (collision.CompareTag("arrow") && !playerMove.invuln)
            {
                Destroy(collision.gameObject);
            }
        }
        if (collision.transform.CompareTag("shot"))
        {
            if(collision.name != "Explosion")
            {
                if (collision.GetComponent<shot>().bReturn)
                {
                    Destroy(collision.gameObject);
                }
            }
            
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy") && !playerMove.invuln)
        {
            //collision.transform.GetComponent<enemy>().hp = 1000;
            //die();
            hp.takeDamage(1);
            StartCoroutine(playerMove.invulnFrame());
            //print("yeah?:");
            //collision.gameObject.GetComponent<Knockback>().knockback(collision.transform.position - transform.position, 1000);
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - transform.position) * 5000, ForceMode2D.Impulse);

        }
        if (collision.transform.CompareTag("Dummy"))
        {
            collision.transform.GetComponent<enemy>().hp.currentHP = 1000;
            
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy") && !playerMove.invuln)
        {
            hp.takeDamage(1);
            StartCoroutine(playerMove.invulnFrame());

        }
    }
}
