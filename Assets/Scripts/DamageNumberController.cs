using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumberController : MonoBehaviour {


	[System.NonSerialized]public int damageVal;
	private RectTransform rect;


	private Text text;

	private float movement;
	private float fade;

	private float speed;


	// Use this for initialization
	void Awake () {
		rect = GetComponent<RectTransform>();
		text = GetComponent<Text>();

		fade = 1;
		movement = 0;
		speed = 1;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateText();
		rect.anchoredPosition = new Vector2(0, movement);
		text.color = new Color(255, 0, 0, fade);

		fade -= Time.deltaTime * speed;
		movement += Time.deltaTime * speed;
		if(fade <= 0){
			Destroy(this.gameObject);
		}
	}

	public void UpdateText(){
		text.text = "" + damageVal;
	}
}
