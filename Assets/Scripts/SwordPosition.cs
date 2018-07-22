using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPosition : MonoBehaviour {

	private GameController gameControl;

	private Transform swordTransform;
	private GameObject player;
	private Transform playerTransform;
	private MoveObject playerMovementScript;
	private MoveConfirmation moveConf;
	private float input;

	private float lerpVal;

	private bool rotating;
	public float rotateSpeed;
	private bool rotateCoolingDown;
	private int rotateDirection;
	private int rotatePosition;

	public float cooldownTime;

	private RaycastHit2D sideRay;
    private RaycastHit2D diagRay;

	// Use this for initialization
	private void Awake () {
		swordTransform = GetComponent<Transform>();

		player = GameObject.Find("Player");
		playerTransform = player.GetComponent<Transform>();
		playerMovementScript = player.GetComponent<MoveObject>();
		moveConf = player.GetComponent<MoveConfirmation>();
		gameControl = GameObject.Find("GameController").GetComponent<GameController>();


		rotating = false;
		lerpVal = 0;

		rotatePosition = 0;
	}
	
	// Update is called once per frame
	private void FixedUpdate () {
		if (!playerMovementScript.isMoving)
        {
            input = Input.GetAxis("Rotate");
			//Debug.Log(input);

            if (input != 0 && !rotateCoolingDown && !rotating && moveConf.canRotate(RoundOnes(input), out sideRay, out diagRay, rotatePosition))
            {
				
				InitiateRotation(input);
				StartCoroutine(EndTurn());
				
            }

			
        }
	}

	

	private IEnumerator Rotate(Transform sword, Transform player, int dir){
		
		//Rotates the sword at the corresponding rotation speed
		

		while(lerpVal < 90.0f){
			sword.RotateAround(player.position, Vector3.forward, rotateSpeed * Time.deltaTime * -dir);
			lerpVal += Time.deltaTime * rotateSpeed; //Increments the total angle travelled

			//Checks whether the sword has rotated a full 90 degrees and then counter rotates any amount that it over shot
			if(lerpVal >= 90.0f){
				sword.RotateAround(player.position, Vector3.forward, (lerpVal - 90.0f) * dir);
				rotating = false;
			}
			yield return null;
		}
		
		
		lerpVal = 0.0f;
		
		yield return 0;
	}

	//Theres probably a relavent Math function that does this
	private int RoundOnes(float num){

		if(num < 0){
			return -1;
		}
		if(num > 0){
			return 1;
		}
		return 0;

	}

	private void UpdatePosition(int change){

		rotatePosition += change;

		if(rotatePosition < 0){
			rotatePosition = 3;
		}
		else if(rotatePosition > 3){
			rotatePosition = 0;
		}

		player.GetComponent<PlayerController>().ChangeSprite(rotatePosition);

		Debug.Log("Sword position is " + rotatePosition);

	}

	public void CornerPull(Transform player, int pos){

		Vector2 endPos = new Vector2(0, 0);

		if (pos == 0) endPos = new Vector2(0, 1);
		if (pos == 1) endPos = new Vector2(1, 0);
		if (pos == 2) endPos = new Vector2(0,-1);
		if (pos == 3) endPos = new Vector2(-1, 0);

		StartCoroutine(playerMovementScript.move(playerTransform, endPos));
		
	}

	public void WallPush(Vector2 playerMove, int pos){
	
		StartCoroutine(playerMovementScript.move(playerTransform, playerMove));
	}

	public void DragBehind(Vector2 playerDir){
		Debug.Log(playerDir);

		//Moving Up
		if(playerDir == new Vector2(0.0f, 1.0f)){
			if(rotatePosition == 1){
				InitiateRotation(1.0f);
			}
			if(rotatePosition == 3){
				InitiateRotation(-1.0f);
			}
		}
		//Moving Down
		if(playerDir == new Vector2(0.0f, -1.0f)){
			if(rotatePosition == 1){
				InitiateRotation(-1.0f);
			}
			if(rotatePosition == 3){
				InitiateRotation(1.0f);
			}
		}
		//Moving right
		if(playerDir == new Vector2(1.0f, 0.0f)){
			if(rotatePosition == 0){
				InitiateRotation(-1.0f);
			}
			if(rotatePosition == 2){
				InitiateRotation(1.0f);
			}
		}
		//Moving Left
		if(playerDir == new Vector2(-1.0f, 0.0f)){
			if(rotatePosition == 0){
				InitiateRotation(1.0f);
			}
			if(rotatePosition == 2){
				InitiateRotation(-1.0f);
			}
		}
	}

	private void InitiateRotation(float dirInput){
		//STarts rotating and puts the rotation on cooldown
        Debug.Log("Rotating...");
		rotating = true;
		rotateCoolingDown = true;
		rotateDirection = RoundOnes(dirInput);
		UpdatePosition(RoundOnes(dirInput));
		StartCoroutine(RotateCooldown());	
		StartCoroutine(Rotate(swordTransform, playerTransform, rotateDirection));	
	}

	//Puts rotation on cooldown
	IEnumerator RotateCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
		rotateCoolingDown = false;
        
    }

	public int GetRotatePosition(){
		return rotatePosition;
	}

	IEnumerator EndTurn(){
        yield return new WaitUntil(() => !rotating);
        gameControl.TurnEnd();
        gameControl.UpdateFog();
    }


}
