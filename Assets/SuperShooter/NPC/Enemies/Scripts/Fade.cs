using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperShooter
{
    public class Fade : MonoBehaviour
    {

        public Color alphaColor;





        public void Start()
        {
            alphaColor = this.GetComponent<Material>().color;
            alphaColor.a = 0;
        }


        // Update is called once per frame

        void Update()
        {

                StartCoroutine("FadeImage");

            

        }




        IEnumerator FadeImage()
        {
            // fade from opaque to transparent




            // loop over 1 second
            for (float i = 0; i <= 1; i +=  Time.deltaTime / 2)
            {

                    // set color with i as alpha
                    this.GetComponent<Renderer>().material.color = new Color(1, 1, 1, i);

                    yield return null;
                
            }


        }
    }

}








