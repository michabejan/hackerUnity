using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {




   public Tile tilePrefab;

   public List<Tile> tiles;


    public void generate(int sizeX, int sizeY)
    {
        tiles = new List<Tile>();
        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeY; z++)
            {
                Tile tile = Instantiate(tilePrefab) as Tile;
                //      tiles[x, z] = tile;

                tile.transform.parent = transform;
                tile.transform.localPosition = new Vector3(x, 0f, z);
                tile.name = tile.transform.localPosition.x + " " + " + " + tile.transform.localPosition.z;
                tiles.Add(tile);

            }
        }
    }


    public void proceduralGenerate(int sizeY)
    {
        bool t = true;
        int a = 0;
        tiles = new List<Tile>();
        for (int i = 0; i < sizeY; i++)
        {
            
            
            Tile tile = Instantiate(tilePrefab) as Tile;
            tile.transform.parent = transform;
            tile.transform.localPosition = new Vector3(a, 0f, i*2);
            print("lalaalalala");
            tiles.Add(tile);
            /*
            print(a);
            if (t == true)
            {
                a++;
                t = false;
            }
            else
            {
                a--;
                t = true;
            }
         */   
        }

    }


    

}
