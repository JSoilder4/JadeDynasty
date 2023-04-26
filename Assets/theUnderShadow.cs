using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theUnderShadow : MonoBehaviour
{
    public SpriteRenderer parentSprite;
    public SpriteRenderer mySprite;
    // Start is called before the first frame update
    void Start()
    {
        parentSprite = GetComponentInParent<SpriteRenderer>();
        mySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mySprite.flipX = parentSprite.flipX;
        mySprite.flipY = parentSprite.flipY;
    }
}
