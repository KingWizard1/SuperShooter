using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperShooter
{
    public class Fade : MonoBehaviour
    {

 
        //  private float transparency = 0f;
        // private bool transparencyGoingUp = true;
        // private bool currentlyFlashing = false;
        private bool canFade;
        private Color alphaColor;
        private float timeToFade = 1.0f;



        public void Start()
        {
            alphaColor = this.GetComponent<MeshRenderer>().material.color;
            alphaColor.a = 0;
        }

        /* 
         IEnumerator flashTransparency(float waitTime)
         {
             //update the alpha (transparency)
             Color tempColor = this.GetComponent<MeshRenderer>().material.color;
             tempColor.a = transparency;
             this.GetComponent<Renderer>().material.color = tempColor;

             //adjust what the alpha will be updated to next time this coroutine is run
             if (transparencyGoingUp)
             {
                 transparency += 0.1f;
                 if (transparency > 0.95f && transparency < 1.2f && currentlyFlashing) //if(transparency == 1)
                     transparencyGoingUp = false;
             }
             else //if transparency is going down
             {
                 transparency -= 0.1f;
                 if (transparency < 0.2f && transparency > 0.08f) //if(transparency == 0.1f)
                     transparencyGoingUp = true;
             }

             //wait before calling this again so it isnt called every frame and therefore flash way too fast
             yield return new WaitForSeconds(waitTime / 9); // 9 because it updates 9 times per direction, so waitTime == total time to go top to bot or bot to top
         }
         */
        // Update is called once per frame
        
        void Update()
        {


            //flashTransparency(9);



            //  this.GetComponent<Renderer>().material.color = new Color(0f, 0f, 0f, 0f);






            Color tempcolor = this.GetComponent<Renderer>().material.color;
            tempcolor.a = Mathf.MoveTowards(0, 1, Time.deltaTime);
            this.GetComponent<Renderer>().material.color = tempcolor;


            //     this.GetComponent<Renderer>().material.color = Color.Lerp(this.GetComponent<Renderer>().material.color, alphaColor, timeToFade * Time.deltaTime);


            StartCoroutine(FadeImage(true));

        }

      
        
            // fades the image out when you click
           
        

        IEnumerator FadeImage(bool fadeAway)
        {
            // fade from opaque to transparent
            if (fadeAway)
            {

                // loop over 1 second
                for (float i = 0; i <= 1; i += Time.deltaTime)
                {
                    // set color with i as alpha
                    this.GetComponent<Renderer>().material.color = new Color(1, 1, 1, i);
                    yield return null;
                }
            }
        }

    }
    
}





