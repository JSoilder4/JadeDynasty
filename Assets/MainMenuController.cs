using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public GameObject instructions;

    public Image helpButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayButton(){
        SceneManager.LoadScene(1);
    }
    public void ToggleInstructions(){
        instructions.SetActive(!instructions.activeSelf);
        helpButton.fillCenter = !helpButton.fillCenter;
    }
}
