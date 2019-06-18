using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
[RequireComponent(typeof(TextMeshPro))]
public class TMPSetTextToName : MonoBehaviour
{

    [Header("Text")]
    public string text;
    public string prefix;

    [Header("Text Options")]
    public bool usePrefix = true;
    public bool useTwoLines = true;
    public bool setTextToName = true;

    [Header("Enable/Disable")]
    public bool worksInGame = false;
    public bool worksInEditor = true;

    // ------------------------------------------------- //

    private TextMeshPro tmp;

    // ------------------------------------------------- //

    private void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
    }


    private void Start()
    {


        //DisableIfNotAllowed();

        ApplyText();


    }


    // ------------------------------------------------- //

    private void ApplyText()
    {

        var prefixStr = (useTwoLines ? prefix + Environment.NewLine : prefix);
        var str = (usePrefix ? prefixStr : string.Empty);
        str += (setTextToName ? name : text);

        tmp.text = str;


    }


    private void DisableIfNotAllowed()
    {

        // Disable script if we're in the editor, but not allowed to run.
        if (Application.isEditor && !worksInEditor) {
            enabled = false;
            return;
        }

        // Disable script if we are in game, but not allowed to run.
        if (!Application.isEditor && !worksInGame) {
            enabled = false;
            return;
        }

    }


    // ------------------------------------------------- //

    void Update()
    {

        DisableIfNotAllowed();

        ApplyText();

    }



    // ------------------------------------------------- //




    // ------------------------------------------------- //

}
