using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugBoolChecker : MonoBehaviour
{
    public int posX, posY;
    public bool theBool;
    public SpriteRenderer sprite;
    public Color red = new Color(255, 0, 0);
    public Color green = new Color(0, 255, 0);

    private GenerationManager genManage;
    // Start is called before the first frame update
    void Start()
    {
        genManage = GameObject.FindWithTag("GameController").GetComponent<GenerationManager>();

        sprite = GetComponent<SpriteRenderer>();

        


    }

    // Update is called once per frame
    void Update()
    {
        theBool = genManage.roomGrid[posX, posY];
        if (theBool)
        {
            sprite.color = green;
        }
        else
        {
            sprite.color = red;
        }
    }
}
