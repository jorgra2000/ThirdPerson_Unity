using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private Rigidbody rb;

    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       if (target != null) 
       {
            Vector3 direction = target.position - transform.position;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform newTarget) 
    {
        target = newTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(target != null && other.gameObject.CompareTag("Enemy")) 
        {
            other.GetComponent<Soldier>().TakeDamage(5);
            Destroy(gameObject);
        }
    }
}
