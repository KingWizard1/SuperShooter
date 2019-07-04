using UnityEngine;
using TMPro;

namespace SuperShooter
{

    public class DeathUI : MonoBehaviour
    {

        public TMPController deathText;
        public TextMeshProUGUI deathText2;

        // ------------------------------------------------- //


        // ------------------------------------------------- //

        public void Show(string deathText, string deathReason)
        {

            this.deathText.SetText(deathText);
            this.deathText2.text = deathReason;

            gameObject.SetActive(true);

        }

        public void Hide()
        {

            gameObject.SetActive(false);

        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}