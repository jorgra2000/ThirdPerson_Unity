using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

public class Soldier : MonoBehaviour
{
    [SerializeField] private bool isAlly;
    [SerializeField] private float lifePoints;
    [SerializeField] private GameObject coinPrefab;

    const string IDLE = "Idle";
    const string RUN = "Run";

    private NavMeshAgent agent;
    private Animator animator;

    private Transform destination;

    private void Awake()
    {
        if (isAlly) 
        {
            destination = GameObject.FindGameObjectWithTag("CastleP2").transform;
        }
        else 
        {
            destination = GameObject.FindGameObjectWithTag("CastleP1").transform;
        }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        agent.destination = destination.position;
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
            if (!isAlly) 
            {
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAlly) 
        {
            if (other.CompareTag("CastleP1")) 
            {
                other.GetComponent<Castle>().LooseHealth(10);
                Destroy(gameObject);
            }
        }
    }
}
