using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public int numberOfTiles;
    public float xRange;
    public float zOffset;

    private bool newGame;
    public Map mapPrefab;
    private Map map;
	// Use this for initialization
	void Start () {
        this.BeginGame();
	}
    public Map Map
    {
        get { return map; }
    }

    private void BeginGame()
    {
        map = Instantiate(mapPrefab) as Map;
        map.build(numberOfTiles, xRange, zOffset);
        //map.generate(50, 50);
    } 
}
