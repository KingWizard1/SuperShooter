using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AreaUI : MonoBehaviour
{

    public GameObject enemyIcon;
    public TextMeshProUGUI enemyText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI objectiveText;

    public float objectiveFadeOutTime = 5f;

    // ------------------------------------------------- //


    // ------------------------------------------------- //

    void Start()
    {

        SetEnemyCount(0);

        timerText.text = string.Empty;
        objectiveText.text = string.Empty;

    }

    // ------------------------------------------------- //

    void Update()
    {
        
    }

    // ------------------------------------------------- //

    public void SetAreaTimerText(string text)
    {
        timerText.text = text;
    }

    // ------------------------------------------------- //

    public void ShowObjectiveText(string text, bool underlined = false)
    {
        objectiveText.text = text;
        objectiveText.fontStyle = underlined ? FontStyles.Underline : FontStyles.Normal;
        objectiveText.canvasRenderer.SetAlpha(1);                       // Reset the canvas alpha back to 1.
        objectiveText.CrossFadeAlpha(0, objectiveFadeOutTime, false);   // Changes the canvasRenderer's alpha. Not the text color alpha.
    }

    // ------------------------------------------------- //

    public void SetEnemyCount(int count)
    {
        if (count == 0) {
            enemyText.text = string.Empty;
            enemyIcon.SetActive(false);
        }
        else {
            enemyText.text = count.ToString();
            enemyIcon.SetActive(true);
        }

    }

    // ------------------------------------------------- //


    // ------------------------------------------------- //


}
