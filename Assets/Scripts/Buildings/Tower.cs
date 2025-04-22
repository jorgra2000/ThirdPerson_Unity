using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private LayerMask layerMaskAttack;
    [SerializeField] private Vector3 attackRangeDimensions;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Projectile projectilePrefab;

    private Transform targetToAttack;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AttackProcess());
    }

    // Update is called once per frame
    void Update()
    {
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

        if (detectedObjects.Count != 0)
        {
            return detectedObjects[0].transform;
        }

        return null;
    }

    IEnumerator AttackProcess()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Projectile bullet = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            bullet.SetTarget(targetToAttack);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, attackRangeDimensions * 2);
    }
}
