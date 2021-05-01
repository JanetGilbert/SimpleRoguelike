using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { EMPTY, FLOOR, CORRIDOR };

public class GenerateLevel : MonoBehaviour
{
    // Describes a simple rectangular room.
    private struct RoomDef
    {
        public int x;
        public int y;
        public int w;
        public int h;

        public int CenterX
        {
            get => x + w/2;
        }

        public int CenterY
        {
            get => y + h / 2;
        }
    }


    // Cache
    private DisplayDungeon displayDungeon; // For displaying the dungeon

    // Level definitions
    const int levelX = 30;
    const int levelY = 20;

    const int maxRooms = 5;
    const int minRoomSize = 3;
    const int maxRoomSize = 6;

    // Level
    private TileType[,] level = new TileType[levelX, levelY];


    // Rooms
    private List<RoomDef> roomDefs = new List<RoomDef>();

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

        // todo: GENERATION ALGORITHM
        

    }


    // Is this position inside the level grid?
    bool IsValidPosition(int x, int y)
    {
        return ((x > 0) && (x < levelX) && (y > 0) && (y < levelY));
    }

    // Check whether a rectangle is unused.
    bool IsEmpty(int startX, int startY, int w, int h)
    {
        if (!IsValidPosition(startX, startY) || !IsValidPosition(startX + w - 1, startY + h - 1))
        {
            return false;
        }

        for (int x = startX; x < startX + w; x++)
        {
            for (int y = startY; y < startY + h; y++)
            {
                if (level[x,y] != TileType.EMPTY)
                {
                    return false;
                }
            }
        }

        return true;
    }
    

}
