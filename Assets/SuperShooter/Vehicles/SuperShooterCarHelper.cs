using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperShooterCarHelper : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    //public bool Skidding { get; private set; }
    //public float BrakeInput { get; private set; }
    //public float CurrentSteerAngle { get { return m_SteerAngle; } }
    //public float CurrentSpeed { get { return m_Rigidbody.velocity.magnitude * 2.23693629f; } }
    //public float MaxSpeed { get { return m_Topspeed; } }
    //public float Revs { get; private set; }
    //public float AccelInput { get; private set; }

    float lastSpeed = 0;

    void Update()
    {
        // Speed
        //var controller = GetComponent<UnityStandardAssets.Vehicles.Car.CarController>();
        //bool accel = controller.CurrentSpeed > lastSpeed;
        //Debug.Log((accel ? "^ " : "v ") + controller.CurrentSpeed);
        //lastSpeed = controller.CurrentSpeed;
        

    }

    private void LateUpdate()
    {
        //// Audio effects
        //var localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer");
        //if (localPlayer)
        //{
        //    var direction = transform.position - localPlayer.transform.position;
        //    bool isSomethingInTheWay = Physics.Raycast(transform.position, direction, 50f);
        //    if (isSomethingInTheWay)
        //        foreach (AudioSource source in GetComponents<AudioSource>())
        //        {
        //            source.volume = Mathf.Lerp(source.volume, 0, 10);
        //        }
        //}



    }

}
