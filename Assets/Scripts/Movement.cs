using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public bool isCollectingTrash;

    private Rigidbody rb;
    private Animator anim;
    public Camera camera;
    private PlayerManager playerManager;
    public TextMeshProUGUI removeTrashText;
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
        playerManager = GetComponent<PlayerManager>();
        isGrounded = true;

        InitializeAttackAnimations();
    }

    void Update()
    {
        if (isCollectingTrash == false)
        {
            AttackInput();
            UltimateInput();
            Move();
        }
        
        if (playerManager.stamina == 0)
        {
            isSprinting = false;
            moveSpeed = walkSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Trash")
        {
            removeTrashText.gameObject.SetActive(true);
        }
        
        if (other.gameObject.tag == "Enemy")
        {
            var enemy = other.gameObject.GetComponent<EnemyManagement>();
            var navMesh = other.gameObject.GetComponent<NavMesh>();
            if (navMesh.isFighting == true)
            {
                enemy.GetDamage(10);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Trash")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isCollectingTrash = true;
                moveSpeed = 0;
                anim.SetTrigger("CollectTrash");
                removeTrashText.gameObject.SetActive(false);
                other.gameObject.GetComponent<Trash>().navMesh.FightPlayer();
                Destroy(other.gameObject, 1f);
                Invoke("ResetCollectingTrash", 1.5f);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Trash")
        {
            removeTrashText.gameObject.SetActive(false);
        }
    }

    // private void OnCollisionEnter(Collider other)
    // {
    //     if(other.gameObject.tag == "Trash")
    //     {
    //         removeTrashText.gameObject.SetActive(true);
    //     }
    //     
    //     if (other.gameObject.tag == "Enemy")
    //     {
    //         var enemy = other.gameObject.GetComponent<EnemyManagement>();
    //         enemy.GetDamage(10);
    //     }
    // }

    // private void OnCollisionStay(Collider other)
    // {
    //     if(other.gameObject.tag == "Trash")
    //     {
    //         if (Input.GetKeyDown(KeyCode.F))
    //         {
    //             isCollectingTrash = true;
    //             moveSpeed = 0;
    //             anim.SetTrigger("CollectTrash");
    //             removeTrashText.gameObject.SetActive(false);
    //             other.gameObject.GetComponent<Trash>().navMesh.FightPlayer();
    //             Destroy(other.gameObject, 1f);
    //             Invoke("ResetCollectingTrash", 1.5f);
    //         }
    //     }
    // }
    //
    // private void OnCollisionExit(Collider other)
    // {
    //     if(other.gameObject.tag == "Trash")
    //     {
    //         removeTrashText.gameObject.SetActive(false);
    //     }
    // }

    private void InitializeAttackAnimations()
    {
        attackAnimations.Add("Attack1");
        attackAnimations.Add("Attack2");
    }

    private void AttackInput()
    {
        if(Input.GetMouseButtonDown(0) && isCollectingTrash == false)
        {
            camera.gameObject.transform.DOShakeRotation(0.2f, 0.3f, 10, 90, false);
        }
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
    }

    private void UltimateInput()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isUlti && isCollectingTrash == false)
        {
            camera.gameObject.transform.DOShakeRotation(2.3f, 0.5f, 10, 90, false);
            StartUltimate();
        }
    }

    private void StartUltimate()
    {
        isGrounded = false;
        moveSpeed = 0;
        anim.SetBool("IsUlti", true);
        Invoke("ResetUlti", 2.4f);
    }

    private void ResetUlti()
    {
        anim.SetBool("IsUlti", false);
        isGrounded = true;
        isUlti = false;
    }
    
    private void ResetCollectingTrash()
    {
        isCollectingTrash = false;
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
        if (isGrounded && Input.GetKey(sprintKey) && playerManager.stamina != 0)
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
        playerManager.UseStamina(playerManager.maxStamina/1000);
    }

    private void StartWalking()
    {
        state = MovementState.walking;
        moveSpeed = walkSpeed;
        camera.DOFieldOfView(60, 0.5f);
        isSprinting = false;
    }
}