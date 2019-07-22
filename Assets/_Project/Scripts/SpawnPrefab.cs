using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab = null;
    
    void Start()
    {
        InstantiatePrefab();
    }

    private void InstantiatePrefab()
    {
        if (prefab != null)
        {
            Instantiate(prefab, transform);
        }
    }
}
