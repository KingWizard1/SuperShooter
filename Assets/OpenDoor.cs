using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    public Animator anim;




    // Start is called before the first frame update
    void Start()
    {

        anim = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetMouseButtonDown(0))

        {

            anim.SetTrigger("open");



        }
    }
}
