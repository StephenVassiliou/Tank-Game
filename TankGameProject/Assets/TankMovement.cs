using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TankMovement : MonoBehaviour
{

    public float m_Speed = 12f; // How fast the tank moves forward and backwards
    public float m_TurnSpeed = 180f; // How fast the tank turns in degrees per second
    public float m_DashSpeed = 500f;  // Running Speed
    public float m_JumpPower = 350f;

    public bool canJump = true;
    public float jumpTimer = 1;
    private float origJumpTimer;

    public bool canDash = true;
    public float dashTimer = 1;
    private float origDashTimer;

    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue; // The current value of the movement input
    private float m_TurnInputValue; // The current value of the turn input


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        origJumpTimer = jumpTimer;

        origDashTimer = dashTimer;
    }

    private void OnEnable()
    {
        // When the tank is turned on, make sure it is not kinematic
        m_Rigidbody.isKinematic = false;

        // Also reset the input values
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }

    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving
        m_Rigidbody.isKinematic = true;
    }

    // Update is called once per frame
    private void Update()
    {
        m_MovementInputValue = Input.GetAxis("Vertical");
        m_TurnInputValue = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown("space") && canJump)
        {
            m_Rigidbody.AddForce(transform.up * m_JumpPower);
            canJump = false;
        }
        if (canJump == false)
        {
            jumpTimer -= Time.deltaTime;

            if (jumpTimer < 0)
            {
                canJump = true;
                jumpTimer = origJumpTimer;
            }
        }

        if (Input.GetKey("q") && Input.GetKey("w") && canDash)
        {
            m_Rigidbody.AddForce(transform.forward * m_DashSpeed);
            canDash = false;  
        }
        else if (Input.GetKey("q") && Input.GetKey("s") && canDash)
        {
            m_Rigidbody.AddForce(-transform.forward * m_DashSpeed);
            canDash = false;
        }
        if (canDash == false)
        {
            dashTimer -= Time.deltaTime;

            if (dashTimer < 0)
            {
                canDash = true;
                dashTimer = origDashTimer;
            }
        }

    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        
        // Create a vector in the direction the tank is facing with a magnitude
        // based on the input, speed and the time between frames
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody position
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

    }

    private void Turn()
    {
        // Determine the number of degrees to be turned based on the input,
        // speed and time between frames

        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis
        Quaternion turnRotaion = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotaion);
    }

   

}
