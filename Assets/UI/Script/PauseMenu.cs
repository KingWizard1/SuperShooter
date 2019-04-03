using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace superShooter
{

    public class PauseMenu : MonoBehaviour
    {

        public static bool paused;
        public bool showOption;
        public GameObject pauseMenu;
        public AudioSource soundAudio;
        public Light dirLight;
        public Slider soundSlider;
        public Slider lightSlider;

        public GameObject soundButton, systemButton, keyButton;
        public GameObject soundPanel, systemPanel, keyPanel;
        public GameObject soundImage1, systemImage1, keyImage1;
        //public GameObject soundText1, soundText2, systemText1, systemText2, keyText1, keyText2;


        // Use this for initialization
        void Start()
        {

            Time.timeScale = 1f;
            paused = false;
            pauseMenu.SetActive(false);


            soundAudio = GameObject.Find("Audio Source").GetComponent<AudioSource>();
            dirLight = GameObject.Find("Directional Light").GetComponent<Light>();
            soundSlider.value = PlayerPrefs.GetFloat("Audio Source");
            lightSlider.value = PlayerPrefs.GetFloat("Directional Light");
            return;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                paused = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;


                systemPanel.SetActive(false);
                systemImage1.SetActive(false);

                soundPanel.SetActive(true);
                soundImage1.SetActive(true);

                keyPanel.SetActive(false);
                keyImage1.SetActive(false);
                OnClickSystemPanel();
                OnClickSoundPanel();
                OnclickKeyPanel();

            }
            PlayerPrefs.SetFloat("Audio Source", soundAudio.volume);
            PlayerPrefs.SetFloat("Directional Light", dirLight.intensity);
        }
 
        public void Resume()
        {
            paused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


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
            systemImage1.SetActive(true);

            soundPanel.SetActive(false);
            soundImage1.SetActive(false);

            keyPanel.SetActive(false);
            keyImage1.SetActive(false);
        }
        public void OnClickSoundPanel()
        {
            systemPanel.SetActive(false);
            systemImage1.SetActive(false);

            soundPanel.SetActive(true);
            soundImage1.SetActive(true);

            keyPanel.SetActive(false);
            keyImage1.SetActive(false);
        }
        public void OnclickKeyPanel()
        {

                systemPanel.SetActive(false);
                systemImage1.SetActive(false);

                soundPanel.SetActive(false);
                soundImage1.SetActive(false);

                keyPanel.SetActive(true);
                keyImage1.SetActive(true);
            
        }
    }
}
