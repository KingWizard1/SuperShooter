using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{

    /// <summary>Loops through all children of this object and
    /// sets their tag to match the tag of this object.</summary>
    public class AutoSetChildTags : MonoBehaviour
    {

        void Awake()
        {
            // Didnt work for children of child objects (not recursive)
            //foreach (Transform t in transform)
            //{
            //    t.tag = tag;
            //}

            // Alternative approach
            var colliderGameObjects = GetComponentsInChildren<Collider>().Select(c => c.gameObject);
            foreach (var go in colliderGameObjects)
            {
                go.tag = tag;
            }
        }

    }

    

}