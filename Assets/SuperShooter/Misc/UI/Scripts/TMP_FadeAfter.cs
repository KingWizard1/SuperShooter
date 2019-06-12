using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SuperShooter
{

    public class TMP_FadeAfter : MonoBehaviour
    {

        public float fadeOutAfter = 5;
        public float fadeOutTime = 5;

        private float timer = 0;
        private bool fadingOut = false;

        private TextMeshProUGUI tmp;

        // Use this for initialization
        void Start()
        {

            tmp = GetComponent<TextMeshProUGUI>();

            timer = fadeOutAfter;

        }

        // Update is called once per frame
        void Update()
        {


            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                if (!fadingOut)
                {
                    fadingOut = true;
                    timer = fadeOutTime;
                }
                else
                {
                    tmp.CrossFadeColor(tmp.color.ChangeAlpha(0), fadeOutTime, false, true);

                    // We're done.
                    enabled = false;
                }

            }


        }
    }

}