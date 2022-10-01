using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whateveryouwant : MonoBehaviour
{

    public AudioSource mySource;
    public AudioClip oneShot;

    // Start is called before the first frame update
    void Start()
    {
        mySource = GetComponent<AudioSource>();



        mySource.PlayOneShot(oneShot);
    }
    public void playSound()
    {
        mySource.clip = oneShot;
        mySource.Play(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
