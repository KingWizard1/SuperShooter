using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SuperShooter
{
    public class PlayerProgressionUI : MonoBehaviour
    {
        [Header("XP Texts")]
        public TextMeshProUGUI xpBulletHit;
        public TextMeshProUGUI xpEnemyKilled;
        public TextMeshProUGUI xpHeadshot;
        public TextMeshProUGUI xpWaveCompleted;
        public TextMeshProUGUI xpAreaCleared;
        public float xpTextFadeoutTime = 4f;

        [Header("XP Bar")]
        public Slider xpBar;
        public TextMeshProUGUI xpBarTextL;
        public TextMeshProUGUI xpBarTextC;
        public TextMeshProUGUI xpBarTextR;

        // ------------------------------------------------- //



        // ------------------------------------------------- //

        private void Start()
        {
            ShowBulletHit(0);
            ShowEnemyKilled(0);
            ShowEnemyHeadshot(0);
            ShowWaveCompleted(0);
            ShowAreaCleared(0);

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

        public void ShowBulletHit(int xp)
        {
            if (xp == 0) xpBulletHit.text = string.Empty;
            else xpBulletHit.text = $"Bullet Hit +{xp}";
            xpBulletHit.canvasRenderer.SetAlpha(1);
            xpBulletHit.CrossFadeAlpha(0, xpTextFadeoutTime, false);
        }

        public void ShowEnemyKilled(int xp)
        {
            if (xp == 0) xpEnemyKilled.text = string.Empty;
            else xpEnemyKilled.text = $"Enemy Killed +{xp}";
            xpEnemyKilled.canvasRenderer.SetAlpha(1);
            xpEnemyKilled.CrossFadeAlpha(0, xpTextFadeoutTime, false);
        }

        public void ShowEnemyHeadshot(int xp)
        {
            if (xp == 0) xpHeadshot.text = string.Empty;
            else xpHeadshot.text = $"Headshot! +{xp}";
            xpHeadshot.canvasRenderer.SetAlpha(1);
            xpHeadshot.CrossFadeAlpha(0, xpTextFadeoutTime, false);
        }

        public void ShowWaveCompleted(int xp)
        {
            if (xp == 0) xpWaveCompleted.text = string.Empty;
            else xpWaveCompleted.text = $"Wave Completed! +{xp}";
            xpWaveCompleted.canvasRenderer.SetAlpha(1);
            xpWaveCompleted.CrossFadeAlpha(0, xpTextFadeoutTime, false);
        }

        public void ShowAreaCleared(int xp)
        {
            if (xp == 0) xpAreaCleared.text = string.Empty;
            else xpAreaCleared.text = $"+{xp}";
            xpAreaCleared.canvasRenderer.SetAlpha(1);
            xpAreaCleared.CrossFadeAlpha(0, xpTextFadeoutTime, false);
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


        // ------------------------------------------------- //


        // ------------------------------------------------- //



    }
}