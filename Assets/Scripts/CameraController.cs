using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startFreeze(float dur){
		//StartCoroutine(freezeFrame(dur));
	}

	public IEnumerator freezeFrame(float dur)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(dur);
        Time.timeScale = 1;
    }
}
