using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class UIButtonSoundEvent : MonoBehaviour
{
    private AudioSource source;
    public AudioClip hover, click;

    public void OnPointerEnter(PointerEventData point)
    {
        
        source.PlayOneShot(hover);
        print("wtf");
    }

    public void OnPointerDown()
    {
        source.PlayOneShot(click);
        print("ohhh");
    }
}