using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    public class UIManager : MonoBehaviour
    {


        [Header("UI Elements")]
        public GameObject pickupPrompt;
        public GameObject pickupPrompt3D;

        // ------------------------------------------------- //

        // ------------------------------------------------- //

        public static UIManager Main { get; private set; }

        // ------------------------------------------------- //

        #region Initialisation

        void Awake()
        {
            Main = this;
        }

        // ------------------------------------------------- //

        void Start()
        {

        }

        #endregion

        // ------------------------------------------------- //

        void Update()
        {

        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //

        public void ShowPickupPrompt2D(string text)
        {
            if (pickupPrompt)
                pickupPrompt.GetComponent<PickupPrompt>().ShowPrompt(text);
            else
                Debug.LogError("[UI] No PickupPrompt assigned to UIManager.");

        }

        public void ShowPickupPrompt3D(string text, Vector3 objectPosition)
        {
            if (pickupPrompt3D)
                pickupPrompt3D.GetComponent<PickupPrompt3D>().ShowPrompt(text, objectPosition);
            else
                Debug.LogError("[UI] No PickupPrompt assigned to UIManager.");
        }

        public void HideAllPrompts()
        {
            //if (pickupPrompt)
            //    pickupPrompt.GetComponent<PickupPrompt>().HidePrompt();
            if (pickupPrompt3D)
                pickupPrompt3D.GetComponent<PickupPrompt3D>().HidePrompt();
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}