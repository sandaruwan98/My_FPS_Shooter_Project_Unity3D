using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float force = 200f;
    public float FireRate = 2f;

    public Camera fpscam;
    public Animator animator;
    public Animator animstab;
    public ParticleSystem mf;
    public ParticleSystem flame;
    public ParticleSystem pt;
    public ParticleSystem dst;
    public ParticleSystem smoke;

    public GameObject Bloodimpact;
    public GameObject Dirtimpact;
    public GameObject waterimpact;
    public GameObject woodimpact;
    public GameObject concreteimpact;
    public GameObject glassimpact;
    
    public Animation anim;
    public TextMeshProUGUI textmesh;
    public List<AudioSource> sounds;
   
    
    private int bullets = 10;
    private int totalBullts = 150;
    private float nextTimetofire = 0f;
    private int x;
    private bool run = false;

    // Start is called before the first frame update
    void Start()
    {
        bullets = 10;
        totalBullts = 150;
        x = 0;
    }



    // Update is called once per frame
    void Update()
    {
        StabEnemyCheck();
        if (x > 0)
        {
            x--;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimetofire && x<=0 && (!run)) //shoot-----------
        {
            bullets--;
            nextTimetofire = Time.time + 1f/FireRate;
            if(bullets > 0 && totalBullts>0)
            {
                anim.CrossFade("Fire");
                sounds[0].Play();
                MfPlay();
                shoot();
            }
            else
            {
                x = 150;
                totalBullts -= (10-bullets);
                if(totalBullts > 10)
                {
                    bullets = 10;
                }
                else
                {
                    bullets = totalBullts;
                }
                
                anim.CrossFade("Reload");
                sounds[1].Play();
            }
            
        }
        
        Reload();
        runAnimation();
        textmesh.SetText(bullets + "   " + totalBullts);

        

    }// end of update ------------------------------------------------------

   
    void shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit, range))
        {
            
            Target target = hit.transform.GetComponent<Target>();
            if (hit.transform.tag == "head") // CHECK FOR HEAD SHOT
                {
                Transform swat = hit.transform.parent.GetChild(1);
                target = swat.GetComponent<Target>();
                target.TakeDamage(30f);
            }else if (target != null)
            {
                target.TakeDamage(15f);               
            }
            
            impactPLAY("ENEMY",Bloodimpact);
            impactPLAY("head", Bloodimpact);
            impactPLAY("terrain",Dirtimpact);
            impactPLAY("water", waterimpact);
            impactPLAY("wood", woodimpact);
            impactPLAY("glass", glassimpact);
            impactPLAY("concrete", concreteimpact);
            void impactPLAY(string tag, GameObject obj)
            {
                if (hit.transform.tag == tag)
                {
                    GameObject imapact = Instantiate(obj, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(imapact,5f);


                }
            }

            
            //if (hit.rigidbody != null)
            //{
            //    hit.rigidbody.AddForce(-hit.normal * force);
            //}
        }

    } // end of shoot --------------------------------------------------------------

    
    void runAnimation()
    {
        

        if (Input.GetButtonUp("run"))
        {
            animator.SetBool("run", false);
            run = false;
        }

        if (Input.GetButtonDown("run"))
        {
            animator.SetBool("run", true);
            run = true;
        }

       
    }

    void StabEnemyCheck()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
           
            animator.SetBool("stab", true);
        }
        else
        {
           // animator.SetBool("stab", false);
        }
    }
    
    void MfPlay()
    {
        
         mf.Play();
         flame.Play();
         pt.Play();
         dst.Play();
         smoke.Play();
    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            x = 150;
            totalBullts -= (10 - bullets);
            if (totalBullts > 10)
            {
                bullets = 10;
            }
            else
            {
                bullets = totalBullts;
            }
            anim.CrossFade("Reload");
            sounds[1].Play();

        }
       
    }


}//end of class ----------------------------------------------------------
