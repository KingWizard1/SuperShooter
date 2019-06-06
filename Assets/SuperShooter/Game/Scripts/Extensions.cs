using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SuperShooter
{

    public static class TransformExt
    {

        /// <summary>Fires a raycast downward and returns true if it hits a collider.</summary>
        public static bool CheckIfGrounded(this Transform t, out RaycastHit hit, float rayDistance = 1)
        {

            Ray groundRay = new Ray(t.position, -t.up);
            bool isGrounded = Physics.Raycast(groundRay, out hit, rayDistance);
            return isGrounded;

        }


        //// Commented because method already exists in type 'Transform'.
        ///// <summary>Traverse up the heirarchy for the first parent object
        ///// that has the specified component attached to it, and returns it.</summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="transform"></param>
        ///// <returns></returns>
        //public static T GetComponentInParent<T>(this Transform transform)
        //{
        //    var current = transform;
        //    while (current.parent != null) {
        //        current = current.parent;
        //        T component = current.GetComponent<T>();
        //        if (component != null)
        //            return component;
        //    }
        //    return default;
        //}

    }

    public static class ColorExt
    {

        /// <summary>Changes the alpha component of a <see cref="Color"/>.</summary>
        public static Color ChangeAlpha(this Color color, float newAlpha)
        {
            return new Color(color.r, color.g, color.b, newAlpha);
        }

    }
}
