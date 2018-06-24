using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MathG;

public class HUDController : MonoBehaviour {

	private RectTransform healthBar;
	private Text healthText;

	private PlayerController playerControl;

	private float healthBarMaxWidth;
	private float healthBarCurrWidth;

	// Use this for initialization
	void Awake () {
		healthBar = GameObject.Find("PlayerHealthBar").GetComponent<RectTransform>();
		healthText = GameObject.Find("PlayerHealthText").GetComponent<Text>();

		playerControl = GameObject.Find("Player").GetComponent<PlayerController>();

		healthBarMaxWidth = healthBar.rect.width;
		healthBarCurrWidth = healthBarMaxWidth;

	}
	
	// Update is called once per frame
	void Update () {
		
		UpdateHealthBar();

	}

	public void UpdateHealthBar(){
		healthText.text = playerControl.stats.GetHealth() + "/" + playerControl.stats.GetMaxHealth();

		healthBarCurrWidth = MathG.Mapping.Map(playerControl.stats.GetHealth(), 0f, playerControl.stats.GetMaxHealth(), 0, healthBarMaxWidth);

		healthBar.sizeDelta = new Vector2(healthBarCurrWidth, 25);

	}
}
