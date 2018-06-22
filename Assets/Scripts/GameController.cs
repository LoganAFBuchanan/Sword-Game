using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private GameObject enemyList;
	private GameObject player;



	// Use this for initialization
	void Awake () {
		
		enemyList = GameObject.Find("EnemyList");
		player = GameObject.Find("Player");

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//This is where all enemy and passive updates will occur
	public void TurnEnd(){

		MoveEnemies();

	}


	public void MoveEnemies(){

		foreach (Transform enemy in enemyList.transform)
        {
			EnemyController enemyScript = enemy.gameObject.GetComponent<EnemyController>();
			enemyScript.moveEnemy();

		}

	}
}
