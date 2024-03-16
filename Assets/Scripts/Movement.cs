using DG.Tweening;
using UnityEngine;

public class PlayerMovementAdvanced : MonoBehaviour
{

    public float moveSpeed = 8f;
    public float sprintSpeed = 10f;
    public float rotationSpeed = 3f;
    
    public bool isSprinting;
    
    private Rigidbody rb;
    private Animator anim;
    public Camera camera;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>(); 
    }
    
    void Update()
    {
        Move();
    }
    
    private void Move()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
            camera.DOFieldOfView(80, 0.5f);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            camera.DOFieldOfView(60, 0.5f);
            isSprinting = false;
        }
        
        var movement = GetMovement();
        movement = Vector3.ClampMagnitude(movement, 1);
        
        Quaternion cameraRotation = Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, 0);
        movement = cameraRotation * movement;
    
        anim.SetFloat("Speed", movement.magnitude);
        Vector3 newPosition = transform.position + movement * Time.deltaTime * (isSprinting ? sprintSpeed : moveSpeed);
    
        newPosition.y = Terrain.activeTerrain.SampleHeight(newPosition);
        transform.position = newPosition;
        Rotate(movement);
    }
    
    private void Rotate(Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }
    
    private Vector3 GetMovement()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var movement = new Vector3(horizontal, 0, vertical);
    
        return movement;
    }
    // //her yap�lacak hareket i�lemi i�in de�i�kenler tan�mland�.
    //
    // public string[] randomAttacks;
    //
    // [Header("Movement")]
    // private float moveSpeed = 3.5f;
    // public float sprintSpeed = 5f;
    // public float walkSpeed;
    // public float rotateSpeed = 5;
    //
    // public float groundDrag;
    //
    // [Header("Inputs")]
    // public float vertical; 
    // public float horizontal;
    // public float moveAmount;
    // public Vector3 moveDir;
    //
    // [Header("States")]
    // public bool onGround;
    // public bool sprint;
    // [HideInInspector]
    // public bool jump;
    // [HideInInspector]
    // public bool normalAttack;
    // [HideInInspector]
    // public bool comboAttack;
    // public bool canMove;
    // [HideInInspector]
    // public bool roll;
    //
    // [Header("Jumping")]
    // public float jumpForce;
    // public float jumpCooldown;
    // public float airMultiplier;
    // bool readyToJump;
    //
    // [Header("Keybinds")]
    // public KeyCode jumpKey = KeyCode.Space;
    // public KeyCode sprintKey = KeyCode.LeftShift;
    //
    // [Header("Ground Check")]
    // public float playerHeight;
    // public LayerMask whatIsGround;
    // bool grounded;
    //
    //
    // public Transform orientation;
    //
    // float horizontalInput;
    // float verticalInput;
    //
    // Vector3 moveDirection;
    // float fixedDelta;
    // float delta;
    // Animator anim;
    // [HideInInspector]
    // Rigidbody rb;
    //
    // public MovementState state;
    // public enum MovementState
    // {
    //     walking,
    //     sprinting,
    //     air
    // }
    //
    // private void Start()
    // {     //rigid body al�nd�
    //     rb = GetComponent<Rigidbody>();
    //     rb.freezeRotation = true;
    //      //z�plama durumuna m�saitlik aktive edildi
    //     readyToJump = true;
    //
    // }
    //
    // private void Update()
    // {
    //     // grounded olup olmama durum kontrol�
    //     grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
    //
    //     //girilecek inputa g�re h�z ve durum kontrolleri sa�land�
    //     MyInput();
    //     SpeedControl();
    //     StateHandler();
    //
    //     //  grounded durumuna gelinceye kadar drag 0 a �ekildi
    //     if (grounded)
    //         rb.drag = groundDrag;
    //     else
    //         rb.drag = 0;
    // }
    //
    // private void FixedUpdate()
    // {
    //     fixedDelta = Time.fixedDeltaTime;
    //     MovePlayer();
    // }
    //
    // void GetInput()
    // {
    //     vertical = Input.GetAxis("Vertical");
    //     horizontal = Input.GetAxis("Horizontal");
    //     sprint = Input.GetButton("SprintInput");
    //     jump = Input.GetButtonDown("Jump");
    //     normalAttack = Input.GetButtonDown("Fire1");
    //     comboAttack = Input.GetButtonDown("Fire2");
    //     roll = Input.GetButtonDown("Fire3");
    // }
    //
    // private void MyInput()
    // {
    //     horizontalInput = Input.GetAxisRaw("Horizontal");
    //     verticalInput = Input.GetAxisRaw("Vertical");
    //
    //     // z�plama durumlar�n�n uygunlupuna g�re ger�ekle�mesi sa�land�
    //     if (Input.GetKey(jumpKey) && readyToJump && grounded)
    //     {
    //         readyToJump = false;
    //
    //         Jump();
    //
    //         Invoke(nameof(ResetJump), jumpCooldown);
    //     }
    //
    // }
    //
    // void UpdateStates()
    //     {
    //         canMove = anim.GetBool("canMove"); 
    //
    //         if (jump)
    //         {
    //             if (onGround && canMove)
    //             {
    //                 anim.CrossFade("falling", 0.1f);
    //                 rb.AddForce(0, jumpForce, 0);    
    //             }            
    //         }   
    //
    //         if(comboAttack)
    //         {
    //             if(onGround)
    //             {
    //                 anim.SetTrigger("combo");
    //                 
    //             }
    //         }
    //
    //         if(roll && onGround)
    //         {                
    //             anim.SetTrigger("roll");
    //         }            
    //         
    //         float targetSpeed = moveSpeed;
    //          
    //         if (sprint)
    //         {
    //             targetSpeed = sprintSpeed;            
    //         }
    //         //
    //         // Vector3 v = vertical * camManager.transform.forward;
    //         // Vector3 h = horizontal * camManager.transform.right;            
    //         
    //         // moveDir = ((v + h).normalized) * (targetSpeed * moveAmount);            
    //         
    //         moveDir.y = rb.velocity.y;            
    //         
    //         float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
    //         moveAmount = Mathf.Clamp01(m);
    //         
    //         if (normalAttack && canMove)
    //         {
    //             string targetAnim;
    //
    //             int r = Random.Range(0, randomAttacks.Length);
    //             targetAnim = randomAttacks[r];
    //
    //             anim.CrossFade(targetAnim, 0.1f);       
    //
    //             if (!onGround)
    //             {
    //                 anim.CrossFade("JumpAttack", 0.1f);
    //             }
    //
    //             normalAttack = false;
    //         } 
    //                    
    //     }
    //
    // private void StateHandler()
    // {       //stateler belirlendi (ba�lang��ta biri se�ilmek �zere)
    //     // Ko�ma durumu
    //     if (grounded && Input.GetKey(sprintKey))
    //     {
    //         state = MovementState.sprinting;
    //         moveSpeed = sprintSpeed;
    //     }
    //
    //     // y�r�me durumu
    //     else if (grounded)
    //     {
    //         state = MovementState.walking;
    //         moveSpeed = walkSpeed;
    //     }
    //
    //     // z�plama durumu
    //     else
    //     {
    //         state = MovementState.air;
    //     }
    // }
    //
    // private void MovePlayer()
    // {
    //     // Hareket edilecek y�n hesaplamas�
    //     moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    //
    //     // yer hareketi
    //      if (grounded)
    //         rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    //
    //     // hava hareketi
    //     else if (!grounded)
    //         rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    // }
    //
    // private void SpeedControl()
    // {
    //
    //     // ground veya hava da h�z limitlenmesi sa�land�
    //         Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    //
    //         // limit velocity if needed
    //         if (flatVel.magnitude > moveSpeed)
    //         {
    //             Vector3 limitedVel = flatVel.normalized * moveSpeed;
    //             rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
    //         }
    //     
    // }
    //
    // private void Jump()
    // {
    //     // z�plama ger�ekle�tirildi
    //     rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    //
    //     rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    // }
    // private void ResetJump()
    // {  //z�plama sonras� durum resetlendi
    //     readyToJump = true;
    //
    // }
}