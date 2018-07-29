using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseController : MonoBehaviour {

	private PlayerController playerControl;
	private GameObject player;
	

	// Use this for initialization
	void Awake() {
		player = GameObject.Find("Player");
		playerControl = player.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Vampiric(){
		if(playerControl.curses.Contains("vampiric") && playerControl.curses.Contains("of lifestealing")){

			if(Random.Range(0,1f) <= 0.25){
				playerControl.stats.ChangeHealth(1);
			}
		}else{
			if(Random.Range(0,1f) <= 0.10){
				playerControl.stats.ChangeHealth(1);
			}
		}
	}

	public void Jumpy(PlayerMovement moveControl){
		if(playerControl.curses.Contains("jumpy") && playerControl.curses.Contains("of leaping")){
			moveControl.PlayerMove();
			moveControl.PlayerMove();
		}else{
			moveControl.PlayerMove();
		}
	}

}
