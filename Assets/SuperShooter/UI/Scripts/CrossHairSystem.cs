using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    public class CrossHairSystem : MonoBehaviour
    {

        public GameObject crossHair;

        public GameObject hitMarker;

        // ------------------------------------------------- //

        private CrossHairHitMarker hit;


        // ------------------------------------------------- //

        private void Awake()
        {
            hit = hitMarker.GetComponent<CrossHairHitMarker>();
        }


        // ------------------------------------------------- //

        public void ShowHitMarker(Color color)
        {

            hit.Show(color);


        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


    }

}