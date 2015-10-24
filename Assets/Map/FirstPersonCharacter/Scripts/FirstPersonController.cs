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
        
                // the sound played when character touches back on ground.

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


        private Vector3 prevVec;
        private Vector3 heightVec;
        private Vector3 nextVec;


        private bool flag= false;

        private bool firstTime = true;

        private GameManager gm;
        private bool f1;
        private bool f2;
        private Tile prevTile;
        private Tile nextTile;
        private int tileIndex;
        private float elapsedTime;
        private int nextIndex = 0;
        private float lerpTime = 1f;
        private float currentLerpTime;

        private Map map;
        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
          
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();

            GameObject g = GameObject.Find("GameManager");
            gm = g.GetComponent<GameManager>();
            map = GetComponent<Map>();
            
            prevVec = new Vector3(0, 1, 0);

            // mein Bereich 

            jumpVectors.Add(Vector3.forward);
            jumpVectors.Add(Vector3.left);
            jumpVectors.Add(Vector3.right);
            //firstTime = true;
            jumpIndex = 0;
            elapsedTime = 0;
            tileIndex = 0;
            
            //prevVec = transform.position;
            //transform.position = Vector3.Lerp(transform.position, new Vector3(0,0,0), Time.deltaTime);
        }


        // Update is called once per frame
        private void Update()
        {
            elapsedTime += Time.deltaTime;
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            if (elapsedTime > 5)
            {
                print("hi");
                elapsedTime = 0;
            }
            if (firstTime)
            {
                
                transform.position -= (new Vector3(0, 1, 0) * Time.deltaTime);
                //m_Camera.transform.position = Vector3.Lerp(m_Camera.transform.position, new Vector3(0, 0, 0), Time.deltaTime * 2);
                //m_CollisionFlags = m_CharacterController.Move(new Vector3(0, -1, 0) * Time.deltaTime);
                prevTile = gm.Map.tiles[tileIndex];
                nextTile = gm.Map.tiles[tileIndex + 1];
                heightVec = prevTile.transform.position + ((nextTile.transform.position - prevTile.transform.position) / 2) + new Vector3(0, 2, 0);
                nextIndex++;
            }
            if (transform.position.y < 0)
            {
                
                firstTime = false;
            }
            if (!firstTime)
            {
                float t = currentLerpTime / lerpTime;
                t = Mathf.Sin(t * Mathf.PI * 0.5f);
                if (nextIndex == 0)
                {
                    print("1");
                    transform.position = Vector3.Lerp(heightVec, prevTile.transform.position, Mathf.PingPong(Time.time, 1.0f));
                }
                else
                {
                    transform.position = Vector3.Lerp( nextTile.transform.position,heightVec, Mathf.PingPong(Time.time, 1.0f));
                    print("2");
                }
                if(elapsedTime > 2){
                    
                    
                    if(nextIndex == 0){
                        prevTile = gm.Map.tiles[tileIndex];
                        nextTile = gm.Map.tiles[tileIndex + 1];
                        tileIndex++;
                        
                        heightVec = prevTile.transform.position + ((nextTile.transform.position - prevTile.transform.position)/2) + new Vector3(0, 2, 0);
                        print("prev : x : " + prevTile.transform.position.x + " " + prevTile.transform.position.y + " " + prevTile.transform.position.z + " ");
                        print("next : x : " + nextTile.transform.position.x + " " + nextTile.transform.position.y + " " + nextTile.transform.position.z + " ");
                        print("transform : x : " + heightVec.x + " " + heightVec.y + " " + heightVec.z + " ");
                        f1 = true;
                        nextIndex++;
                    }
                    else
                    {
                        
                        nextIndex = 0;
                    }
                    elapsedTime = 0;
                    currentLerpTime = 0f;
                }
               
                /*
                if (transform.position.y < 0)
                {
                    f1 = true;
                    //prevTile = map.tiles[tileIndex];
                    //nextTile = map.tiles[tileIndex];
                    
                   
                }
                if (transform.position.y > 2)
                {
                    f1 = false;
                }
                  
                if(f1){
                transform.Translate(Vector3.up * Time.deltaTime);
                transform.Translate(Vector3.forward * Time.deltaTime);
                } 
                if(!f1){
                transform.Translate(Vector3.down * Time.deltaTime);
                transform.Translate(Vector3.forward * Time.deltaTime);
                 */
                
            }
            
            
        }


        private void FixedUpdate()
        {
           
          
        }





       


        

       

        /*
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
*/

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



            GUI.Label(new Rect(10, 25, 500, 20), "elapsedTime" + elapsedTime);
            GUI.Label(new Rect(10, 50, 500, 20), "prev : x : " + prevTile.transform.position.x + " " + prevTile.transform.position.y + " " + prevTile.transform.position.z + " ");
            GUI.Label(new Rect(10, 75, 500, 20), "isOngroud:" + m_CharacterController.isGrounded);
            GUI.Label(new Rect(10, 100, 500, 20), "difference x :" + (transform.position.x - prevTransformPositionX) + " " + "difference y: "+ (transform.position.z - prevTransformPositionZ));

            GUI.Box(new Rect(10, 10, Screen.width / 4f, 15), "");

        }
        
    }
}
