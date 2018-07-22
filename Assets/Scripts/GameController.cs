using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private GameObject enemyList;
	private GameObject fogList;
	private GameObject player;
	private PlayerController playerControl;
	private MapCreation mapControl;





	// Use this for initialization
	void Awake () {
		
		enemyList = GameObject.Find("EnemyList");
		fogList = GameObject.Find("FogList");
		player = GameObject.Find("Player");
		if(GameObject.Find("Map_Creator") != null){
			mapControl = GameObject.Find("Map_Creator").GetComponent<MapCreation>();
		}

		playerControl = player.GetComponent<PlayerController>();

	}

	void Start(){
		UpdateFog();

		
	}
	
	// Update is called once per frame
	void Update () {
		if(player.GetComponent<MoveObject>().isMoving){
			UpdateFog();
		}

		if(playerControl.stats.GetHealth() <= 0){
			EndGame("Death");
		}
	}

	//This is where all enemy and passive updates will occur
	public void TurnEnd(){

		UpdateFog();
		AstarPath.active.Scan();
		StartCoroutine(MoveEnemies());

	}

	public void UpdateFog(){
		//Debug.Log("Updating Fog");
		foreach(Transform fog in fogList.transform){
			fog.gameObject.GetComponent<FogController>().changeOpacity();
		}
	}

	public void EndGame(string type){
		switch(type){
			case "Death":
				mapControl.ResetMap();
				playerControl.InitializeValues();
			break;
		}
	}


	IEnumerator MoveEnemies(){

		playerControl.SetMoveWait(true);
		//yield return new WaitForSeconds(0.1f);
		foreach (Transform enemy in enemyList.transform)
        {
			if(enemy.gameObject.activeInHierarchy){
			EnemyController enemyScript = enemy.gameObject.GetComponent<EnemyController>();
			
			enemyScript.enemyDecision();
			yield return new WaitUntil(() => !enemyScript.moveScript.isMoving);
			}

		}
		playerControl.SetMoveWait(false);
	}

	IEnumerator WaitTime(float t)
    {
        
        yield return new WaitForSeconds(t);
        MoveEnemies();
    }
}
