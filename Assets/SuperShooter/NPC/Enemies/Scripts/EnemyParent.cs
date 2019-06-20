﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    /// <summary>Used to look after the parent game object whose
    /// child objects consist of enemy objects.</summary>
    public class EnemyParent : MonoBehaviour
    {
        // Start is called before the first frame update

        public bool spawned;
        // Update is called once per frame

        public void Update()
        {
            
            if (transform.childCount < 1 && spawned == true)
            {
                spawned = false;
                this.gameObject.SetActive(false);
                
            }
            if (transform.childCount > 0)
            {
                spawned = true;
            }
        }

    

        private void OnDestroy()
        {
            if (transform.parent != null) // if object has a parent
            {
                if (transform.childCount <= 1) // if this object is the last child
                {
                    Destroy(transform.parent.gameObject, 0.1f); // destroy parent a few frames later
                }
            }
        }
    }

}