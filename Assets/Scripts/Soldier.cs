using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

public class Soldier : MonoBehaviour
{
    [SerializeField] private float lifePoints;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Vector3 attackRangeDimensions;
    [SerializeField] private LayerMask layerMaskAttack;

    const string IDLE = "Idle";
    const string RUN = "Run";

    private NavMeshAgent agent;
    private Animator animator;

    private Vector3 destination;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        destination = GameObject.FindGameObjectWithTag("CastleP1").transform.position;

        agent.destination = destination;
    }

    private void Update()
    {
        
        SetAnimation();
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

    public void TakeDamage(float damage) 
    {
        lifePoints -= damage;
        if(lifePoints <= 0) 
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CastleP1")) 
        {
            other.GetComponent<Castle>().LooseHealth(10);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, attackRangeDimensions * 2);
    }
}
