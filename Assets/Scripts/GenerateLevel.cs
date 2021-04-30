using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { EMPTY, FLOOR };

public class GenerateLevel : MonoBehaviour
{
    // Cache
    private DisplayDungeon displayDungeon;

    // Level definitions
    const int levelX = 10;
    const int levelY = 10;

    const int maxRooms = 3;
    const int minRoomSize = 3;
    const int maxRoomSize = 6;

    // Level
    private TileType[,] level = new TileType[levelX, levelY];


    void Start()
    {
        displayDungeon = GetComponent<DisplayDungeon>();

        Generate();


        displayDungeon.SetupTiles(level);
        displayDungeon.PrintTiles(level);

    }

    
    void Update()
    {
        
    }

    // Create simple level
    void Generate()
    {
        // Clear
        for (int x = 0; x < levelX; x++)
        {
            for (int y = 0; y < levelY; y++)
            {
                level[x,y] = TileType.EMPTY;
            }
        }

        // Place rooms
        for (int r = 0; r < maxRooms; r++)
        {
            PlaceRoom();
        }
    }

    // Place room (rectangle) in level randomly.
    void PlaceRoom()
    {
        int roomWidth = Random.Range(minRoomSize, maxRoomSize);
        int roomHeight = Random.Range(minRoomSize, maxRoomSize);
        int roomX = Random.Range(0, levelX);
        int roomY = Random.Range(0, levelY);

        Debug.Log($"Place room at {roomX}, {roomY} {roomWidth}, {roomHeight}");

        if (IsValidPosition(roomX, roomY) && IsValidPosition(roomX + roomWidth - 1, roomY + roomHeight - 1))
        {
            for (int x = roomX; x < roomX + roomWidth; x++)
            {
                for (int y = roomY; y < roomY + roomHeight; y++)
                {
                    level[x, y] = TileType.FLOOR;
                }
            }
        }
    }

    // Is this position inside the level grid?
    bool IsValidPosition(int x, int y)
    {
        return ((x > 0) && (x < levelX) && (y > 0) && (y < levelY));
    }

}
