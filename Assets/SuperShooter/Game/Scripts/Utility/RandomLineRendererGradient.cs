using UnityEngine;
using System.Collections;

namespace SuperShooter
{

    public class RandomLineRendererGradient : MonoBehaviour
    {
        private LineRenderer lr;

        void Start()
        {
            lr = GetComponent<LineRenderer>();
            //lr.material = new Material(Shader.Find("Sprites/Default"));

            // Set some positions
            //Vector3[] positions = new Vector3[3];
            //positions[0] = new Vector3(-2.0f, -2.0f, 0.0f);
            //positions[1] = new Vector3(0.0f, 2.0f, 0.0f);
            //positions[2] = new Vector3(2.0f, -2.0f, 0.0f);
            //lr.positionCount = positions.Length;
            //lr.SetPositions(positions);


            var randomColor = Random.ColorHSV(0, 1, .8f, 1, .8f, .8f, 1, 1);

            // A simple 2 color gradient with a fixed alpha of 1.0f.
            float alpha = 1.0f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(randomColor, 0.0f)/*, new GradientColorKey(randomColor, 1.0f)*/ },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
            lr.colorGradient = gradient;
        }

    }
}