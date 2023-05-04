using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{

    public Texture2D cursor;
    public Texture2D[] elemCursors;
    public Vector2 cursorHotspot;

    public static CursorManager cursorMan;

    //public bool titlescreen;

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

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            Cursor.SetCursor(cursor, cursorHotspot, CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(elemCursors[(int)playergun.gunScript.activeGun.element], cursorHotspot, CursorMode.ForceSoftware);
        }

        

        //GUI.skin.settings.cursorColor = playergun.elementalColors[(int)playergun.gunScript.activeGun.element];
    }
}
