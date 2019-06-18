using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SuperShooter
{

    public class TargetUI : MonoBehaviour
    {
        [Header("State")]
        public CharacterEntity target;

        [Header("Target")]
        public Image healthbarBG;
        public Image healthbarFG;
        public TextMeshProUGUI healthbarText;

        [Header("Crosshair")]
        //public GameObject crossHair;
        public TargetCrossHairHitMarker crossHairHitMarker;


        // ------------------------------------------------- //

        private float healthbarWidth = 300;
        private float healthbarHeight = 75;

        // ------------------------------------------------- //

        private void Awake()
        {
            
        }

        // ------------------------------------------------- //

        private void Start()
        {
            // Set healthbars to the same size. FG will be set to BG's size.
            healthbarWidth = healthbarBG.rectTransform.rect.width;
            healthbarHeight = healthbarBG.rectTransform.rect.width;
            SetHealthbarFGRect(healthbarWidth/*, healthbarHeight*/);

            // Start hidden
            ShowHideTarget(false);

        }

        // ------------------------------------------------- //

        public void ShowHitMarker(Color color)
        {

            crossHairHitMarker.Show(color);


        }

        // ------------------------------------------------- //

        public void SetTarget(CharacterEntity character)
        {

            // Set target
            target = character;

            // Update target bar if not null. Otherwise, hide it.
            if (target != null) {

                // Set health bar width based on the character's health
                var percentHealth = (decimal)character.health / character.maxHealth * 100;
                var healthWidth = (decimal)healthbarWidth * percentHealth / 100;
                SetHealthbarFGRect((float)healthWidth);

                // Set health bar text to the character's name
                healthbarText.text = character.characterName;

                // Show the elements
                ShowHideTarget(true);
            }
            else
                ShowHideTarget(false);

        }

        // ------------------------------------------------- //

        private void SetHealthbarFGRect(float width)
        {
            //healthbarFG.rectTransform.sizeDelta = new Vector2(width, height);
            healthbarFG.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }

        private void ShowHideTarget(bool showMe)
        {
            healthbarBG.gameObject.SetActive(showMe);
            healthbarFG.gameObject.SetActive(showMe);
            healthbarText.gameObject.SetActive(showMe);
        }

        // ------------------------------------------------- //


    }

}