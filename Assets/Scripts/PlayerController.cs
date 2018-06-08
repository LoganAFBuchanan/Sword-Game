using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private ObjectStats stats;


	// Use this for initialization
	void Awake () {

		stats = GetComponent<ObjectStats>();

		stats.SetMaxHealth(Constants.PLAYER_STARTING_HEALTH);
		stats.SetHealth(stats.GetMaxHealth());
		stats.SetDamage(Constants.PLAYER_STARTING_DAMAGE);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
