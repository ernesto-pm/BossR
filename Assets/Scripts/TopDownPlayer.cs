using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;

public class TopDownPlayer : MonoBehaviour
    {
        public TopDownCharacterController Character;
        public TopDownCamera CharacterCamera;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        private Camera _mainCamera;

        private void Start()
        {
            // We don't need to lock cursor for top-down view
            Cursor.lockState = CursorLockMode.None;
            _mainCamera = Camera.main;

            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
        }

        private void Update()
        {
            HandleCharacterInput();
        }

        private void LateUpdate()
        {
            // Handle camera updates
            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            // Only handle zoom input for top-down camera
            float scrollInput = -Input.GetAxis(MouseScrollInput);
            
            // Apply inputs to the camera (passing zero for rotation since we maintain fixed top-down view)
            CharacterCamera.UpdateWithInput(Time.deltaTime, scrollInput, Vector3.zero);
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Get mouse position in world space
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = _mainCamera.WorldToScreenPoint(Character.transform.position).z;
            Vector3 worldPos = _mainCamera.ScreenToWorldPoint(mousePos);

            // Calculate direction from character to mouse on the ground plane
            Vector3 direction = worldPos - Character.transform.position;
            direction.y = 0f; // Ensure rotation only happens on the ground plane

            // Calculate rotation to face mouse position
            Quaternion targetRotation = direction != Vector3.zero 
                ? Quaternion.LookRotation(direction) 
                : Character.transform.rotation;

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
            characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
            characterInputs.CameraRotation = targetRotation;
            characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);
            characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
            characterInputs.CrouchUp = Input.GetKeyUp(KeyCode.C);
            characterInputs.BlinkDown = Input.GetKeyDown(KeyCode.LeftShift);

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }
    }