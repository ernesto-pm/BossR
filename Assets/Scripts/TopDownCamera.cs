using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
    {
        [Header("Framing")]
        public Camera Camera;
        public Vector2 FollowPointFraming = new Vector2(0f, 0f);
        public float FollowingSharpness = 10000f;

        [Header("Distance")]
        public float DefaultDistance = 15f; // Increased for better top-down view
        public float MinDistance = 5f;
        public float MaxDistance = 30f;
        public float DistanceMovementSpeed = 5f;
        public float DistanceMovementSharpness = 10f;

        [Header("Rotation")]
        public bool RotateWithPhysicsMover = false;

        [Header("Obstruction")]
        public float ObstructionCheckRadius = 0.2f;
        public LayerMask ObstructionLayers = -1;
        public float ObstructionSharpness = 10000f;
        public List<Collider> IgnoredColliders = new List<Collider>();

        public Transform Transform { get; private set; }
        public Transform FollowTransform { get; private set; }
        private float _currentDistance;
        private Vector3 _currentFollowPosition;
        
        private RaycastHit[] _obstructions = new RaycastHit[MaxObstructions];
        private const int MaxObstructions = 32;
        private bool _distanceIsObstructed;

        void OnValidate()
        {
            DefaultDistance = Mathf.Clamp(DefaultDistance, MinDistance, MaxDistance);
        }

        void Awake()
        {
            Transform = this.transform;
            _currentDistance = DefaultDistance;
        }

        public void SetFollowTransform(Transform t)
        {
            FollowTransform = t;
            _currentFollowPosition = FollowTransform.position;
        }

        public void UpdateWithInput(float deltaTime, float zoomInput, Vector3 rotationInput)
        {
            if (FollowTransform)
            {
                // Set fixed top-down rotation
                Transform.rotation = Quaternion.Euler(90f, 0f, 0f);

                // Handle zoom input
                if (Mathf.Abs(zoomInput) > 0f)
                {
                    _currentDistance = Mathf.Clamp(_currentDistance + (zoomInput * DistanceMovementSpeed), 
                        MinDistance, MaxDistance);
                }

                // Update follow position - only X and Z
                Vector3 targetPosition = FollowTransform.position;
                _currentFollowPosition.x = Mathf.Lerp(_currentFollowPosition.x, targetPosition.x, 
                    1f - Mathf.Exp(-FollowingSharpness * deltaTime));
                _currentFollowPosition.z = Mathf.Lerp(_currentFollowPosition.z, targetPosition.z, 
                    1f - Mathf.Exp(-FollowingSharpness * deltaTime));

                // Set camera position
                Transform.position = new Vector3(
                    _currentFollowPosition.x + FollowPointFraming.x,
                    targetPosition.y + _currentDistance,  // Use height from target plus distance
                    _currentFollowPosition.z + FollowPointFraming.y
                );
            }
        }
    }