using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    public class Projectile : MonoBehaviour
    {
        public float scale = 1f;
        public int amount = 10;
        public Vector3 force = new Vector3(0, 0, 10);
        public Vector3 gravity = new Vector3(0f, -9.7f, 0f);

        [Header("Bullet")]
        //public Transform bullet;
        public float speed = 10f;

        private int targetwaypoint = 0;
        private Vector3 startPoint, endPoint, hitPoint;
        private Quaternion startRotation, endRotation;
        private float startTime = 0f;
        private float percentage = 0f;

        private List<Vector3> trajectory = new List<Vector3>();

        void Calculate(Vector3 force)
        {
            // Reset list (for testing)
            trajectory = new List<Vector3>();
            // Start point from transform
            Vector3 point = transform.position;
            // Loop through 1 - amount
            for (int i = 1; i < amount; i++)
            {
                float frame = (float)i / (float)amount;
                Vector3 pull = gravity * frame;

                Vector3 prevPoint = point;
                // Percentage of current iteration
                point += (force + pull).normalized * scale;

                // Perform Raycast here
                Ray ray = new Ray(prevPoint, point - prevPoint);
                float distance = Vector3.Distance(prevPoint, point);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, distance))
                {
                    // Ignore other bullets
                    if (hit.transform.GetComponent<Projectile>())
                        continue;

                    trajectory.Add(hit.point);
                    return;
                }

                // Add point to trajectory
                trajectory.Add(point);
            }
        }

        void DrawPoints(List<Vector3> points)
        {
            Gizmos.color = Color.blue;
            foreach (var point in points)
            {
                Gizmos.DrawSphere(point, .1f);
            }
            Gizmos.color = Color.white;
            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector3 pointA = points[i];
                Vector3 pointB = points[i + 1];
                Gizmos.DrawLine(pointA, pointB);
            }
        }

        void OnDrawGizmos()
        {
            Calculate(transform.forward);
            DrawPoints(trajectory);

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(trajectory.Last(), .2f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(startPoint, endPoint);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hitPoint, .2f);

        }

        void Start()
        {
            Calculate(transform.forward);

            startRotation = transform.rotation;
            endRotation = transform.rotation;

            startPoint = transform.position;
            endPoint = trajectory[targetwaypoint];

            startTime = Time.time;
        }
        
        void Update()
        {
            float distance = Vector3.Distance(startPoint, endPoint);
            float duration = speed / distance;
            float fraction = duration * scale;
            percentage += fraction * Time.deltaTime;

            transform.position = Vector3.Lerp(startPoint, endPoint, percentage);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, percentage);

            if (percentage >= 1f)
            {
                percentage = 0f;
                startTime = Time.time;
                // increment and wrap the target waypoint index
                targetwaypoint++;
                if (targetwaypoint < trajectory.Count)
                {
                    // assign the new lerp waypoints
                    startPoint = endPoint;
                    endPoint = trajectory[targetwaypoint];

                    // Perform Raycast here
                    Ray bulletRay = new Ray(startPoint, endPoint - startPoint);
                    float rayDistance = Vector3.Distance(startPoint, endPoint);
                    RaycastHit hit;
                    if (Physics.Raycast(bulletRay, out hit, rayDistance))
                    {
                        // Ignore other bullets
                        if (!hit.transform.GetComponent<Projectile>())
                        {
                            endPoint = hit.point;
                            hitPoint = hit.point;
                        }

                    }

                    startRotation = transform.rotation;
                    endRotation = Quaternion.LookRotation(bulletRay.direction);
                }
                else
                {

                    RaycastHit hit;
                    var origin = transform.position;
                    var radius = .2f;
                    if (Physics.SphereCast(origin, radius, transform.forward, out hit, 10))
                    {

                        UIManager.Main.CrossHair.ShowHitMarker(Color.white);

                        if (hit.rigidbody)
                        {
                            // Calculate push direction from move direction.
                            var pushDir = hit.transform.position - transform.position;

                            // Apply!
                            hit.rigidbody.velocity = pushDir * (speed / 8);

                        }

                    }


                    Destroy(gameObject);
                }
            }
        }
    }

}