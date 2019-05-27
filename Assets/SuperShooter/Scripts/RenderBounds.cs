using UnityEngine;

namespace SuperShooter
{
    /// <summary>Draws the render bounds for an object. Useful during development.</summary>
    public class RenderBounds : MonoBehaviour
    {
        private Renderer rend;

        private void Awake()
        {
            rend = GetComponent<Renderer>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(rend.bounds.center, rend.bounds.size);
        }
    }
}
