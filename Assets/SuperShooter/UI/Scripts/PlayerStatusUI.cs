using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SuperShooter
{
    public class PlayerStatusUI : MonoBehaviour
    {
        public GameObject ammoIcon;
        public TextMeshProUGUI ammoText;
        public TextMeshProUGUI ammoText2;
        public TextMeshProUGUI weaponText;
        public TextMeshProUGUI healthText;

        // ------------------------------------------------- //

        public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.

        public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
        public Color flashColour = new Color(200f, 0f, 0f, 200f);   // The colour the damageImage is set to, to flash.

        public float lerpSpeed = 5;


        // ------------------------------------------------- //

        public void SetStatus(PlayerCharacter player)
        {
            // Set health
            if (healthText != null)
                healthText.text = player.health.ToString();

            // Set health color.
            // 0 == Red
            // 25% or less == Yellow
            // Anything above 25% == White
            if (player.health == 0 || player.isDead)
                healthText.color = Color.red;
            else if (player.health <= 25)
                healthText.color = Color.yellow;
            else healthText.color = Color.white;
        }

        // ------------------------------------------------- //

        public void ShowDamage()
        {


            // ... set the colour of the damageImage to the flash colour.
            //damageImage.color = flashColour;

        }

        public void SetWeapon(Weapon weapon)
        {

            if (weapon != null)
            {
                
                var weaponName = weapon.GetDisplayName();

                // Set
                weaponText.text = weaponName;
                ammoText.text = $"{weapon.ammoInClip}/{weapon.maxAmmoPerClip}";
                ammoText2.text = $"+{weapon.ammoRemaining}";


                // Colors. Things are set in a specific order, for logic reasons.
                if      (weapon.isReloadRequired || weapon.isOutOfAmmo) ammoText.color = Color.red;
                else if (weapon.isLastClip)                             ammoText.color = Color.yellow;
                else                                                    ammoText.color = Color.white;

                if      (weapon.isLastClip)                             ammoText2.color = Color.red;
                else if (weapon.isLastReload)                           ammoText2.color = Color.yellow;
                else                                                    ammoText2.color = Color.white;

                if      (weapon.isOutOfAmmo)                            weaponText.color = Color.red;
                else if (weapon.isLastClip || weapon.isReloadRequired)  weaponText.color = Color.yellow;
                else                                                    weaponText.color = Color.white;



            }
            else
            {

                // Set
                weaponText.text = "Unarmed";
                ammoText.text = string.Empty;
                ammoText2.text = string.Empty;

                // Colors
                ammoText.color = Color.white;
                ammoText2.color = Color.white;
                weaponText.color = Color.white;

            }



        }

        // ------------------------------------------------- //

        private void Update()
        {
            // Update health slider
            //if (healthSlider.value != healthSliderVal)
            //    healthSlider.value = Mathf.Lerp(healthSlider.value, healthSliderVal, Time.deltaTime * lerpSpeed);

            // Update damage indicators
            // ... transition the colour back to clear.
            //damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);

            //if (damageImage.color == Color.clear)
            //    damaged = false;

        }



    }
}