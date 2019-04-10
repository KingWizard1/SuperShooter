using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SuperShooter
{

    public class PauseMenu : MonoBehaviour
    {

        public static bool paused;
        public GameObject pauseMenu;
        public AudioSource soundAudio;
        public Light dirLight;
        public Slider soundSlider;
        public Slider lightSlider;

        public GameObject soundButton, systemButton, keyButton;
        public GameObject soundPanel, systemPanel, keyPanel;
        //public GameObject soundImage1, systemImage1, keyImage1;
        //public GameObject soundText1, soundText2, systemText1, systemText2, keyText1, keyText2;


        // Use this for initialization
        void Start()
        {

            Time.timeScale = 1f;
            paused = false;
            pauseMenu.SetActive(false);

            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<FPSController>().enabled = true;
            
            soundAudio = GameObject.Find("Audio Source").GetComponent<AudioSource>();
            dirLight = GameObject.Find("Directional Light").GetComponent<Light>();
            soundSlider.value = PlayerPrefs.GetFloat("Audio Source");
            lightSlider.value = PlayerPrefs.GetFloat("Directional Light");
            return;

          
        }


        // Update is called once per frame
        void Update()
        {
            PlayerPrefs.SetFloat("Audio Source", soundAudio.volume);
            PlayerPrefs.SetFloat("Directional Light", dirLight.intensity);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (paused)
                {
                    Resume();

                }
                else
                {
                    Pause();
                }
            }

        }
 
        public void Resume()
        {
            paused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<FPSController>().enabled = true;


        }
        public void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
        public void Exitmenu()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        public void Volume()
        {
            soundAudio.volume = soundSlider.value;
        }
        public void Brightness()
        {
            dirLight.intensity = lightSlider.value;

        }
        public void OnClickSystemPanel()
        {
            systemPanel.SetActive(true);

            soundPanel.SetActive(false);

            keyPanel.SetActive(false);

        }
        public void OnClickSoundPanel()
        {
            systemPanel.SetActive(false);


            soundPanel.SetActive(true);


            keyPanel.SetActive(false);

        }
        public void OnclickKeyPanel()
        {

                systemPanel.SetActive(false);


                soundPanel.SetActive(false);


                keyPanel.SetActive(true);
            
        }
        public void Pause()
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            paused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<FPSController>().enabled = false;

            systemPanel.SetActive(false);


            soundPanel.SetActive(true);


            keyPanel.SetActive(false);




        }

    }
}
