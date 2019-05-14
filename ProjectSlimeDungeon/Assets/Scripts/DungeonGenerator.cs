using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Room Prefabs")]
    //connections are rooms with open doors in the opposite direction there name states to connect with there opposing spawn point
    public Room spawn;
    public Room[] bottomConnectionRoom;
    public Room[] topConnectionRoom;
    public Room[] rightConnectionRoom;
    public Room[] leftConnectionRoom;

    [Header("Dungeon Generaton Data")]
    public Room[,,] map;
    private List<SpawnPoint> openSpawns;
    private Room startRoom;
    private int currentLayer = 0;

    [Header("Dungeon Information")]
    public int mapRoomPositionX, mapLayerY, mapRoomPositionZ;
    public int seed;

    public void Start()
    {
        map = new Room[mapRoomPositionX, mapLayerY, mapRoomPositionZ];
        openSpawns = new List<SpawnPoint>();
        seed = Random.Range(0, int.MaxValue);
    }
    
    public void StartGeneration()
    {
        GameObject s = Instantiate(spawn.gameObject);
        Room sRoom = s.GetComponent<Room>();
        startRoom = s.GetComponent<Room>();
        map[mapRoomPositionX / 2, mapLayerY, mapRoomPositionZ] = sRoom;
        for(int i = 0; i < sRoom.roomSpawnPoints.Length; i++)
        {
            openSpawns.Add(sRoom.roomSpawnPoints[i]);
        }
        while(openSpawns.Count > 0)
        {
            GenerateNextRoom();
        }

    }

    public void GenerateNextRoom()
    {
        SpawnPoint nextSpawn = openSpawns[Random.Range(0, openSpawns.Count)];
        Room selectedRoom;
        Room extensionRom = nextSpawn.transform.parent.GetComponent<Room>();
        //finding where the randomly selected room is on the map to be used to place the new room on the map
        int X = 0;
        int Z = 0;
        for (int x = 0; x < mapRoomPositionX; x++)
        {
            for(int z = 0; z < mapRoomPositionZ; z++)
            {
                if(map[x,currentLayer,z] = extensionRom)
                {
                    X = x;
                    Z = z;
                }
            }
        }
        // selecting what room to make and instantiate it
        if (nextSpawn.openingDirection == 1)
        {
            selectedRoom = bottomConnectionRoom[Random.Range(0, bottomConnectionRoom.Length)];
            GameObject newRoom = Instantiate(selectedRoom.gameObject, nextSpawn.transform.position, selectedRoom.transform.rotation);
            map[X, currentLayer, Z + 1] = newRoom.GetComponent<Room>();
        }
        else if (nextSpawn.openingDirection == 2)
        {
            selectedRoom = topConnectionRoom[Random.Range(0, topConnectionRoom.Length)];
            GameObject newRoom = Instantiate(selectedRoom.gameObject, nextSpawn.transform.position, selectedRoom.transform.rotation);
            map[X, currentLayer, Z - 1] = newRoom.GetComponent<Room>();
        }
        else if (nextSpawn.openingDirection == 3)
        {
            selectedRoom = rightConnectionRoom[Random.Range(0, rightConnectionRoom.Length)];
            GameObject newRoom = Instantiate(selectedRoom.gameObject, nextSpawn.transform.position, selectedRoom.transform.rotation);
            map[X + 1, currentLayer, Z] = newRoom.GetComponent<Room>();
        }
        else if (nextSpawn.openingDirection == 4)
        {
            selectedRoom = leftConnectionRoom[Random.Range(0, leftConnectionRoom.Length)];
            GameObject newRoom = Instantiate(selectedRoom.gameObject, nextSpawn.transform.position, selectedRoom.transform.rotation);
            map[X - 1, currentLayer, Z] = newRoom.GetComponent<Room>();
        }
    }
}
