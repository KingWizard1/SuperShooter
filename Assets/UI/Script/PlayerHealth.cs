using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SuperShooter
{
    public class PlayerHealth : MonoBehaviour, IKillable
    {
        public int health;
        public int startHealth = 100;
        public Slider healthSlider;                                 // Reference to the UI's health bar.
        public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
        public AudioClip deathClip;                                 // The audio clip to play when the player dies.
        public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.
        public AudioSource playerAudio;
        public Text timeText;

        public GameObject gameOver;
        public Animator anim;                                       // Reference to the Animator component

        //Reference
        public FPSController controller;
        public FPSCameraLook cameraLook { get; private set; }

        bool isDead;                                                // Whether the player is dead.
        bool damaged;                                               // True when the player gets damaged.
        bool isInvincible;

        public float lerpSpeed;

        void Awake()
        {
            // Setting up the references.
            anim = GetComponent<Animator>();
            playerAudio = GetComponent<AudioSource>();
            controller = GetComponent<FPSController>();
            // Set the initial health of the player.
            health = startHealth;
            healthSlider.value = health;

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                TakeDamage(10);
            }

            if (health >= 10)
            {

                    healthSlider.value = health;


                
            }


            // If the player has just been damaged...
            if (damaged)
            {
                // ... set the colour of the damageImage to the flash colour.
                damageImage.color = flashColour;
            }
            // Otherwise...
            else
            {
                // ... transition the colour back to clear.
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }

            // Reset the damaged flag.
            damaged = false;
            // Do nothing if dead.

            // Do nothing if dead.
            if (isDead)
            {

#if DEBUG
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
                    Respawn();
#endif

                return;
            }
        }



        public void TakeDamage(int amount)
        {
            if (isInvincible)
                return;
            // Set the damaged flag so the screen will flash.
            damaged = true;

            // Reduce the current health by the damage amount.
            health -= amount;




            // If the player has lost all it's health and the death flag hasn't been set yet...
            if (health <= 0 && !isDead)
            {
                // ... it should die.
                Kill();
                playerAudio.Play();
            }
        }


        public void Kill()
        {
            // Set the death flag so this function won't be called again.
            isDead = true;
            health = 0;
            UIManager.Main.ShowDeathScreen(true);
            gameOver.SetActive(true);
            anim.SetBool("GameOver", true);

            // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).



            // Turn off the movement and shooting scripts.
            controller.enabled = false;


        }
        public void Respawn()
        {
            isDead = false;
            health = 100;
            controller.enabled = true;
            UIManager.Main.ShowDeathScreen(false);
        }
    }
}