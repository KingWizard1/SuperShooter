//using System.Linq;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace SuperShooter
//{

//    public class Projectile : MonoBehaviour
//    {
//        public float scale = 1f;
//        public int amount = 10;
//        public Vector3 force = new Vector3(0, 0, 10);
//        public Vector3 gravity = new Vector3(0f, -9.7f, 0f);
        
//        [Header("Debug")]
//        public float radius = .1f;
//        public bool drawGizmos = false;

//        [Header("Bullet")]
//        //public Transform bullet;
//        public float speed = 10f;

//        private int targetwaypoint = 0;
//        private Vector3 startPoint, endPoint;
//        private Quaternion startRotation, endRotation;
//        private float startTime = 0f;
//        private float percentage = 0f;

//        private List<Vector3> trajectory = new List<Vector3>();

//        void CalculateTrajectory(Vector3 force)
//        {
//            // Reset list (for testing)
//            trajectory = new List<Vector3>();
//            // Start point from transform
//            Vector3 point = transform.position;
//            // Loop through 1 - amount
//            for (int i = 1; i < amount; i++)
//            {
//                float frame = (float)i / (float)amount;
//                Vector3 pull = gravity * frame;

//                Vector3 prevPoint = point;
//                // Percentage of current iteration
//                point += (force + pull).normalized * scale;

//                // Perform Raycast here
//                Ray ray = new Ray(prevPoint, point - prevPoint);
//                float distance = Vector3.Distance(prevPoint, point);
//                RaycastHit hit;
//                if (Physics.Raycast(ray, out hit, distance))
//                {
//                    // Ignore other bullets
//                    if (hit.transform.GetComponent<Projectile>())
//                        continue;

//                    trajectory.Add(hit.point);

//                    //Debug.Log("Target is " + hit.transform.name + " at " + endPoint);

//                    // Exit function. We've found our target.
//                    return;
//                }

//                // Add point to trajectory
//                trajectory.Add(point);
//            }
//        }

//        void DrawPoints(List<Vector3> points)
//        {
//            Gizmos.color = Color.blue;
//            foreach (var point in points)
//            {
//                Gizmos.DrawSphere(point, radius);
//            }
//            Gizmos.color = Color.red;
//            for (int i = 0; i < points.Count - 1; i++)
//            {
//                Vector3 pointA = points[i];
//                Vector3 pointB = points[i + 1];
//                Gizmos.DrawLine(pointA, pointB);
//            }
//        }

//        void OnDrawGizmos()
//        {
//            if (!drawGizmos)
//                return;

//            CalculateTrajectory(transform.forward);
//            DrawPoints(trajectory);

//            Gizmos.color = Color.magenta;
//            Gizmos.DrawSphere(trajectory.Last(), .15f);

//            Gizmos.color = Color.yellow;
//            Gizmos.DrawLine(startPoint, endPoint);

//        }

//        void Start()
//        {
//            CalculateTrajectory(transform.forward);

//            startRotation = transform.rotation;
//            endRotation = transform.rotation;

//            startPoint = transform.position;
//            endPoint = trajectory[targetwaypoint];

//            startTime = Time.time;
//        }

//        void FixedUpdate()
//        {
//            float distance = Vector3.Distance(startPoint, endPoint);
//            float duration = speed / distance;
//            float fraction = duration * scale;
//            percentage += fraction * Time.deltaTime;

//            transform.position = Vector3.Lerp(startPoint, endPoint, percentage);
//            transform.rotation = Quaternion.Lerp(startRotation, endRotation, percentage);

//            //Debug.Log(percentage);

//            // Percentage represents the amount of distance that has been travelled
//            // between the start point and end point of the current segment.

//            Debug.Log(targetwaypoint + " / " + trajectory.Count + " (" + percentage + ")");

//            if (percentage >= 1f)
//            {
//                percentage = 0f;
//                startTime = Time.time;

//                // Have we reached the last waypoint in the trajectory?
//                if (targetwaypoint == trajectory.Count)
//                {
//                    Debug.Log("Reached last waypoint");

//                    // Perform Raycast here
//                    Ray bulletRay = new Ray(startPoint, endPoint - startPoint);
//                    float rayDistance = Vector3.Distance(startPoint, endPoint);
//                    RaycastHit hit;
//                    if (Physics.Raycast(bulletRay, out hit, rayDistance))
//                    {
//                        // Ignore other bullets
//                        if (!hit.transform.GetComponent<Projectile>())
//                        {

//                            endPoint = hit.point;

//                            Debug.Log("Hit " + hit.transform.name + " at " + hit.point);
//                            UIManager.Main.CrossHairSystem.ShowHitMarker(Color.white);
//                        }


//                    }

//                    // Destroy self.
//                    Destroy(gameObject);
//                }
//                else
//                {
//                    // increment to next waypoint.
//                    targetwaypoint++;

//                    // If we're not at the final point yet
//                    if (targetwaypoint < trajectory.Count)
//                    {
//                        // Assign the new lerp waypoints to head toward
//                        startPoint = endPoint;
//                        endPoint = trajectory[targetwaypoint];

//                        // Rotation
//                        Ray bulletRay = new Ray(startPoint, endPoint - startPoint);

//                        startRotation = transform.rotation;
//                        endRotation = Quaternion.LookRotation(bulletRay.direction);
//                    }
//                }
//            }
//        }
//    }

//}