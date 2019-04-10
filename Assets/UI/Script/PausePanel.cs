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
        public GameObject soundImage1,systemImage1, keyImage1;
        public GameObject soundText1, soundText2, systemText1, systemText2, keyText1, keyText2;
        // Use this for initialization
        void Start()
        {
            soundButton = GetComponent<GameObject>();
            systemButton = GetComponent<GameObject>();
            keyButton = GetComponent<GameObject>();
            soundPanel = GetComponent<GameObject>();
            systemPanel = GetComponent<GameObject>();
            keyPanel = GetComponent<GameObject>();
            soundImage1 = GetComponent<GameObject>();
            systemImage1 = GetComponent<GameObject>();
            keyImage1 = GetComponent<GameObject>();
            soundText1 = GetComponent<GameObject>();
            soundText2 = GetComponent<GameObject>();
            systemText1 = GetComponent<GameObject>();
            systemText2 = GetComponent<GameObject>();
            keyText1 = GetComponent<GameObject>();
            keyText2 = GetComponent<GameObject>();
            systemPanel.SetActive(false);
            keyPanel.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void ClickSoundPanel()
        {
            soundPanel.SetActive(true);
            soundImage1.SetActive(true);
            soundText2.SetActive(true);
            soundText1.SetActive(false);
            systemText2.SetActive(false);
            keyText2.SetActive(false);
            systemPanel.SetActive(false);
            keyPanel.SetActive(false);
            systemImage1.SetActive(false);
            keyImage1.SetActive(false);
        }
        public void ClickSystemPanel()
        {
            systemPanel.SetActive(true);
            systemImage1.SetActive(true);
            systemText2.SetActive(true);
            soundText1.SetActive(true);
            soundText2.SetActive(false);
            systemText1.SetActive(false);
            keyText2.SetActive(false);
            keyPanel.SetActive(false);           
            keyImage1.SetActive(false);
        }
    }
}
