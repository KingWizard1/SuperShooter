using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SuperShooter
{
    public static class ColorExt
    {

        /// <summary>Changes the alpha component of a <see cref="Color"/>.</summary>
        public static Color ChangeAlpha(this Color color, float newAlpha)
        {
            return new Color(color.r, color.g, color.b, newAlpha);
        }

    }
}
