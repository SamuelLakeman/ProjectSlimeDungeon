using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dGCode : MonoBehaviour
{
    
    [Header("Room Prefabs")]
    public Room spawn;
    public Room[] bottomConnectionRooms;
    public Room[] topConnectionRooms;
    public Room[] rightConnectionRooms;
    public Room[] leftConnectionRooms;

    [Header("Dungeon Generation Data")]
    public List<SpawnPoint> openSpawns = new List<SpawnPoint>();
    public List<Room> rooms= new List<Room>();
    private Room startRoom;
    private int currentLayer = 0;
    private bool closeOpenDoorways; 
    public int mapMinX,mapMaxX,mapMinZ,mapMaxZ;
    
    [Header("Dungeon Information")]
    public int seed;
    public bool generationComplete;
    
    public void Start()
    {
        openSpawns = new List<SpawnPoint>();
        seed = Random.Range(0,int.MaxValue);
        StartGeneration();
    }
    
    public void StartGeneration() 
    {
        GameObject s = Instantiate(spawn.gameObject);
        startRoom = s.GetComponent<Room>();
        for(int i = 0; i < s.GetComponent<Room>().roomSpawnPoints.Length; i++)
        {
            openSpawns.Add(s.GetComponent<Room>().roomSpawnPoints[i]);
        }

        StartCoroutine("GenerateFirstLayer");
    }

    public IEnumerator GenerateFirstLayer()
    {
        while (openSpawns.Count > 0)
        {
            SpawnPoint nextSpawn = openSpawns[Random.Range(0, openSpawns.Count)];
            Vector3 spawnPos = nextSpawn.gameObject.transform.position;
            if (spawnPos.x >= mapMaxX && spawnPos.x <= mapMinX || spawnPos.z >= mapMaxZ && spawnPos.z <= mapMinZ)
            {
                openSpawns.Remove(nextSpawn);
                Debug.Log("end1");
                yield return new WaitForSeconds(1);
            }
            else
            {
                Room selectedRoom;
                // Room extensionRoom = nextSpawn.parent.GetComponent<Room>();
                //selecting what room to make and instantiating it
                if (nextSpawn.openingDirection == 1)
                {
                    selectedRoom = topConnectionRooms[Random.Range(0, topConnectionRooms.Length)];
                    GameObject newRoom = Instantiate(selectedRoom.gameObject, nextSpawn.transform.position, selectedRoom.gameObject.transform.rotation);
                    nextSpawn.spawned = true;
                    for (int i = 0; i < newRoom.GetComponent<Room>().roomSpawnPoints.Length; i++)
                    {
                        if (newRoom.GetComponent<Room>().roomSpawnPoints[i].openingDirection != 2)
                        {
                            openSpawns.Add(newRoom.GetComponent<Room>().roomSpawnPoints[i]);
                        }
                    }
                    openSpawns.Remove(nextSpawn);
                }
                else if (nextSpawn.openingDirection == 2)
                {
                    selectedRoom = bottomConnectionRooms[Random.Range(0, bottomConnectionRooms.Length)];
                    GameObject newRoom = Instantiate(selectedRoom.gameObject, nextSpawn.transform.position, selectedRoom.gameObject.transform.rotation);
                    nextSpawn.spawned = true;
                    for (int i = 0; i < newRoom.GetComponent<Room>().roomSpawnPoints.Length; i++)
                    {
                        if (newRoom.GetComponent<Room>().roomSpawnPoints[i].openingDirection != 1)
                        {
                            openSpawns.Add(newRoom.GetComponent<Room>().roomSpawnPoints[i]);
                        }
                    }
                    openSpawns.Remove(nextSpawn);
                }
                else if (nextSpawn.openingDirection == 3)
                {
                    selectedRoom = leftConnectionRooms[Random.Range(0, leftConnectionRooms.Length)];
                    GameObject newRoom = Instantiate(selectedRoom.gameObject, nextSpawn.transform.position, selectedRoom.gameObject.transform.rotation);
                    nextSpawn.spawned = true;
                    for (int i = 0; i < newRoom.GetComponent<Room>().roomSpawnPoints.Length; i++)
                    {
                        if (newRoom.GetComponent<Room>().roomSpawnPoints[i].openingDirection != 4)
                        {
                            openSpawns.Add(newRoom.GetComponent<Room>().roomSpawnPoints[i]);
                        }
                    }
                    openSpawns.Remove(nextSpawn);
                }
                else if (nextSpawn.openingDirection == 4)
                {
                    selectedRoom = rightConnectionRooms[Random.Range(0, rightConnectionRooms.Length)];
                    GameObject newRoom = Instantiate(selectedRoom.gameObject, nextSpawn.transform.position, selectedRoom.gameObject.transform.rotation);
                    nextSpawn.spawned = true;
                    for (int i = 0; i < newRoom.GetComponent<Room>().roomSpawnPoints.Length; i++)
                    {
                        if (newRoom.GetComponent<Room>().roomSpawnPoints[i].openingDirection != 3)
                        {
                            openSpawns.Add(newRoom.GetComponent<Room>().roomSpawnPoints[i]);
                        }
                    }
                    openSpawns.Remove(nextSpawn);
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
}