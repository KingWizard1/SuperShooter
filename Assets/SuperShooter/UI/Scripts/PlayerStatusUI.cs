using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SuperShooter
{
    public class PlayerStatusUI : MonoBehaviour
    {
        public GameObject ammoIcon;
        public TextMeshProUGUI ammoText;
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
                var currentAmmoInClip = weapon.ammo;
                var maxAmmoPerClip = weapon.maxAmmoPerClip;
                var totalAmmoLeft = (weapon.maxAmmoPerClip * weapon.clips);

                var weaponName = weapon.GetDisplayName();

                // Set
                weaponText.text = weaponName;
                ammoText.text = string.Format("{0}/{1} + {2}",
                    currentAmmoInClip, maxAmmoPerClip, totalAmmoLeft);

                // Colors
                ammoText.color = weapon.isClipEmpty ? Color.red : Color.white;
                weaponText.color = weapon.isOutOfAmmo ? Color.red : Color.white;
                if (weapon.isLastClip) {
                    ammoText.color = Color.yellow;
                    weaponText.color = Color.yellow;
                }

            }
            else
            {

                // Set
                weaponText.text = "Unarmed";
                ammoText.text = string.Empty;

                // Colors
                ammoText.color = Color.white;
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