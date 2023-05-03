using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stasisChains : MonoBehaviour
{

    public hp hpScriptThatSpawnedMe;
    public enemy enemyScriptThatSpawnedMe;

    public Animator myAnim;

    public AudioClip unChain;
    public AudioSource mySource;
    public bool alreadyDead;
    // Start is called before the first frame update
    void Start()
    {
        mySource = GetComponent<AudioSource>();

        myAnim = GetComponent<Animator>();

        try{
            transform.position = hpScriptThatSpawnedMe.HPCanvas.transform.position;
        }
        catch{
            print("john the chains are being bratty again");
        }
        

        //transform.localScale = enemyScriptThatSpawnedMe.gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!enemyScriptThatSpawnedMe.stasisFrozen && !alreadyDead)
        {
            myAnim.SetTrigger("stopStasis");
            mySource.PlayOneShot(unChain);
            alreadyDead = true;
        }
    }
    public void killme(){
        Destroy(gameObject);
    }
    


}
