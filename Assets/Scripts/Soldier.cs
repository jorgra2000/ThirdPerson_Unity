using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

public class Soldier : MonoBehaviour
{
    [SerializeField] private bool isAlly;
    [SerializeField] private float lifePoints;

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
}
