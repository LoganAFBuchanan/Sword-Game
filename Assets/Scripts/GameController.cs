using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private GameObject enemyList;
	private GameObject player;
	private PlayerController playerControl;



	// Use this for initialization
	void Awake () {
		
		enemyList = GameObject.Find("EnemyList");
		player = GameObject.Find("Player");

		playerControl = player.GetComponent<PlayerController>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//This is where all enemy and passive updates will occur
	public void TurnEnd(){

		StartCoroutine(MoveEnemies());

	}


	IEnumerator MoveEnemies(){

		playerControl.SetMoveWait(true);
		yield return new WaitForSeconds(0.1f);
		foreach (Transform enemy in enemyList.transform)
        {
			if(enemy.gameObject.activeInHierarchy){
			EnemyController enemyScript = enemy.gameObject.GetComponent<EnemyController>();
			enemyScript.moveEnemy();
			yield return new WaitForSeconds(0.1f);
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
