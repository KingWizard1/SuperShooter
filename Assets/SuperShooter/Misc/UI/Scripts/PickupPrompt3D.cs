using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SuperShooter
{

    public class PickupPrompt3D : MonoBehaviour
    {

        public TextMeshPro promptText;
        //private GameObject promptKeySprite;

        // ------------------------------------------------- //

        private void Awake()
        {

        }

        // ------------------------------------------------- //

        private void Update()
        {
            if (promptText.enabled)
            {
                transform.RotateToMainCamera();
            }
        }

        // ------------------------------------------------- //

        public void ShowPrompt(string text, Vector3 promptPosition)
        {
            promptText.enabled = true;
            promptText.text = text;

            transform.position = promptPosition + new Vector3(0, 1);

            //promptKeySprite.SetActive(true);
            //promptKeyText.enabled = true;
        }

        public void HidePrompt()
        {
            promptText.enabled = false;
            //promptKeySprite.SetActive(false);
            //promptKeyText.enabled = false;
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}