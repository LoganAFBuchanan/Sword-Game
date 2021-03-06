﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStats : MonoBehaviour {

	//Used to store enemy type and stats

	private int maxHealth;
	private int health;
	private int damage;

	public int sightRange;

	public int atkRange {get; set;}

	[System.NonSerialized] public int goldCount;
	[System.NonSerialized] public int currFloor;
	
	private GameObject objectCanvas;
	private GameObject damageNumber;


	// Use this for initialization
	void Awake () {
		objectCanvas = this.transform.Find("ObjectCanvas").gameObject;
		damageNumber = Resources.Load<GameObject>("Prefabs/DamageNumber");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int GetMaxHealth(){
		return maxHealth;
	}

	public void SetMaxHealth(int input){
		maxHealth = input;
	}

	public void ChangeMaxHealth(int change){
		maxHealth += change;
	}

	public int GetHealth(){
		return health;
	}

	public void SetHealth(int input){
		health = input;
	}

	public void ChangeHealth(int change){
		Debug.Log(this.gameObject.name + "had it's health changed by:" + change);
		GameObject damageNum = GameObject.Instantiate(damageNumber, this.transform);
		damageNum.GetComponent<DamageNumberController>().damageVal = change;
		damageNum.GetComponent<RectTransform>().SetParent(objectCanvas.transform);
		health += change;
	}

	public int GetDamage(){
		return damage;
	}

	public void SetDamage(int input){
		damage = input;
	}

	public void ChangeDamage(int change){
		damage += change;
	}
}
