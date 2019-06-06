using UnityEngine;
using System.Collections;

namespace SuperShooter
{
    
    public class Spin : MonoBehaviour
    {

        public float speed = 10f;

        public Vector3 axis;        

        void Update()
        {

            transform.Rotate(axis, speed * Time.deltaTime);
        }
    } 
}