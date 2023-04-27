using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class minimapObject : MonoBehaviour
{

    //public SpriteRenderer sprite;
    public Image image;
    public int x;
    public int y;
    // Start is called before the first frame update
    void Start()
    {
        //sprite = GetComponent<SpriteRenderer>();
        //image = GetComponent<Image>();
    }
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Check()
    {
        //print("get checked.");
        switch (GameManager.GM.playerRoomGrid[x, y])
        {
            case "null":
                image.enabled = false;
                break;
            case "true":
                image.enabled = true;
                image.color = new Color(0, 1, 0);
                break;
            case "false":
                image.enabled = true;
                image.color = new Color(1, 0, 0);
                break;
        }
    }
}
