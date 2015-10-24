using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {


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
        map.proceduralGenerate(30);
        //map.generate(50, 50);
    } 
}
