using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveConfirmation : MonoBehaviour {

	private GameObject player;
	private GameObject sword;

	private SwordPosition swordScript;
	private Transform swordTransform;
	private BoxCollider2D swordCollider;

	private Transform playerTransform;
	private BoxCollider2D playerCollider;

	private bool swordOK;
	private bool playerOK;

	private RaycastHit2D ray;
    public LayerMask wallLayer;
	public LayerMask swordLayer;
	public LayerMask playerLayer;
	public LayerMask enemyLayer;

	// Use this for initialization
	void Awake () {
		
		player = GameObject.Find("Player");
		sword = GameObject.Find("Sword");

		swordScript = sword.GetComponent<SwordPosition>();
		swordTransform = sword.GetComponent<Transform>();
		swordCollider = sword.GetComponent<BoxCollider2D>();

		playerTransform = player.GetComponent<Transform>();
		playerCollider = player.GetComponent<BoxCollider2D>();
	

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//CanMove should start multiplying by gridsize
	public int canMove(GameObject movableObject, float xDir, float yDir, out RaycastHit2D objectHit)
    {
		//0 = Free
		//1 = Wall
		//2 = Sword
		//3 = Player
		//4 = Enemy

		int foundObject = 0;

		xDir = xDir * Constants.GRIDSIZE;
		yDir = yDir * Constants.GRIDSIZE;

        //Store start position to move from, based on objects current transform position.
        Vector2 objectStart = movableObject.GetComponent<Transform>().position;

        ////Debug.Log("Moving from " + start);

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector2 objectEnd = objectStart + new Vector2(xDir, yDir);

        ////Debug.Log("Moving to " + end);

        //Disable the boxCollider so that linecast doesn't hit this object's own collider.
        movableObject.GetComponent<BoxCollider2D>().enabled = false;

        //Cast a line from start point to end point checking collision on blockingLayer.
        objectHit = Physics2D.Linecast(objectStart, objectEnd, wallLayer);
		

		//Debug.Log(movableObject + " is detecting walls with "+ objectHit.transform);
		
        if(objectHit.transform != null){
			foundObject = 1;
		}

		
		objectHit = Physics2D.Linecast(objectStart, objectEnd, swordLayer);
		//Debug.DrawRay(objectStart, new Vector2(xDir, yDir), Color.green, 4.0f);

		//Debug.Log(movableObject + " is detecting Swords with "+ objectHit.transform);

		if(objectHit.transform != null){
			foundObject = 2;
		}

		objectHit = Physics2D.Linecast(objectStart, objectEnd, playerLayer);
		//Debug.DrawRay(objectStart, new Vector2(xDir, yDir), Color.green, 4.0f);

		//Debug.Log(movableObject + " is detecting Players with "+ objectHit.transform);

		if(objectHit.transform != null){
			foundObject = 3;
		}

		objectHit = Physics2D.Linecast(objectStart, objectEnd, enemyLayer);
		//Debug.DrawRay(objectStart, new Vector2(xDir, yDir), Color.green, 4.0f);

		//Debug.Log(movableObject + " is detecting Enemies with "+ objectHit.transform);

		if(objectHit.transform != null){
			foundObject = 4;
		}


		

        //Re-enable boxCollider after linecast
        movableObject.GetComponent<BoxCollider2D>().enabled = true;

		

        //If something was hit, return false, Move was unsuccesful.
        return foundObject;

    }

	public bool canRotate(int xDir, out RaycastHit2D sideHit, out RaycastHit2D diagHit, int pos)
    {

		bool sideOK = false;
		bool diagOK = false;
		//Store start position to move from, based on objects current transform position.
		Vector2 swordStart = swordTransform.position;

        ////Debug.Log("Moving from " + start);

        // Calculate end position based on the direction parameters passed in when calling Move.
		Vector2 swordEndSide = swordStart + new Vector2(xDir, 0);
		Vector2 swordEndDiag = swordStart + new Vector2(xDir, -1);

		if(pos == 1){
			swordEndSide = swordStart + new Vector2(0, -xDir);
			swordEndDiag = swordStart + new Vector2(-1, -xDir);
		}

		if(pos == 2){
			swordEndSide = swordStart + new Vector2(-xDir, 0);
			swordEndDiag = swordStart + new Vector2(-xDir, 1);
		}

		if(pos == 3){
			swordEndSide = swordStart + new Vector2(0, xDir);
			swordEndDiag = swordStart + new Vector2(1, xDir);
		}

        ////Debug.Log("Moving to " + end);

        //Disable the boxCollider so that linecast doesn't hit this object's own collider.
        playerCollider.enabled = false;
		swordCollider.enabled = false;

        //Cast a line from start point to end point checking collision on blockingLayer.
        sideHit = Physics2D.Linecast(swordStart, swordEndSide, wallLayer);
		diagHit = Physics2D.Linecast(swordStart, swordEndDiag, wallLayer);

        ////Debug.Log(hit);

        //Re-enable boxCollider after linecast
        playerCollider.enabled = true;
		swordCollider.enabled = true;

        //Check if anything was hit
        if (sideHit.transform == null){ sideOK = true; }
		if (diagHit.transform == null){ diagOK = true; }

		if(sideOK && diagOK){
			return true;
		}

		if(sideOK && !diagOK){
			swordScript.CornerPull(playerTransform, pos);
			return true;
		}

		if(!sideOK){
				if((pos == 0 && xDir == 1) || (pos == 2 && xDir == -1)){
					
					if(canMove(player, -1, 0, out ray) == 0){
						swordScript.WallPush(new Vector2(-1, 0), pos);
						return true;
					}
				}
				if((pos == 0 && xDir == -1) || (pos == 2 && xDir == 1)){
					
					if(canMove(player, 1, 0, out ray) == 0){
						swordScript.WallPush(new Vector2(1, 0), pos);
						return true;
					}
				}
				if((pos == 1 && xDir == 1) || (pos == 3 && xDir == -1)){
					
					if(canMove(player, 0, 1, out ray) == 0){
						swordScript.WallPush(new Vector2(0, 1), pos);
						return true;
					}
				}
				if((pos == 1 && xDir == -1) || (pos == 3 && xDir == 1)){
					
					if(canMove(player, 0, -1, out ray) == 0){
						swordScript.WallPush(new Vector2(0, -1), pos);
						return true;
					}
				}
			
			}	

        //If something was hit, return false, Move was unsuccessful.
        return false;
	}


}
