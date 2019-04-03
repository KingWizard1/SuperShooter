using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SuperShooter
{

    public class PausePanel : MonoBehaviour
    {
        public GameObject soundButton, systemButton, keyButton;
        public GameObject soundPanel, systemPanel, keyPanel;
        public Image soundImage1, soundImage2, systemImage1, systemImage2, keyImage1, keyImage2;
        // Use this for initialization
        void Start()
        {
            soundButton = GetComponent<GameObject>();
            systemButton = GetComponent<GameObject>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
