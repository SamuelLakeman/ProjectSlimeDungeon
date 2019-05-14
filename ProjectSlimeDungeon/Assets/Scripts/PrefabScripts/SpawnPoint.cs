using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public int openingDirection;
    //1 = Top spawn point needs Bottom connection
    //2 = Bottom spawn point need Top connection
    //3 = Left spawn point needs Right connection
    //4 = Right spawn point need Left connection
    public bool spawned = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            GameObject DG = GameObject.FindObjectOfType<dGCode>().gameObject;
            DG.GetComponent<dGCode>().openSpawns.Remove(this);
        }
    }

}