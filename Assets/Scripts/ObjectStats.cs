using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStats : MonoBehaviour {

	//Used to store enemy type and stats

	private int maxHealth;
	private int health;
	private int damage;


	// Use this for initialization
	void Start () {
		
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
