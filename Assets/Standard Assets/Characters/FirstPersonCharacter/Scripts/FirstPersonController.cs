using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField]
        private bool m_IsWalking;
        [SerializeField]
        private float m_WalkSpeed;
        [SerializeField]
        private float m_RunSpeed;
        [SerializeField]
        [Range(0f, 1f)]
        private float m_RunstepLenghten;
        [SerializeField]
        private float m_JumpSpeed;
        [SerializeField]
        private float m_StickToGroundForce;
        [SerializeField]
        private float m_GravityMultiplier;
        [SerializeField]
        private MouseLook m_MouseLook;
        [SerializeField]
        private bool m_UseFovKick;
        [SerializeField]
        private FOVKick m_FovKick = new FOVKick();
        [SerializeField]
        private bool m_UseHeadBob;
        [SerializeField]
        private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField]
        private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField]
        private float m_StepInterval;
        [SerializeField]
        private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField]
        private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField]
        private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;


        // mein Arbeitsbereich 
        private int jumpIndex;
        private List<Vector3> jumpVectors = new List<Vector3>();
        private float prevTransformPositionX = 0;
        private float prevTransformPositionZ = 0;
        private int[]difficulty = {0,1,2,3,4};

        private string orientationInput = "none";
        private float leftRightTilt = 0;
        private float upDownTilt = 0;
        private float inputDirection = 0;

        private float rotationSmoothening = 0.0F;
        private float rotationSensitivity = 0.0F;


        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_MouseLook.Init(transform, m_Camera.transform);


            // mein Bereich 

            jumpVectors.Add(Vector3.forward);
            jumpVectors.Add(Vector3.left);
            jumpVectors.Add(Vector3.right);

            jumpIndex = 0;

        }


        // Update is called once per frame
        private void Update()
        {
            updateCamera();
            if (Input.GetKey("up")) this.rotationSmoothening += 0.005F;
            if (Input.GetKey("down")) this.rotationSmoothening -= 0.005F;

            if (Input.GetKey("left")) this.rotationSensitivity += 0.1F;
            if (Input.GetKey("right")) this.rotationSensitivity -= 0.1F;



        }


        public void receiveInput(string input)
        {
            this.orientationInput = input;

            string[] inputParams = input.Split(' ');

            this.leftRightTilt = Convert.ToSingle(inputParams[0]);
            this.upDownTilt = Convert.ToSingle(inputParams[1]);
            this.inputDirection = Convert.ToSingle(inputParams[2]);

        }


        private void updateCamera()
        {
            

            // upDownTilt: -90 bis 90

            float tiltAroundZ = (upDownTilt) * this.rotationSensitivity;
            float tiltAroundX = (leftRightTilt) * this.rotationSensitivity;
            Quaternion target = Quaternion.Euler(tiltAroundX, tiltAroundZ, 0);
            this.m_Camera.transform.rotation = Quaternion.Slerp(this.m_Camera.transform.rotation, target, Time.deltaTime * this.rotationSmoothening);
        }

        private void FixedUpdate()
        {
            



        }





       


        

       


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }
       
        private void airAccel(){

        }

        void OnGUI()
        {
            


            GUI.Label(new Rect(10, 25, 500, 20), "Sensitivity:" + this.rotationSensitivity);
            GUI.Label(new Rect(10, 50, 500, 20), "Smooth:" + this.rotationSmoothening);
            GUI.Label(new Rect(10, 100, 500, 20), "difference x :" + (transform.position.x - prevTransformPositionX) + " " + "difference y: "+ (transform.position.z - prevTransformPositionZ));
            GUI.Label(new Rect(10, 125, 500, 20), "leftRight Tilt:" + this.leftRightTilt);
            GUI.Label(new Rect(10, 150, 500, 20), "upDown Tilt:" + this.upDownTilt);


            GUI.Box(new Rect(10, 10, Screen.width / 4f, 15), "");

        }
        
    }
}
