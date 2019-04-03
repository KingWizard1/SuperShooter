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
    public GameObject pauseMenu, optionMenu;
    public AudioSource soundAudio;
    public Light dirLight;
    public Slider soundSlider;
    public Slider lightSlider;



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
            TogglePause();
        }
    }
    public void ToggleOption()
    {
        if (showOption)
        {
            showOption = false;
            pauseMenu.SetActive(true);
            optionMenu.SetActive(false);


        }
        else
        {
            showOption = true;
            pauseMenu.SetActive(false);
            optionMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            soundSlider = GameObject.Find("AudioSlider").GetComponent<Slider>();
            lightSlider = GameObject.Find("Brightness").GetComponent<Slider>();
            soundSlider.value = soundAudio.volume;
            lightSlider.value = dirLight.intensity;



        }
        optionMenu.SetActive(showOption);
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
        PlayerPrefs.SetFloat("Audio Source", soundAudio.volume);
        PlayerPrefs.SetFloat("Directional Light", dirLight.intensity);
        SceneManager.LoadScene(0);
    }
    public void Exitmenu()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    public void TogglePause()
    {

        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //player.GetComponent<CharacterMovement>().enabled = false;
        //player.GetComponent<MouseLook>().enabled = false;
        //mainCam.GetComponent<MouseLook>().enabled = false;


    }
    public void BackMenu()
    {
        showOption = false;
        pauseMenu.SetActive(true);
        optionMenu.SetActive(false);
        return;
    }
    public void Volume()
    {
        soundAudio.volume = soundSlider.value;
    }
    public void Brightness()
    {
        dirLight.intensity = lightSlider.value;

    }
}
}
