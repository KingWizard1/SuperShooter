using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperShooter
{
    public class OpenDoors : MonoBehaviour, IInteractable
    {

        public Animator anim;
        public bool up = false;


        public void Drop()
        {
            throw new System.NotImplementedException();
        }

        public string GetDisplayName()
        {
            return $"Open Door";
        }

        public string GetInteractionString()
        {
            return $"Open Door";
        }

        public void Pickup()
        {
            throw new System.NotImplementedException();
        }




        // Start is called before the first frame update
        void Start()
        {
             
            anim = gameObject.GetComponent<Animator>();
            // equipped = false;

        }

        // Update is called once per frame
        void Update()
        {



            anim.SetBool("Equipped", up);


            if (Input.GetKeyDown(KeyCode.E))

            {

                anim.Play("Door2Open");
                anim.Play("Door1Open");






            }


            //  if (GetComponent<FPSController>().equipped == true)
            //  {

            // }




            if (up == true)
            {
                anim.Play("SpawnDoorL");
                anim.Play("SpawnDoorR");
            }





        }



    }
}