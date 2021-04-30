using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DisplayDungeon : MonoBehaviour
{

    // Tile definitions.
    [System.Serializable]
    private struct TileDef
    {
        public TileType type; // Tile name
        public Sprite tile; // Tile sprite
        public char debugChar; // Debug display for tile.
    }

    // Set tile definitions in editor.
    [SerializeField] private TileDef[] tileDefs;

    // Grid layout gaps
    private float xGap;
    private float yGap;

    


    void Start()
    {
        // Gaps are the size of a tile.
        xGap = tileDefs[0].tile.bounds.size.x;
        yGap = -tileDefs[0].tile.bounds.size.y; // Going upwards
    }


    void Update()
    {

    }

    // Get the char representing the tile for debugging.
    private char GetDebugChar(TileType type)
    {
        foreach (TileDef def in tileDefs)
        {
            if (type == def.type)
            {
                return def.debugChar;
            }
        }

        return ' ';
    }

    // Get the sprite for the tile.
    private Sprite GetTileSprite(TileType type)
    {
        foreach (TileDef def in tileDefs)
        {
            if (type == def.type)
            {
                return def.tile;
            }
        }

        return null;
    }

    // Setup tiles to display grid.
    public void SetupTiles(TileType[,] level)
    {
        // Width and height of level grid.
        int width = level.GetLength(0); 
        int height = level.GetLength(1); 

        float xPos = 0.0f;
        float yPos = 0.0f;

        for (int x = 0; x < width; x++)
        {
            yPos = 0.0f;

            for (int y = 0; y < height; y++)
            {
                // Create game object.
                SpriteRenderer tile = new GameObject("Tile_" + x + " " + y).AddComponent<SpriteRenderer>();

                tile.transform.parent = transform;
                tile.sprite = GetTileSprite(level[x, y]);
                tile.transform.position = new Vector3(xPos, yPos, 0.0f);

                yPos += yGap;
            }

            xPos += xGap;
        }

        // Ensure camera displays the whole grid, centered.
        CameraFit.Fit(new Rect(-(xGap / 2.0f), -(yGap / 2.0f), (width * xGap), height * yGap));
    }

    // Debug Print level
    public void PrintTiles(TileType[,] level)
    {
        int width = level.GetLength(0);
        int height = level.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            string line = "";

            for (int x = 0; x < width; x++)
            {
                line += GetDebugChar(level[x, y]);
            }

            Debug.Log(line);
        }
    }

}
