using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    const string IDLE = "Idle";
    const string RUN = "Run";

    private PlayerActions input;

    private NavMeshAgent agent;
    private Animator animator;
    private IInteractable currentInteractable = null;
    private Transform targetToAttack;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private LayerMask clickableLayers;
    [SerializeField] private float remainingDistance;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private LayerMask layerMaskAttack;
    [SerializeField] private Vector3 attackRangeDimensions;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float attackSpeed;

    private float lookRotationSpeed = 8f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        input = new PlayerActions();
        AssignInputs();
        StartCoroutine(AttackProcess());
    }

    private void Update()
    {
        FaceTarget();
        SetAnimation();
        targetToAttack = DetectNearTarget();
    }

    private Transform DetectNearTarget()
    {
        List<GameObject> detectedObjects = new List<GameObject>();
        Vector3 center = transform.position;

        Collider[] colliders = Physics.OverlapBox(center, attackRangeDimensions, Quaternion.identity, layerMaskAttack);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                detectedObjects.Add(col.gameObject);
            }
        }

        if(detectedObjects.Count != 0) 
        {
            return detectedObjects[0].transform;
        }

        return null;
    }

    IEnumerator AttackProcess() 
    {
        while (true) 
        {
            if (targetToAttack != null)
            {
                animator.SetTrigger("attack");
            }
            yield return new WaitForSeconds(attackSpeed);
        }
    }

    private void Attack() 
    {
        Projectile bullet = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        bullet.SetTarget(targetToAttack);
    }

    void AssignInputs() 
    {
        input.Main.Move.performed += ctx => ClickToMove();
        input.Main.Interact.performed += ctx => Interact();
    }

    public void Interact() 
    {
        if (currentInteractable != null) 
        {
            currentInteractable.Interact();
        }
    }

    void ClickToMove() 
    {
        RaycastHit hit;

        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers)) 
        {
            agent.destination = hit.point;
        }
    }

    void FaceTarget() 
    {
        if (agent.velocity.magnitude > 0.1f && remainingDistance > agent.stoppingDistance)
        {
            Vector3 direction = (agent.destination - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
        }
    }

    void SetAnimation() 
    {
        if (agent.velocity == Vector3.zero)
        {
            animator.SetBool("moving", false);
        }
        else 
        {
            animator.SetBool("moving", true);
        }
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Coin"))
        {
            gameManager.AddCoin();
            Destroy(other.gameObject);
        }

        if (other.TryGetComponent<Building>(out Building building))
        {
            building.ShowCost();
            currentInteractable = building;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.TryGetComponent<Building>(out Building building))
        {
            building.HideCost();
            currentInteractable = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, attackRangeDimensions * 2);
    }
}
