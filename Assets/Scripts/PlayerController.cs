using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
 
	[System.NonSerialized] public ObjectStats stats;
	private PlayerMovement moveScript;

	private SpriteRenderer spriteRender;

	public Sprite upLook;
	public Sprite rightLook;
	public Sprite downLook;
	


	// Use this for initialization
	void Awake () {

		stats = GetComponent<ObjectStats>();
		spriteRender = GetComponent<SpriteRenderer>();
		moveScript = GetComponent<PlayerMovement>();

		stats.SetMaxHealth(Constants.PLAYER_STARTING_HEALTH);
		stats.SetHealth(stats.GetMaxHealth());
		stats.SetDamage(Constants.PLAYER_STARTING_DAMAGE);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void ChangeSprite(int dir){
		switch(dir){
			case 0:
              spriteRender.sprite = upLook;
			  spriteRender.flipX = false;
              break;
			case 1:
              spriteRender.sprite = rightLook;
			  spriteRender.flipX = false;
              break;
          	case 2:
              spriteRender.sprite = downLook;
			  spriteRender.flipX = false;
              break;
			case 3:
              spriteRender.sprite = rightLook;
			  spriteRender.flipX = true;
              break;
          	default:
              Debug.Log("Change Sprite for Player Error");
              break;
		}
	}
}
