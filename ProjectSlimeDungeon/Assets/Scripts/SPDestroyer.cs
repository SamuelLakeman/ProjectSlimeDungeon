using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPDestroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            GameObject DG = GameObject.FindObjectOfType<dGCode>().gameObject;
            DG.GetComponent<dGCode>().openSpawns.Remove(other.GetComponent<SpawnPoint>());
        }
    }
}
