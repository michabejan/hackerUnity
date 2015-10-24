using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

    public Tile tilePrefab;
    public List<Tile> tiles;
     
    

    public void build(int numberOfTiles, float xRange, float zOffset)
    {   

        Tile nextTile = buildFirstTile();

        
        for (int i = 0; i < numberOfTiles; ++i)
        {
            // assign tile from previous iteration as previousTile
            Tile previousTile = nextTile;       
            nextTile = buildNextTile(previousTile, xRange, zOffset);

            
                    
        }
    }

    private Tile buildFirstTile()
    {
        Tile tile = Instantiate(tilePrefab) as Tile;
        //tile.transform.parent = transform;
        tile.transform.localPosition = new Vector3(0, 0, 0);

        return tile;
    }

    private Tile buildNextTile(Tile previousTile, float xRange, float zOffset)
    {
        Tile nextTile = nextTile = Instantiate(tilePrefab) as Tile;

        Vector3 oldPosition = previousTile.transform.localPosition;

        float newX = Random.Range(oldPosition.x - xRange, oldPosition.x + xRange);
        float newZ = oldPosition.z + zOffset;

        Vector3 newPosition = new Vector3(newX, 0,  newZ);

        nextTile.transform.localPosition = newPosition;
        return nextTile;
    }


    private Vector3 getNextVector()
    {
        return new Vector3(1, 1, 1);
    }


    

}
