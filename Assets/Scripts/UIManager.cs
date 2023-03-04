using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager UIM;

    private void Awake()
    {
        if (UIM == null)
        {
            DontDestroyOnLoad(this); //this means it will exist if you switch scenes.
            UIM = this;
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
        
    }
}
