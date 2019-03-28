using UnityEngine;
using System.Collections;

namespace KingWizard
{

    public class Spin : MonoBehaviour
    {
        public float speed = 10f;


        void Update()
        {
            transform.Rotate(-Vector3.forward, speed * Time.deltaTime);
        }
    } 
}