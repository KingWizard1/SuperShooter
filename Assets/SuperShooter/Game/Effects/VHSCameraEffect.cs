using JayRay;
using UnityEngine;

namespace SuperShooter
{

    /// <summary>Handles all 80's/Outrun/Synthwave concepts and effects in the game</summary>
    public class VHSCameraEffect : AnalogueGlitch
    {

        [Header("Scanlines")]
        public Material scanlineMat;
        public float scanlineOpacity;

        // ------------------------------------------------- //

        private GameObject _scanLinePlaneObj;
        private Renderer _scanLinePlaneRend;

        // ------------------------------------------------- //

        private void Awake()
        {

            // Add scanline effect object to this camera object.
            // We achieve this by creating a plane and placing it in front of the camera, close to it.
            // We then apply a scanline material to the plane to create the effect, then tweak some parameters.
            _scanLinePlaneObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            _scanLinePlaneObj.transform.SetParent(transform);
            _scanLinePlaneObj.transform.localScale = new Vector3(1f, 1f, 1f);
            _scanLinePlaneObj.transform.localPosition = new Vector3(0f, 0f, 1f);
            _scanLinePlaneObj.transform.Rotate(-90, 0, 0);  // Rotate so the forward-normal is facing us (the camera)
            _scanLinePlaneObj.SetActive(enabled);
            _scanLinePlaneObj.name = "VHSScanlines";

            // Get the renderer and set the specified material
            _scanLinePlaneRend = _scanLinePlaneObj.GetComponent<Renderer>();
            _scanLinePlaneRend.sharedMaterial = scanlineMat;

            // Set opacity value to starting opacity value
          //  _scanLinePlaneRend.sharedMaterial.SetFloat("Opacity", scanlineOpacity);

        }

        private void Start()
        {


        }

        // ------------------------------------------------- //

        private void OnEnable()
        {
            _scanLinePlaneObj?.SetActive(true);
        }

        private void OnDisable()
        {
            _scanLinePlaneObj?.SetActive(false);
        }

        private void OnDestroy()
        {
            Destroy(_scanLinePlaneObj);
        }

        // ------------------------------------------------- //

        private void Update()
        {
            //var has = _scanLinePlaneRend.sharedMaterial.HasProperty("Opacity");

            //// Update rend materials if anything has changed.
            //// We do difference checks to prevent the renderer from having to do extra work.
            //if (_scanLinePlaneRend.sharedMaterial.GetFloat("Opacity") != scanlineOpacity)
            //    _scanLinePlaneRend.sharedMaterial.SetFloat("Opacity", scanlineOpacity);



        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


    }

}