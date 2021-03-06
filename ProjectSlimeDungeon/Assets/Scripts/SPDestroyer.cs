﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPDestroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            other.GetComponent<SpawnPoint>().spawned = true;
        }
    }
}
