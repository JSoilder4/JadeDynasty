using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Knockback : MonoBehaviour
{
    public float delay = 0.15f;

    public UnityEvent onBegin, onDone;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void knockback(Vector3 direction, float strength)
    {
        StopAllCoroutines();
        onBegin?.Invoke();
        direction.Normalize();
        rb.AddForce(direction * strength, ForceMode2D.Impulse);
        StartCoroutine(reset());
    }
    public IEnumerator reset()
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
        onDone?.Invoke();
    }


}