using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour {

	private MapCreation mapScript;
	private PlayerController playerControl;
	private GameController gameControl;

	// Use this for initialization
	void Awake () {
		mapScript = GameObject.Find("Map_Creator").GetComponent<MapCreation>();
		playerControl = GameObject.Find("Player").GetComponent<PlayerController>();
		gameControl = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D other)
    {
		if(other.gameObject.layer == 11){
			if(!other.GetComponent<MoveObject>().isMoving){
				playerControl.stats.currFloor += 1;
				mapScript.ResetMap();
				gameControl.UpdateFog();
			}
		}
	}
}
