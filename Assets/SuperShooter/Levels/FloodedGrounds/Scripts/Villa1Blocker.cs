﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villa1Blocker : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject enemy;
    // Update is called once per frame
    void Update()
    {


        if (enemy == null)
        {

            Destroy(this.gameObject);

        }
    }
}
