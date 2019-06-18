using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperShooter
{

    public class TargetCrossHairHitMarker : MonoBehaviour
    {
        [Header("Colours")]
        public Color color = Color.white;

        [Header("References")]
        public GameObject TopLeftMarker;
        public GameObject TopRightMarker;
        public GameObject BottomLeftMarker;
        public GameObject BottomRightMarker;

        // ------------------------------------------------- //

        // References
        private Image t1, t2, t3, t4;

        // Mechanics
        private bool show = false;
        public float fadeInTime = .05f;
        public float fadeOutTime = .1f;
        private float fadeInTimer = 0;      //  0 is default value.
        private float fadeOutTimer = -1;    // -1 is default value.

        // ------------------------------------------------- //

        private void Awake()
        {
            t1 = TopLeftMarker.GetComponent<Image>();
            t2 = TopRightMarker.GetComponent<Image>();
            t3 = BottomLeftMarker.GetComponent<Image>();
            t4 = BottomRightMarker.GetComponent<Image>();
        }

        // ------------------------------------------------- //

        private void Start()
        {
            // Invisible to start
            SetColors(color.ChangeAlpha(show ? 255 : 0));
        }

        // ------------------------------------------------- //

        private void Update()
        {
            // Debug
            //if (Input.GetKeyDown(KeyCode.Q))
            //    show = !show;

            if (show)
                FadeIn();
            else
                FadeOut();

        }

        // ------------------------------------------------- //

        private void FadeIn()
        {
            // Set new alpha
            var alpha = Mathf.Lerp(0, 1, fadeInTimer / fadeInTime);
            SetColors(color.ChangeAlpha(alpha));

            // Increment timer
            fadeInTimer += Time.deltaTime;

            // Check if we're done
            if (fadeInTimer >= fadeInTime) {
                SetColors(color.ChangeAlpha(1));    // Ensure.
                fadeInTimer = 0;
                fadeOutTimer = 0;   // Allow fade out to start.
                show = false;
            }
        }

        private void FadeOut()
        {
            // Prevent updating colors after a fade out has completed (see below).
            if (fadeOutTimer == -1)
                return;

            // Set new alpha
            var alpha = Mathf.Lerp(1, 0, fadeOutTimer / fadeOutTime);
            SetColors(color.ChangeAlpha(alpha));

            // Increment timer
            fadeOutTimer += Time.deltaTime;

            // Check if we're done
            if (fadeOutTimer >= fadeOutTime) {
                SetColors(color.ChangeAlpha(0));    // Ensure.
                fadeOutTimer = -1; // Signals not to redraw on next update.
            }
        }

        private void SetColors(Color color)
        {
            t1.color = color;
            t2.color = color;
            t3.color = color;
            t4.color = color;
        }

        // ------------------------------------------------- //

        public void Show(Color color)
        {
            this.color = color;
            show = true;
        }

        public void Hide()
        {

        }

        // ------------------------------------------------- //

    }

}