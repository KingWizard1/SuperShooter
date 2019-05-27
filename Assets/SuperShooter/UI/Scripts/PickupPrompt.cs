using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SuperShooter
{

    public class PickupPrompt : MonoBehaviour
    {

        private TextMeshProUGUI promptText;
        private TextMeshProUGUI promptKeyText;
        private GameObject promptKeySprite;

        // ------------------------------------------------- //

        private void Awake()
        {

            // Find my own objects
            foreach (Transform t in transform)
            {
                if (t.name == "PromptText") promptText = t.GetComponent<TextMeshProUGUI>();
                if (t.name == "PromptKeyText") promptKeyText = t.GetComponent<TextMeshProUGUI>();
                if (t.name == "PromptKeySprite") promptKeySprite = t.gameObject;
            }

        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //

        public void ShowPrompt(string text)
        {
            promptText.enabled = true;
            promptText.text = text;

            promptKeySprite.SetActive(true);
            promptKeyText.enabled = true;
        }

        public void HidePrompt()
        {
            promptText.enabled = false;
            promptKeySprite.SetActive(false);
            promptKeyText.enabled = false;
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}