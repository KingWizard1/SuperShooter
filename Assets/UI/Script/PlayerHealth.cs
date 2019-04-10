using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SuperShooter
{
    public class PlayerHealth : MonoBehaviour
    {
        public Slider healthSlider;                                 // Reference to the UI's health bar.
        public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
        public AudioClip deathClip;                                 // The audio clip to play when the player dies.
        public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
        public Color flashColour = new Color(200f, 0f, 0f, 200f);     // The colour the damageImage is set to, to flash.
        public AudioSource playerAudio;
        public Text timeText;

        public GameObject gameOver;
        public Animator anim;                                       // Reference to the Animator component

        //Reference
        //public CharacterController charCon;
        //public FPSController controller;
        //public FPSCameraLook camLook { get; private set; }


        int currentHealth;

        //bool isDead;                                                // Whether the player is dead.
        //bool damaged;                                               // True when the player gets damaged.
        //bool isInvincible;

        public float lerpSpeed;

        void Awake()
        {
            // Setting up the references.
            anim = GetComponent<Animator>();
            playerAudio = GetComponent<AudioSource>();
            //controller = GetComponent<FPSController>();
            // Set the initial health of the player.
            //health = startHealth;

        }


        public void SetHealth(int health, int maxHealth, bool isDead)
        {

            currentHealth = health;

            if (isDead)
            {

                playerAudio.Play();


                gameOver.SetActive(true);
                anim.SetBool("GameOver", true);


            }

        }

        private bool damaged = false;

        public void ShowDamage()
        {

            damaged = true;

            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;
            
        }

        private void Update()
        {
            //if (healthSlider.value != currentHealth)
            //healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, Time.deltaTime * lerpSpeed);
            healthSlider.value = currentHealth;

            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);

            if (damageImage.color == Color.clear)
                damaged = false;

        }



        //public void Kill()
        //{
        //    // Set the death flag so this function won't be called again.
        //    isDead = true;
        //    health = 0;
        //    healthSlider.value = health;

        //    gameOver.SetActive(true);
        //    anim.SetBool("GameOver", true);

        //    // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        //    //Time.timeScale = 0f;

        //    //Cursor.lockState = CursorLockMode.None;
        //    //Cursor.visible = true;
        //    //FPSController.controller.enabled = false;
        //    // Turn off the movement and shooting scripts.


        //}

    }
}