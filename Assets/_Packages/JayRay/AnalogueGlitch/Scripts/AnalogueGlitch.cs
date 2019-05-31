using UnityEngine;

namespace JayRay
{

    //[ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("JayRay/Analogue Glitch")]
    public class AnalogueGlitch : MonoBehaviour
    {
        //[Header("Analogue Glitch")]
        //[Tooltip("Enable/disable glitch effects.")]
        //public bool glitch = true;

        // ------------------------------------------------- //

        [SerializeField]
        public Shader shader;

        [SerializeField, Range(0, 1)]
        public float _ScanLineJitter;

        [SerializeField, Range(0, 1)]
        public float _VerticalJump;

        [SerializeField, Range(0, 1)]
        public float _HorizontalShake;

        [SerializeField, Range(0, 1)]
        public float _ColourDrift;

        // ------------------------------------------------- //

        private Material _material;

        private float _verticalJumpTime;

        // ------------------------------------------------- //

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            //if (!glitch)
            //    return;

            if (_material == null)
            {
                _material = new Material(shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            _verticalJumpTime += Time.deltaTime * _VerticalJump * 11.3f;

            var s1_thresh = Mathf.Clamp01(1.0f + _ScanLineJitter * 1.2f);
            var s1_disp = 0.002f + Mathf.Pow(_ScanLineJitter, 3) * 0.05f;
            _material.SetVector(nameof(_ScanLineJitter), new Vector2(s1_disp, s1_thresh));
            var vj = new Vector2(_VerticalJump, _verticalJumpTime);
            _material.SetVector(nameof(_VerticalJump), vj);
            _material.SetFloat(nameof(_HorizontalShake), _HorizontalShake);
            var cd = new Vector2(_ColourDrift * 0.04f, Time.time * 606.11f);
            _material.SetVector(nameof(_ColourDrift), cd);
            Graphics.Blit(source, destination, _material);
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}