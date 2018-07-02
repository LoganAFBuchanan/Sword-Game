using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Core code taken from http://wiki.unity3d.com/index.php/GridMove

public class PlayerMovement : MonoBehaviour
{
    
    private Vector2 input;

    private BoxCollider2D boxCollider;

    private RaycastHit2D ray;

    private MoveConfirmation moveConf;
    private MoveObject moveController;

    private PlayerController playerControl;
    private GameController gameControl;

    private GameObject playerSword;
    private SwordPosition swordPos;

    private bool allowDiagonals;


    protected virtual void Awake(){

        boxCollider = GetComponent<BoxCollider2D>(); 

        moveConf = GetComponent<MoveConfirmation>();
        moveController = GetComponent<MoveObject>();
        playerControl = GetComponent<PlayerController>();
        gameControl = GameObject.Find("GameController").GetComponent<GameController>();

        playerSword = GameObject.Find("Sword");
        swordPos = playerSword.GetComponent<SwordPosition>();

        allowDiagonals = false;
        
        
    }

    public void FixedUpdate()
    {
        if (!moveController.isMoving && !playerControl.GetMoveWait() && (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")))
        {
            PlayerMove();
            gameControl.TurnEnd();
            gameControl.UpdateFog();
        }
    }

    public void PlayerMove(){
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (!allowDiagonals) {
                if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) {
                    input.y = 0;
                } else {
                    input.x = 0;
                }
            }

            //Move into wall with sword in front
            if (input != Vector2.zero && //Checks for player Input
                moveConf.canMove(this.gameObject, System.Math.Sign(input.x), System.Math.Sign(input.y), out ray) == 2 && //Checks player's path
                moveConf.canMove(playerSword, System.Math.Sign(input.x), System.Math.Sign(input.y), out ray) == 1) //Checks Sword's path
            {
                return;
            }
            
            //Move into Enemy
            if (input != Vector2.zero && //Checks for player Input
                moveConf.canMove(this.gameObject, System.Math.Sign(input.x), System.Math.Sign(input.y), out ray) == 4) 
            {
                //May need to have the enemy deal damage here, however I need to somehow grab the enemies stats here to deal the correct amount of damage.
                //Instead I am going to end the player's turn and have the enemy then move onto the player. Without this the player would be dealt damage twice.
                //Ie: the player moves into the enemy and takes damage, and then the enemy move into the layer and does that damage again.
                return;
            }

            //Move into open space with sword being blocked
            if (input != Vector2.zero && //Checks for player Input
                moveConf.canMove(this.gameObject, System.Math.Sign(input.x), System.Math.Sign(input.y), out ray) == 0 && //Checks player's path
                moveConf.canMove(playerSword, System.Math.Sign(input.x), System.Math.Sign(input.y), out ray) == 1) //Checks Sword's path
            {
               // Debug.Log("Moving...");
                StartCoroutine(moveController.move(transform, input));
                swordPos.DragBehind(input);

            }


            //Move into open space
            if (input != Vector2.zero && //Checks for player Input
                (moveConf.canMove(this.gameObject, System.Math.Sign(input.x), System.Math.Sign(input.y), out ray) == 0 || moveConf.canMove(this.gameObject, System.Math.Sign(input.x), System.Math.Sign(input.y), out ray) == 2) && //Checks player's path
                moveConf.canMove(playerSword, System.Math.Sign(input.x), System.Math.Sign(input.y), out ray) == 0) //Checks Sword's path
            {
               // Debug.Log("Moving...");
                StartCoroutine(moveController.move(transform, input));
            }

            

            
    }


}