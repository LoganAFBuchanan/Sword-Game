using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour {

	private MapCreation mapScript;

	// Use this for initialization
	void Awake () {
		mapScript = GameObject.Find("Map_Creator").GetComponent<MapCreation>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
    {
		if(other.gameObject.layer == 11){
			mapScript.ResetMap();
		}
	}
}
