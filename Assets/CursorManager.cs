using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    public Texture2D cursor;
    public Texture2D[] elemCursors;
    public Vector2 cursorHotspot;

    public static CursorManager cursorMan;

    private void Awake() {
        
        if (cursorMan == null)
        {
            DontDestroyOnLoad(this); //this means it will exist if you switch scenes.
            cursorMan = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cursorHotspot = new Vector2(cursor.width/2, cursor.height/2);
        Cursor.SetCursor(elemCursors[(int)playergun.gunScript.activeGun.element], cursorHotspot, CursorMode.ForceSoftware);

        //GUI.skin.settings.cursorColor = playergun.elementalColors[(int)playergun.gunScript.activeGun.element];
    }
}
