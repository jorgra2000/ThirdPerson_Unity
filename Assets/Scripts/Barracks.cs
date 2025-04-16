using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : MonoBehaviour
{
    [SerializeField] private GameObject spawnUnit;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float timeSpawn;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(timeSpawn);
            Instantiate(spawnUnit, spawnPoint.position, Quaternion.identity);
        }
    }
}
