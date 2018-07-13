using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour {

	private bool spawned;

	// Use this for initialization
	void Awake () {

		spawned = true;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(spawned){
			spawned = false;
			
			StartCoroutine(WaitToDestroy());
		}
	}

	private IEnumerator WaitToDestroy(){
		yield return new WaitForSeconds(0.5f);
		Debug.Log("FIRE AGGGHHHHHH!!!!!");
		Destroy(this.gameObject);
	}
}
