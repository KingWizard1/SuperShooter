using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SuperShooter
{
    public class PlayerProgressionUI : MonoBehaviour
    {
        
        public TextMeshProUGUI xpText1;
        public TextMeshProUGUI xpText2;
        public TextMeshProUGUI xpText3;

        public Slider xpBar;
        public TextMeshProUGUI xpBarTextL;
        public TextMeshProUGUI xpBarTextC;
        public TextMeshProUGUI xpBarTextR;

        public float textFadeInTimer = 0.15f;
        public float textHoldTimer = 1f;
        public float textFadeOutTimer = 1f;

        // ------------------------------------------------- //

        private float XPText1Timer = 0;
        private float XPText2Timer = 0;
        private float XPText3Timer = 0;


        // ------------------------------------------------- //

        private void Start()
        {
            xpText1.enabled = false;
            xpText2.enabled = false;
            xpText3.enabled = false;

            ShowHideXPBar(false);
        }

        // ------------------------------------------------- //

        private void Update()
        {


            // XP Text
            //XPText1Timer += Time.deltaTime;
            //if (XPText1Timer > textFadeOutTimer)


        }

        // ------------------------------------------------- //

        public void ShowHideXPBar(bool show)
        {
            if (xpBar) xpBar.gameObject.SetActive(show);
            if (xpBarTextL) xpBarTextL.gameObject.SetActive(show);
            if (xpBarTextC) xpBarTextC.gameObject.SetActive(show);
            if (xpBarTextR) xpBarTextR.gameObject.SetActive(show);
        }

        // ------------------------------------------------- //

        public void SetXPBar(PlayerCharacter player)
        {
            

            // Set XP Bar
            if (xpBar) {
                xpBar.minValue = player.lastXPRequiredToLevel;
                xpBar.maxValue = player.currentXPRequiredToLevel;
                xpBar.value = player.currentXP;
            }

            if (xpBarTextL) xpBarTextL.text = player.currentXPLevel.ToString();
            if (xpBarTextR) xpBarTextR.text = (player.currentXPLevel + 1).ToString();
            if (xpBarTextC) xpBarTextC.text = $"{player.currentXP} / {player.currentXPRequiredToLevel}";

        }

        // ------------------------------------------------- //

        public void SetXPText1(string text)
        {
            xpText1.text = text;
            xpText1.enabled = true;
            xpText1.alpha = 100;
            xpText1.CrossFadeAlpha(0, 4f, false);
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //



    }
}