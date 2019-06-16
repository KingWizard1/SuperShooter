using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperShooter
{
    public class OpenDoors : MonoBehaviour, IInteractable
    {

        public Animator anim;
        public bool up = false;
        public bool e;
        public bool isDoor;
        public GameObject doorEnemy1;


        public void Drop()
        {
        //    throw new System.NotImplementedException();
        }

        public string GetDisplayName()
        {
            return $"Open Door";
        }

        public string GetInteractionString()
        {
            return $"Open Door";
            
        }

        public void Pickup(ICharacterController owner)
        {
          //  throw new System.NotImplementedException();
        }




        // Start is called before the first frame update
        void Start()
        {
             
            anim = gameObject.GetComponent<Animator>();
            // equipped = false;

        }

        void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Player" )
            {

                isDoor = true;

               
            }
        }
        // Update is called once per frame
        void Update()
        {

           var player = GameManager.Main?.PlayerObject;
           var weapon = player.GetComponentInChildren<Weapon>();
            

                anim.SetBool("Equipped", up);



            if (Input.GetKeyDown(KeyCode.E) && isDoor == true)
            {

                e = true;
                anim.SetBool("Open", true);
                doorEnemy1.SetActive(true);
                Debug.Log("OPENUP");

            }






            if (weapon != null)
            {
                anim.Play("SpawnDoorL");
                anim.Play("SpawnDoorR");
            }





        }



    }
}