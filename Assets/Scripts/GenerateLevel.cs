using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    // Level definitions
    const int levelX = 30;
    const int levelY = 30;

    const char empty = 'O';
    const char floor = '#';

    const int maxRooms = 50;
    const int minRoomSize = 3;
    const int maxRoomSize = 6;

    // Level
    char [,] level = new char[levelX, levelY];


    void Start()
    {
        Generate();
        Print();

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
                level[x,y] = empty;
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
                    level[x, y] = floor;
                }
            }
        }
    }

    // Is this position in the level?
    bool IsValidPosition(int x, int y)
    {
        return ((x > 0) && (x < levelX) && (y > 0) && (y < levelY));
    }

    // Debug Print level
    void Print()
    {
        for (int y = 0; y < levelY; y++)
        {
            string line = "";

            for (int x = 0; x < levelX; x++)
            {
                line += level[x, y];
            }

            Debug.Log(line);
        }
    }
}
