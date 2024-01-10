using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMovement : MonoBehaviour
{
    private Camera m_Camera;
    private Vector3 move;

    CharacterController controller;
    [SerializeField] float speed = 12f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField]Transform groundCheck;
    [SerializeField]LayerMask groundMask;
    [SerializeField] AudioClip attacked;
    [SerializeField] Transform Character;
    public static Vector3 CharacterVector;



    [Header("HeadBob")]
    [SerializeField] float walkBobSpeed = 14f;
    [SerializeField] float walkBobAmount = 0.05f;
    [SerializeField] float defaultYPos = 0;
    [SerializeField] float timer ;
    Vector3 Velocity;
    [SerializeField] bool isGrounded;
    bool isWalking;
    float groundDistance = 0.4f;


    
    
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        m_Camera  = Camera.main;
        defaultYPos = m_Camera.transform.localPosition.y;
    }    
    void Update()
    {
        CharacterVector = Character.position;
        
        HandleHeadBob();
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && Velocity.y < 0)
        {
            Velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        move = transform.right * x   + transform.forward * z ;
        controller.Move(move * speed * Time.deltaTime);
        Velocity.y += gravity *Time.deltaTime;
        controller.Move(Velocity * Time.deltaTime);

    }




    void HandleHeadBob()  
    {

        if(Mathf.Abs(move.x) > 0.1f || Mathf.Abs(move.z) > 0.1f)
        {
            timer += Time.deltaTime * walkBobSpeed;
            m_Camera.transform.localPosition = new Vector3(m_Camera.transform.localPosition.x ,
                defaultYPos + Mathf.Sin(timer) * walkBobAmount,
                m_Camera.transform.localPosition.z);
        }

    }
        
    
}
