using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SuperShooter
{

    public class TMPController : MonoBehaviour
    {


        public TextMeshProUGUI body;
        public TextMeshProUGUI outline;

        // ------------------------------------------------- //

        void Update()
        {

        }

        // ------------------------------------------------- //

        public void SetText(string text)
        {
            body.text = text;
            outline.text = text;
        }

        // ------------------------------------------------- //

        public void FadeIn()
        {

        }

        public void FadeOut()
        {
            //body.GetComponent<Renderer>().material.color.a = 0.25;
            
        }

        // ------------------------------------------------- //

        // ------------------------------------------------- //


    }

}