using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyAnimationController : MonoBehaviour
{

    private new Animation animation;
    private AnimationState animationState;

    public string animationName;
    public float animationSpeed = 1;

    private void Awake()
    {
        
        animation = GetComponent<Animation>();
        

    }

    private void Start()
    {
        animationState = animation[animationName];
        animationState.speed = animationSpeed;

    }

#if UNITY_EDITOR
    private void Update()
    {
        animationState.speed = animationSpeed;
    }
#endif

    public void Play()
    {
        animation.Play();
    }

}
