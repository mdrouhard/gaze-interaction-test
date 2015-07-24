using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (Rigidbody))]
    [RequireComponent(typeof (CapsuleCollider))]
    public class OVRRBController : MonoBehaviour
    {
        [Serializable]
        public class MovementSettings
        {
            public float ForwardSpeed = 8.0f;   // Speed when moving forward
            public float BackwardSpeed = 4.0f;  // Speed when moving backwards
            public float StrafeSpeed = 4.0f;    // Speed when moving sideways
            public float RunMultiplier = 2.0f;   // Multiplier for speed when moving quickly
	        public KeyCode RunKey = KeyCode.LeftShift;
            public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
            [HideInInspector] public float CurrentTargetSpeed = 8f;

#if !MOBILE_INPUT
            private bool m_Running;
#endif

            public void UpdateDesiredTargetSpeed(Vector2 input)
            {
	            if (input == Vector2.zero) return;
				if (input.x > 0 || input.x < 0)
				{
					//strafe
					CurrentTargetSpeed = StrafeSpeed;
				}
				if (input.y < 0)
				{
					//backwards
					CurrentTargetSpeed = BackwardSpeed;
				}
				if (input.y > 0)
				{
					//forwards
					//handled last as if strafing and moving forward at the same time forwards speed should take precedence
					CurrentTargetSpeed = ForwardSpeed;
				}


#if !MOBILE_INPUT
	            if (Input.GetKey(RunKey))
	            {
		            CurrentTargetSpeed *= RunMultiplier;
		            m_Running = true;
	            }
	            else
	            {
		            m_Running = false;
	            }
#endif
            }

#if !MOBILE_INPUT
            public bool Running
            {
                get { return m_Running; }
            }
#endif
        }


        [Serializable]
        public class AdvancedSettings
        {
            public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
        }

		public OVRCameraRig ovrCam;
		public GameObject reticle;

        public MovementSettings movementSettings = new MovementSettings();
        public MouseLook mouseLook = new MouseLook();
        public AdvancedSettings advancedSettings = new AdvancedSettings();


        private Rigidbody m_RigidBody;
        private CapsuleCollider m_Capsule;
        private float m_YRotation;
        private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;


        public Vector3 Velocity
        {
            get { return m_RigidBody.velocity; }
        }

        public bool Running
        {
            get
            {
 #if !MOBILE_INPUT
				return movementSettings.Running;
#else
	            return false;
#endif
            }
        }


        private void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
			mouseLook.Init (transform, ovrCam.centerEyeAnchor);
        }


        private void Update()
        {
			//TODO: use cross-platform input
			// toggle visibility of reticle with keypress
			if (Input.GetKeyDown ("t")) {
				reticle.SetActive(!reticle.activeSelf);
			}
            RotateView();
        }


        private void FixedUpdate()
        {
            Vector2 input = GetInput();
			bool isBraking = IsBraking();

			// if braking, only brake
			if (isBraking) {
				m_RigidBody.velocity = m_RigidBody.velocity * 0.95f;
				m_RigidBody.angularVelocity = m_RigidBody.angularVelocity * 0.95f;
			// else if a move is desired, apply appropriate force
			} else if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) /*&& (advancedSettings.airControl || m_IsGrounded)*/)
            {
                // always move along the camera forward as it is the direction that it being aimed at
				Vector3 desiredMove = ovrCam.centerEyeAnchor.forward*input.y + ovrCam.centerEyeAnchor.right*input.x;

                desiredMove.x = desiredMove.x*movementSettings.CurrentTargetSpeed;
                desiredMove.z = desiredMove.z*movementSettings.CurrentTargetSpeed;
                desiredMove.y = desiredMove.y*movementSettings.CurrentTargetSpeed;

                if (m_RigidBody.velocity.sqrMagnitude <
                    (movementSettings.CurrentTargetSpeed*movementSettings.CurrentTargetSpeed))
                {
                    m_RigidBody.AddForce(desiredMove, ForceMode.Impulse);
                }
            }

			// If input & previous velocity are nonexistent or negligible, sleep
			if (Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
			{
				m_RigidBody.Sleep();
			}
        }


		private bool IsBraking() 
		{
			float brakeAmt = CrossPlatformInputManager.GetAxis ("Brake");

			bool braking = (brakeAmt > 0) ? true : false;
			return braking;
		}

        private Vector2 GetInput()
        {
            
            Vector2 input = new Vector2
                {
                    x = CrossPlatformInputManager.GetAxis("Horizontal"),
                    y = CrossPlatformInputManager.GetAxis("Vertical")
                };

			movementSettings.UpdateDesiredTargetSpeed(input);
            return input;
        }


        private void RotateView()
        {
            //avoids the mouse looking if the game is effectively paused
            if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

            // get the rotation before it's changed
            float oldYRotation = transform.eulerAngles.y;

			mouseLook.LookRotation (transform, ovrCam.centerEyeAnchor);
        }

		// Stop on collision (don't bounce)
		void OnCollisionEnter(Collision collision) {
			m_RigidBody.velocity = Vector3.zero;
		}
    }
}
