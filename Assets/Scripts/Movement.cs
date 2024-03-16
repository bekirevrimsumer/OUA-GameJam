using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementAdvanced : MonoBehaviour
{
    //her yapýlacak hareket iþlemi için deðiþkenler tanýmlandý.


    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        air
    }

    private void Start()
    {     //rigid body alýndý
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
         //zýplama durumuna müsaitlik aktive edildi
        readyToJump = true;

    }

    private void Update()
    {
        // grounded olup olmama durum kontrolü
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        //girilecek inputa göre hýz ve durum kontrolleri saðlandý
        MyInput();
        SpeedControl();
        StateHandler();

        //  grounded durumuna gelinceye kadar drag 0 a çekildi
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // zýplama durumlarýnýn uygunlupuna göre gerçekleþmesi saðlandý
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }

    private void StateHandler()
    {       //stateler belirlendi (baþlangýçta biri seçilmek üzere)
        // Koþma durumu
        if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // yürüme durumu
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        // zýplama durumu
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        // Hareket edilecek yön hesaplamasý
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // yer hareketi
         if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // hava hareketi
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {

        // ground veya hava da hýz limitlenmesi saðlandý
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        
    }

    private void Jump()
    {
        // zýplama gerçekleþtirildi
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {  //zýplama sonrasý durum resetlendi
        readyToJump = true;

    }
}