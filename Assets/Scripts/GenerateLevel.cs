using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { EMPTY, FLOOR, CORRIDOR };

public class GenerateLevel : MonoBehaviour
{
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
    private DisplayDungeon displayDungeon;

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

        // Place rooms
        int tries = 99; // Don't keep trying forever or we could get an infinite loop.
        int roomsMade = 0;

        while (tries > 0 && roomsMade < maxRooms)
        {
            RoomDef room;

            bool success = PlaceRoom(out room);

            if (success)
            {
                // Connect to previous room with tunnel
                if (roomDefs.Count > 0)
                {
                    MakeTunnel(roomDefs[roomDefs.Count - 1], room);
                }

                roomDefs.Add(room); // Add to list of rooms

                roomsMade++;
            }

            tries--;
        }

 

    }

    // Place room (rectangle) in level randomly.
    private bool PlaceRoom(out RoomDef room)
    {
        room.w = Random.Range(minRoomSize, maxRoomSize);
        room.h = Random.Range(minRoomSize, maxRoomSize);
        room.x = Random.Range(0, levelX);
        room.y = Random.Range(0, levelY);

        if (IsEmpty(room.x-1, room.y-1, room.w+2, room.h+2)) // Leave a border
        {
            Debug.Log($"Place room at {room.x}, {room.y} {room.w}, {room.h}");

            for (int x = room.x; x < room.x + room.w; x++)
            {
                for (int y = room.y; y < room.y + room.h; y++)
                {
                    level[x, y] = TileType.FLOOR;
                }
            }

            return true;
        }

        return false;
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


    // Inspired by http://www.roguebasin.com/index.php?title=Complete_Roguelike_Tutorial,_using_python%2Blibtcod,_part_3
    // Draws a tunnel (possibly with a bend in it) from the center of one room to the center of the other.
    private void MakeTunnel(RoomDef prev, RoomDef cur)
    {
        void TunnelVertical(int x, int startY, int endY)
        {
            if (startY > endY) (startY, endY) = (endY, startY); // Direction is not important

            for (int y = startY; y < endY; y++)
            {
                if (level[x, y] == TileType.EMPTY) level[x, y] = TileType.CORRIDOR;
            }
        }

       void TunnelHorizontal(int y, int startX, int endX)
        {
            if (startX > endX) (startX, endX) = (endX, startX); // Direction is not important

            for (int x = startX; x < endX; x++)
            {
                if (level[x, y] == TileType.EMPTY) level[x, y] = TileType.CORRIDOR;
            }
        }

        if (Random.Range(0, 2) == 0)
        {
            TunnelVertical(prev.CenterX, prev.CenterY, cur.CenterY);
            TunnelHorizontal(cur.CenterY, prev.CenterX, cur.CenterX);
        }
        else
        {
            TunnelHorizontal(prev.CenterY, prev.CenterX, cur.CenterX);
            TunnelVertical(cur.CenterX, prev.CenterY, cur.CenterY);
        }
    }


}
