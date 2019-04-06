using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace SuperShooter { 
public class PlayerHealth : MonoBehaviour
{

    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.
    public AudioSource playerAudio;
    public Text timeText;

    public GameObject gameOver;
    public Animator anim;                                              // Reference to the Animator component
    CharacterController playerMovement;                         // Reference to the player's movement.
                             
    bool isDead;                                                // Whether the player is dead.
    bool damaged;                                               // True when the player gets damaged.


    public float lerpSpeed;

    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<CharacterController>();
        // Set the initial health of the player.
        FPSController.health = FPSController.startHealth;
        
        
    }
   
    void Update()
    {
        if (FPSController.health >= 10)
        {
            if (FPSController.health != healthSlider.value)
            {
                // Set the health bar's value to the current health.
                healthSlider.value = Mathf.Lerp(healthSlider.value, FPSController.health, Time.deltaTime * lerpSpeed);
            }
        }
        else
        {
            
            healthSlider.value = FPSController.health;
            

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
        
    }
 


    public void TakeDamage(int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

            // Reduce the current health by the damage amount.
        FPSController.health -= amount;

       
 

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (FPSController.health <= 0 && !isDead)
        {
            // ... it should die.
            Death();
            playerAudio.Play();
        }
    }
  

    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;

      
        gameOver.SetActive(true);
        anim.SetBool("GameOver", true);
        
        

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        
        

        // Turn off the movement and shooting scripts.
        playerMovement.enabled = false;
       
        
    }
}
}