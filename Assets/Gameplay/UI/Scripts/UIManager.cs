using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    public class UIManager : MonoBehaviour
    {


        [Header("UI Elements")]
        public GameObject pickupPrompt;


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

        public void ShowPickupPrompt(string text)
        {
            if (pickupPrompt)
                pickupPrompt.GetComponent<PickupPrompt>().ShowPrompt(text);
            else
                Debug.LogError("[UI] No PickupPrompt assigned to UIManager.");

        }

        public void HideAllPrompts()
        {
            if (pickupPrompt)
                pickupPrompt.GetComponent<PickupPrompt>().HidePrompt();
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}