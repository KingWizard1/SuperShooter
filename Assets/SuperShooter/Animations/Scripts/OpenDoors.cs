using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperShooter
{
    public class OpenDoors : MonoBehaviour, IInteractable
{

        public Animator anim;

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

        }

        // Update is called once per frame
        void Update()
        {


            if (Input.GetKeyDown(KeyCode.E))

            {

                anim.Play("Door2Open");
                anim.Play("Door1Open");






            }
            if (GetComponent<Weapon>().hasWeapon == true)
            {
                anim.Play("SpawnDoorL");
                anim.Play("SpawnDoorR");
            }
        }
    }
}