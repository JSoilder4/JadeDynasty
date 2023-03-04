using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genericEnemyShot : MonoBehaviour
{

    public bool destroyOnWall;
    public float timeToWait;
    public Collider2D col2D;
    // Start is called before the first frame update
    void Start()
    {
        col2D = GetComponent<Collider2D>();
        col2D.enabled = false;
        StartCoroutine(collideActivateWaiter());
    }

    // Update is called once per frame
    void Update()
    {
        print(col2D.enabled);
    }

    public IEnumerator collideActivateWaiter()
    {
        yield return new WaitForSeconds(timeToWait);
        col2D.enabled = true;
        //print(col2D.enabled);
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") && destroyOnWall)
        {
            Destroy(gameObject);
        }
    }
}
