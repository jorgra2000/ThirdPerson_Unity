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

    [SerializeField] private GameManager gameManager;
    [SerializeField] private LayerMask clickableLayers;
    [SerializeField] private float remainingDistance;

    private float lookRotationSpeed = 8f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        input = new PlayerActions();
        AssignInputs();
    }

    private void Update()
    {
        FaceTarget();
        SetAnimation();
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
            animator.Play(IDLE);
        }
        else 
        {
            animator.Play(RUN);
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
}
