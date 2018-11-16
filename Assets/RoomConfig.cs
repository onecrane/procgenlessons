using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Room
{
    public bool isOpenUp, isOpenDown, isOpenLeft, isOpenRight;
    public Vector2Int logicalPosition;
}

public class Generator
{

    Vector2Int GetRandomPosition()
    {
        bool isValid = false;
        while (!isValid && openPositions.Count > 0)
        {
            Vector2Int nextPosition = openPositions[Random.Range(0, openPositions.Count)];
            if (nextPosition.x - minX > 4 || maxX - nextPosition.x > 4 || nextPosition.y - minY > 4 || maxY - nextPosition.y > 4)
                openPositions.Remove(nextPosition);
            else
                return nextPosition;
        }
        throw new System.Exception("Ran out of spaces.");
    }

    int maxX = 0, maxY = 0, minX = 0, minY = 0;
    Dictionary<Vector2Int, Room> rooms = new Dictionary<Vector2Int, Room>();
    List<Vector2Int> openPositions = new List<Vector2Int>();

    public Generator(GameObject roomPrefab)
    {
        rooms.Add(Vector2Int.zero, new Room() { logicalPosition = Vector2Int.zero });
        openPositions.Add(new Vector2Int(1, 0));
        openPositions.Add(new Vector2Int(0, 1));
        openPositions.Add(new Vector2Int(-1, 0));
        openPositions.Add(new Vector2Int(0, -1));

        for (int i = 0; i < 8; i++)
        {
            Vector2Int nextPosition = GetRandomPosition();

            rooms.Add(nextPosition, new Room() { logicalPosition = nextPosition });

            if (nextPosition.x < minX) minX = nextPosition.x;
            if (nextPosition.x > maxX) maxX = nextPosition.x;
            if (nextPosition.y < minY) minY = nextPosition.y;
            if (nextPosition.y > maxY) maxY = nextPosition.y;

            if (!openPositions.Contains(nextPosition + Vector2Int.up) && !rooms.ContainsKey(nextPosition + Vector2Int.up) && maxY - minY < 4) openPositions.Add(nextPosition + Vector2Int.up);
            if (!openPositions.Contains(nextPosition + Vector2Int.down) && !rooms.ContainsKey(nextPosition + Vector2Int.down) && maxY - minY < 4) openPositions.Add(nextPosition + Vector2Int.down);
            if (!openPositions.Contains(nextPosition + Vector2Int.left) && !rooms.ContainsKey(nextPosition + Vector2Int.left) && maxX - minX < 4) openPositions.Add(nextPosition + Vector2Int.left);
            if (!openPositions.Contains(nextPosition + Vector2Int.right) && !rooms.ContainsKey(nextPosition + Vector2Int.right) && maxX - minX < 4) openPositions.Add(nextPosition + Vector2Int.right);

            openPositions.Remove(nextPosition);

        }

        foreach (Vector2Int pos in rooms.Keys)
        {
            Room room = rooms[pos];

            room.isOpenUp = rooms.ContainsKey(pos + Vector2Int.up);
            room.isOpenDown = rooms.ContainsKey(pos + Vector2Int.down);
            room.isOpenLeft = rooms.ContainsKey(pos + Vector2Int.left);
            room.isOpenRight = rooms.ContainsKey(pos + Vector2Int.right);
            
            room.logicalPosition = pos + new Vector2Int(-minX, -minY);

            GameObject newRoom = GameObject.Instantiate(roomPrefab);
            newRoom.GetComponent<RoomConfig>().Configure(room);
        }
    }
}

public class RoomConfig : MonoBehaviour {

    public void Configure(Room room)
    {
        transform.position = new Vector3(room.logicalPosition.x * 5, 0, room.logicalPosition.y * 5);
        transform.Find("upDoor").gameObject.SetActive(!room.isOpenUp);
        transform.Find("downDoor").gameObject.SetActive(!room.isOpenDown);
        transform.Find("leftDoor").gameObject.SetActive(!room.isOpenLeft);
        transform.Find("rightDoor").gameObject.SetActive(!room.isOpenRight);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
