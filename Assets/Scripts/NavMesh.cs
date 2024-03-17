using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class NavMesh : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float fightSpeed = 5f;
    public List<GameObject> trashes = new List<GameObject>();
    public GameObject player;
    
    private bool isWandering = false;
    private bool isWalking = false;
    private bool isGarbageOnTheGround = false;
    public bool isFighting = false;
    public bool isAttacking = false;
    
    private float _minX = 69, _maxX = 123, _minZ = 168, _maxZ = 220;
    private int _trashIndex = 0;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Outline _outline;

    private void Start()
    {
        _outline = GetComponent<Outline>();
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = moveSpeed;
        GetRandomTrashActive();
        if (!isGarbageOnTheGround)
        {
            StartCoroutine(ThrowGarbage());
        }
    }    
    
    void Update()
    {
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
        if (isWandering == false && isFighting == false)
        {
            StartCoroutine(Wander());
        }
        if (isWalking && !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.1f && isFighting == false)
        {
            StartCoroutine(WaitAtDestination());
        }
        if(isFighting && isAttacking == false)
        {
            CheckIfPlayerIsClose();
        }
        else
        {
            _navMeshAgent.stoppingDistance = 0;
        }
    }

    public void FightPlayer()
    {
        isFighting = true;
        _animator.SetBool("IsFighting", true);
        _navMeshAgent.speed = fightSpeed;
        _navMeshAgent.stoppingDistance = 3;
    }
    
    private void CheckIfPlayerIsClose()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 3f && !isAttacking)
        {
            StartCoroutine(AttackPlayer());
        }
        else
        {
            _navMeshAgent.SetDestination(player.transform.position);
        }
    }
    
    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1f);
        _animator.SetBool("Attack",true);
        yield return new WaitForSeconds(0.2f);
        _animator.SetBool("Attack", false);
        isAttacking = false;
    }

    private void GetRandomTrashActive()
    {
        _trashIndex = Random.Range(0, trashes.Count);
        trashes[_trashIndex].SetActive(true);
    }
    
    private void SetNoParentTrash()
    {
        trashes[_trashIndex].GetComponent<Rigidbody>().isKinematic = true;
        trashes[_trashIndex].GetComponent<Rigidbody>().useGravity = false;
        trashes[_trashIndex].GetComponent<BoxCollider>().isTrigger = false;
        var transformPosition = trashes[_trashIndex].transform.position;
        transformPosition.y = 0.5f;
        trashes[_trashIndex].transform.parent = null;
        isGarbageOnTheGround = true;
        _outline.enabled = true;
    }
    
    private Vector3 RandomPoint()
    {
        float x = Random.Range(_minX, _maxX);
        float z = Random.Range(_minZ, _maxZ);
        Vector3 randomPoint = new Vector3(x, 0, z);
        return randomPoint;
    }

    IEnumerator Wander()
    {
        int walkWait = Random.Range(1, 5);
        int walkTime = Random.Range(1, 6);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        _navMeshAgent.SetDestination(RandomPoint());
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(walkWait);

        isWandering = false;
    }
    
    IEnumerator ThrowGarbage()
    {
        var garbageTime = Random.Range(10, 30);
        yield return new WaitForSeconds(garbageTime);
        isGarbageOnTheGround = true;
        SetNoParentTrash();
    }
    
    IEnumerator WaitAtDestination()
    {
        isWalking = false;
    
        int waitTime = Random.Range(3, 6);
        yield return new WaitForSeconds(waitTime);
    
        isWandering = false;
    }
}

    
