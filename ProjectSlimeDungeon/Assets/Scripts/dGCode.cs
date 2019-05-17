using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dGCode : MonoBehaviour
{
    public int roomNumber;
    public float generationSpeed;

    [Header("Room Prefabs")]
    public Room spawn;
    public Room[] bottomConnectionRooms;
    public Room[] topConnectionRooms;
    public Room[] rightConnectionRooms;
    public Room[] leftConnectionRooms;

    [Header("Dungeon Generation Data")]
    public List<SpawnPoint> openSpawns = new List<SpawnPoint>();
    public List<SpawnPoint> SPList = new List<SpawnPoint>();
    private Room startRoom;
    private bool closeOpenDoorways;
    public int mapMinX, mapMaxX, mapMinZ, mapMaxZ;

    [Header("Dungeon Information")]
    public int seed;
    public bool generationComplete;

    public void Start()
    {
        openSpawns = new List<SpawnPoint>();
        seed = Random.Range(0, int.MaxValue);
        StartGeneration();
    }

    public void StartGeneration()
    {
        GameObject s = Instantiate(spawn.gameObject);
        startRoom = s.GetComponent<Room>();
        for (int i = 0; i < s.GetComponent<Room>().roomSpawnPoints.Length; i++)
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
            if (nextSpawn == null)
            {
                Debug.Log("we got a nuller!");
                openSpawns.Remove(nextSpawn);
                yield return new WaitForSeconds(generationSpeed);
            }
            else
            {
                if (nextSpawn.spawned == false)
                {
                    Vector3 spawnPos = nextSpawn.gameObject.transform.position;
                    if (spawnPos.x >= mapMaxX || spawnPos.x <= mapMinX || spawnPos.z >= mapMaxZ || spawnPos.z <= mapMinZ)
                    {
                        openSpawns.Remove(nextSpawn);
                        yield return new WaitForSeconds(generationSpeed);
                    }
                    else
                    {

                        Room selectedRoom;
                        // Room extensionRoom = nextSpawn.parent.GetComponent<Room>();
                        //selecting what room to make and instantiating it
                        if (nextSpawn.openingDirection == 1)
                        {
                            selectedRoom = FilterThenSelectRoom(topConnectionRooms, nextSpawn);
                            GameObject newRoom = Instantiate(selectedRoom.gameObject, nextSpawn.transform.position, selectedRoom.gameObject.transform.rotation);
                            newRoom.name += roomNumber;
                            nextSpawn.spawned = true;
                            if(nextSpawn.partner != null)
                            {
                                if (openSpawns.Contains(nextSpawn.partner))
                                {
                                    openSpawns.Remove(nextSpawn.partner);
                                }
                                nextSpawn.partner.spawned = true;
                            }
                            for (int i = 0; i < newRoom.GetComponent<Room>().roomSpawnPoints.Length; i++)
                            {
                                if (newRoom.GetComponent<Room>().roomSpawnPoints[i].openingDirection != 2)
                                {
                                    openSpawns.Add(newRoom.GetComponent<Room>().roomSpawnPoints[i]);
                                    SPList.Add(newRoom.GetComponent<Room>().roomSpawnPoints[i]);
                                }
                            }
                            openSpawns.Remove(nextSpawn);
                        }
                        else if (nextSpawn.openingDirection == 2)
                        {
                            selectedRoom = FilterThenSelectRoom(bottomConnectionRooms, nextSpawn);
                            GameObject newRoom = Instantiate(selectedRoom.gameObject, nextSpawn.transform.position, selectedRoom.gameObject.transform.rotation);
                            newRoom.name += roomNumber;
                            nextSpawn.spawned = true;
                            if (nextSpawn.partner != null)
                            {
                                if (openSpawns.Contains(nextSpawn.partner))
                                {
                                    openSpawns.Remove(nextSpawn.partner);
                                }
                                nextSpawn.partner.spawned = true;
                            }
                            for (int i = 0; i < newRoom.GetComponent<Room>().roomSpawnPoints.Length; i++)
                            {
                                if (newRoom.GetComponent<Room>().roomSpawnPoints[i].openingDirection != 1)
                                {
                                    openSpawns.Add(newRoom.GetComponent<Room>().roomSpawnPoints[i]);
                                    SPList.Add(newRoom.GetComponent<Room>().roomSpawnPoints[i]);
                                }
                            }
                            openSpawns.Remove(nextSpawn);
                        }
                        else if (nextSpawn.openingDirection == 3)
                        {
                            selectedRoom = FilterThenSelectRoom(leftConnectionRooms, nextSpawn);
                            GameObject newRoom = Instantiate(selectedRoom.gameObject, nextSpawn.transform.position, selectedRoom.gameObject.transform.rotation);
                            newRoom.name += roomNumber;
                            nextSpawn.spawned = true;
                            if (nextSpawn.partner != null)
                            {
                                if (openSpawns.Contains(nextSpawn.partner))
                                {
                                    openSpawns.Remove(nextSpawn.partner);
                                }
                                nextSpawn.partner.spawned = true;
                            }
                            for (int i = 0; i < newRoom.GetComponent<Room>().roomSpawnPoints.Length; i++)
                            {
                                if (newRoom.GetComponent<Room>().roomSpawnPoints[i].openingDirection != 4)
                                {
                                    openSpawns.Add(newRoom.GetComponent<Room>().roomSpawnPoints[i]);
                                    SPList.Add(newRoom.GetComponent<Room>().roomSpawnPoints[i]);
                                }
                            }
                            openSpawns.Remove(nextSpawn);
                        }
                        else if (nextSpawn.openingDirection == 4)
                        {
                            selectedRoom = FilterThenSelectRoom(rightConnectionRooms, nextSpawn);
                            GameObject newRoom = Instantiate(selectedRoom.gameObject, nextSpawn.transform.position, selectedRoom.gameObject.transform.rotation);
                            newRoom.name += roomNumber;
                            nextSpawn.spawned = true;
                            if (nextSpawn.partner != null)
                            {
                                if (openSpawns.Contains(nextSpawn.partner))
                                {
                                    openSpawns.Remove(nextSpawn.partner);
                                }
                                nextSpawn.partner.spawned = true;
                            }
                            for (int i = 0; i < newRoom.GetComponent<Room>().roomSpawnPoints.Length; i++)
                            {
                                if (newRoom.GetComponent<Room>().roomSpawnPoints[i].openingDirection != 3)
                                {
                                    openSpawns.Add(newRoom.GetComponent<Room>().roomSpawnPoints[i]);
                                    SPList.Add(newRoom.GetComponent<Room>().roomSpawnPoints[i]);
                                }
                            }
                            openSpawns.Remove(nextSpawn);
                        }
                    }
                    roomNumber++;
                    yield return new WaitForSeconds(generationSpeed);
                }
                yield return new WaitForSeconds(generationSpeed);
            }
        }
        for (int i = 0; i < SPList.Count; i++)
        {
            if (SPList[i].spawned == false)
            {
                if (SPList[i].openingDirection == 1)
                {
                    GameObject newRoom = Instantiate(topConnectionRooms[0].gameObject, SPList[i].transform.position, topConnectionRooms[0].gameObject.transform.rotation);
                    newRoom.name += roomNumber;
                }
                else if (SPList[i].openingDirection == 2)
                {
                    GameObject newRoom = Instantiate(bottomConnectionRooms[0].gameObject, SPList[i].transform.position, bottomConnectionRooms[0].gameObject.transform.rotation);
                    newRoom.name += roomNumber;
                }
                else if (SPList[i].openingDirection == 3)
                {
                    GameObject newRoom = Instantiate(leftConnectionRooms[0].gameObject, SPList[i].transform.position, leftConnectionRooms[0].gameObject.transform.rotation);
                    newRoom.name += roomNumber;
                }
                else if (SPList[i].openingDirection == 4)
                {
                    GameObject newRoom = Instantiate(rightConnectionRooms[0].gameObject, SPList[i].transform.position, rightConnectionRooms[0].gameObject.transform.rotation);
                    newRoom.name += roomNumber;
                }
                roomNumber++;
                SPList[i].spawned = true;
            }
        }
    }

    public Room FilterThenSelectRoom(Room[] rooms, SpawnPoint SP)
    {
        List<Room> possibleRooms = new List<Room>();
        for (int i = 0; i < rooms.Length; i++)
        {
            possibleRooms.Add(rooms[i]);
        }
        RaycastHit hitN;
        RaycastHit hitS;
        RaycastHit hitE;
        RaycastHit hitW;
        if (SP.openingDirection != 2 && Physics.Raycast(SP.transform.position, transform.forward, out hitN, 100))
        {
            if (hitN.transform.tag != "SpawnPoint")
            {
                for (int i = 0; i < rooms.Length; i++)
                {
                    bool hasTroubleSP = false;
                    for (int s = 0; s < rooms[i].roomSpawnPoints.Length; s++)
                    {
                        if (rooms[i].roomSpawnPoints[s].openingDirection == 1)
                        {
                            hasTroubleSP = true;
                        }
                    }

                    if (possibleRooms.Contains(rooms[i]) && hasTroubleSP == true)
                    {
                        possibleRooms.Remove(rooms[i]);
                    }
                }
            }
        }
        if (SP.openingDirection != 1 && Physics.Raycast(SP.transform.position, -transform.forward, out hitS, 100))
        {
            if (hitS.transform.tag != "SpawnPoint")
            {
                for (int i = 0; i < rooms.Length; i++)
                {
                    bool hasTroubleSP = false;
                    for (int s = 0; s < rooms[i].roomSpawnPoints.Length; s++)
                    {
                        if (rooms[i].roomSpawnPoints[s].openingDirection == 2)
                        {
                            hasTroubleSP = true;
                        }
                    }

                    if (possibleRooms.Contains(rooms[i]) && hasTroubleSP == true)
                    {
                        possibleRooms.Remove(rooms[i]);
                    }
                }
            }
        }
        if (SP.openingDirection != 3 && Physics.Raycast(SP.transform.position, transform.right, out hitE, 100))
        {
            if (hitE.transform.tag != "SpawnPoint")
            {
                for (int i = 0; i < rooms.Length; i++)
                {
                    bool hasTroubleSP = false;
                    for (int s = 0; s < rooms[i].roomSpawnPoints.Length; s++)
                    {
                        if (rooms[i].roomSpawnPoints[s].openingDirection == 4)
                        {
                            hasTroubleSP = true;
                        }
                    }

                    if (possibleRooms.Contains(rooms[i]) && hasTroubleSP == true)
                    {
                        possibleRooms.Remove(rooms[i]);
                    }
                }
            }
        }
        if (SP.openingDirection != 4 && Physics.Raycast(SP.transform.position, -transform.right, out hitW, 100))
        {
            if (hitW.transform.tag != "SpawnPoint")
            {
                for (int i = 0; i < rooms.Length; i++)
                {
                    bool hasTroubleSP = false;
                    for (int s = 0; s < rooms[i].roomSpawnPoints.Length; s++)
                    {
                        if (rooms[i].roomSpawnPoints[s].openingDirection == 3)
                        {
                            hasTroubleSP = true;
                        }
                    }

                    if (possibleRooms.Contains(rooms[i]) && hasTroubleSP == true)
                    {
                        possibleRooms.Remove(rooms[i]);
                    }
                }
            }
        }

        return possibleRooms[Random.Range(0, possibleRooms.Count)];
    }
}