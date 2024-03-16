using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerMovementAdvanced : MonoBehaviour
{
public float moveSpeed;
    public float walkSpeed = 8f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public List<string> attackAnimations = new List<string>();

    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;
    public bool isUlti;

    private Rigidbody rb;
    private Animator anim;
    public Camera camera;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        air
    }

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        isGrounded = true;

        InitializeAttackAnimations();
    }

    void Update()
    {
        AttackInput();
        UltimateInput();
        Move();
    }

    private void InitializeAttackAnimations()
    {
        attackAnimations.Add("Attack1");
        attackAnimations.Add("Attack2");
        attackAnimations.Add("Attack3");
    }

    private void AttackInput()
    {
        if (Input.GetMouseButton(0))
        {
            PerformRandomAttack();
        }
    }

    private void PerformRandomAttack()
    {
        var random = Random.Range(0, attackAnimations.Count);
        anim.SetBool(attackAnimations[random], true);
        Invoke("ResetAttack", 0.2f);
    }

    private void ResetAttack()
    {
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", false);
    }

    private void UltimateInput()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isUlti)
        {
            StartUltimate();
        }
    }

    private void StartUltimate()
    {
        isGrounded = false;
        moveSpeed = 0;
        anim.SetBool("IsUlti", true);
        Invoke("ResetUlti", 2.5f);
    }

    private void ResetUlti()
    {
        anim.SetBool("IsUlti", false);
        isGrounded = true;
        isUlti = false;
    }

    private void Move()
    {
        StateHandler();

        var movement = GetMovement();
        movement = Vector3.ClampMagnitude(movement, 1);

        Quaternion cameraRotation = Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, 0);
        movement = cameraRotation * movement;
        if (movement.magnitude == 0)
            moveSpeed = 0;

        anim.SetFloat("Speed", moveSpeed);

        Vector3 newPosition = transform.position + movement * Time.deltaTime * moveSpeed;

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

    private void StateHandler()
    {
        if (isGrounded && Input.GetKey(sprintKey))
        {
            StartSprinting();
        }
        else if (isGrounded)
        {
            StartWalking();
        }
    }

    private void StartSprinting()
    {
        state = MovementState.sprinting;
        isSprinting = true;
        camera.DOFieldOfView(80, 0.5f);
        moveSpeed = sprintSpeed;
    }

    private void StartWalking()
    {
        state = MovementState.walking;
        moveSpeed = walkSpeed;
        camera.DOFieldOfView(60, 0.5f);
        isSprinting = false;
    }
}