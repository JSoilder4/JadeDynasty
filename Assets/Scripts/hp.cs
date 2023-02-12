using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hp : MonoBehaviour
{
    public float maxHP;
    public float currentHP;

    public SpriteRenderer sprite;
    
    public AudioClip fireClip;

    public AudioSource mySource;

    public bool player;

    public enemy myEnemyScript;

    public Vector3 knockbackDir;

    public bool dead;

    public GameObject HPCanvas;
    public Image HPBar;
    public Image elemHPBar;

    enum element
    {
        Nothing,
        Fire,
        Water,
        Lightning
    }
    element myElement;

    public float elemHPMax;
    public float elemHPCurrent;

    int ha = 0;
    public void elementEffect()
    {

    }
    public void colorNormalize()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        sprite = GetComponent<SpriteRenderer>();
        mySource = GetComponent<AudioSource>();
        if(!player)
        {
            myEnemyScript = GetComponent<enemy>();
            HPCanvas = transform.Find("HPBarCanvas").gameObject;
            HPBar = HPCanvas.transform.Find("HPBar").GetComponent<Image>();
            elemHPBar = HPCanvas.transform.Find("elemHPBar").GetComponent<Image>();
            myElement = element.Nothing;
            if (myEnemyScript.elite)
            {
                myElement = (element)Random.Range(1, System.Enum.GetNames(typeof(element)).Length);
                elemHPMax = maxHP / 3;
                elemHPCurrent = elemHPMax;
            }
        }
        switch (myElement)
        {
            case element.Fire:
                elemHPBar.color = Color.red;
                break;
            case element.Water:
                elemHPBar.color = Color.blue;
                break;
            case element.Lightning:
                elemHPBar.color = Color.yellow;
                break;

            default:
                Destroy(elemHPBar);
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elementEffect();
        if(dead)
        {
            
            while(ha == 0)
            { 
                sprite.color = new Color(255,255,255,255);
                ha++;
            }
            
        }
        checkHealth();

    }
    public void takeDamage(float dmg) //player damage
    {
        currentHP -= dmg;
    }
    public void takeDamage(gunEnumScript.element elem, float dmg, Vector3 shotDir) //enemy damage
    {
        if(myEnemyScript.elite)
        {
            if(elemHPCurrent > 0)
            {
                switch(elem)
                {
                    case gunEnumScript.element.Nothing:
                        elemHPCurrent -= dmg*0.75f;
                    break;
                    case gunEnumScript.element.Fire:
                        if(myElement == element.Fire)
                        {
                            elemHPCurrent -= dmg*2f;
                        }
                        else
                        {
                            elemHPCurrent -= dmg*0.5f;
                        }
                    break;
                    case gunEnumScript.element.Water:
                        if(myElement == element.Water)
                        {
                            elemHPCurrent -= dmg*2f;
                        }
                        else
                        {
                            elemHPCurrent -= dmg*0.5f;
                        }
                    break;
                    case gunEnumScript.element.Lightning:
                        if(myElement == element.Lightning)
                        {
                            elemHPCurrent -= dmg*2f;
                        }
                        else
                        {
                            elemHPCurrent -= dmg*0.5f;
                        }
                    break;
                    case gunEnumScript.element.Stasis:
                    
                    break;
                }
            }
            else
            {
                currentHP -= dmg;
            }
            
        }
        else
        {
            currentHP -= dmg;
        }



        
        if(currentHP <= 0)
        {
            knockbackDir = shotDir.normalized;//new Vector3(shotDir.x, shotDir.y, 0);
            //print(knockbackDir);
        }
    }
    public void healDamage(float heal)
    {
        if ((currentHP + heal) > maxHP)
        {
            currentHP = maxHP;
        }
        else
        {
            currentHP += heal;
        }
        
    }
    public void checkHealth()
    {
        HPBar.fillAmount = currentHP / maxHP;
        // if (onfire)
        // {

        //     sprite.color = colorChange;
        //     currentHP -= 1;
        //     fireTimer -= Time.deltaTime;
        //     if (fireTimer <= 0)
        //     {
        //         onfire = false;
        //     }
        // }
        if (!dead &&currentHP <= 0 && !player)
        {
            print("die" + currentHP);
            // GameManager.GM.updateScore(GameManager.GM.maxScore / GameManager.GM.enemiesToReset.Count);
            // GameManager.GM.enemiesToReset.Remove(gameObject);
            Destroy(HPBar);
            myEnemyScript.die();
            dead = true;
            this.enabled = false;
            //Destroy(gameObject);
        }
    }
    public void fireSound()
    {
        mySource.clip = fireClip;
        mySource.Play();
    }
    public IEnumerator ColorFade(float colorDuration, Color desiredColor)//(float aTime, float dur)
    {
        float t = 0;    
        while (t < colorDuration)
        {
            t += Time.deltaTime;
            sprite.color = Color.Lerp(desiredColor, new Color(1,1,1), t / colorDuration);
            yield return null;
        }
        yield return null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<shot>() != null)
        {
            if(!player){
                StopAllCoroutines();
                StartCoroutine(ColorFade(0.75f, collision.GetComponent<shot>().sprite.color));
            }
            
            // switch (collision.GetComponent<shot>().element)
            // {
            //     case gunEnumScript.element.Nothing:
            //         {
            //             break;
            //         }
            //     case gunEnumScript.element.Fire:
            //         {
                        
            //             break;
            //         }
            //     case gunEnumScript.element.Water:
            //         {

            //             break;
            //         }
            //     case gunEnumScript.element.Lightning:
            //         {

            //             break;
            //         }
            //     case gunEnumScript.element.Stasis:
            //         {

            //             break;
            //         }
            // }
            
        }
        if (!player && collision.transform.CompareTag("shot"))
        {
            switch (collision.GetComponent<shot>().effect)
            {
                case gunEnumScript.effect.EXPLOSION:


                    break;

                default:
                    takeDamage(collision.GetComponent<shot>().element, collision.GetComponent<shot>().damage, transform.position - collision.transform.position);
                    break;
            }
            

            
            
            //Destroy(collision.gameObject);
        }
        
    }
}
