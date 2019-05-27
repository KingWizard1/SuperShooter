using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnlargeMap : MonoBehaviour {
    public GameObject largeMap;
    public bool enlarge;

	// Use this for initialization
	void Start () {
        largeMap = GetComponent<GameObject>();

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.M))
        {
            if (enlarge) { largeMap.SetActive(true); } else { largeMap.SetActive(false); } 
        }
        
    }
}
