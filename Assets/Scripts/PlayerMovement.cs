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

    private GameObject playerSword;
    private SwordPosition swordPos;

    private bool allowDiagonals;

    private bool moveCoolingDown;

    protected virtual void Awake(){

        boxCollider = GetComponent<BoxCollider2D>(); 

        moveConf = GetComponent<MoveConfirmation>();
        moveController = GetComponent<MoveObject>();

        playerSword = GameObject.Find("Sword");
        swordPos = playerSword.GetComponent<SwordPosition>();

        allowDiagonals = false;
        moveCoolingDown = false;
    }

    public void FixedUpdate()
    {
        if (!moveController.isMoving && !moveCoolingDown && (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")))
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (!allowDiagonals) {
                if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) {
                    input.y = 0;
                } else {
                    input.x = 0;
                }
            }

            if (input != Vector2.zero && //Checks for player Input
                moveConf.canMove(this.gameObject, System.Math.Sign(input.x), System.Math.Sign(input.y), out ray) == 2 && //Checks player's path
                moveConf.canMove(playerSword, System.Math.Sign(input.x), System.Math.Sign(input.y), out ray) == 1) //Checks Sword's path
            {
                return;
            }

            if (input != Vector2.zero && //Checks for player Input
                moveConf.canMove(this.gameObject, System.Math.Sign(input.x), System.Math.Sign(input.y), out ray) == 0 && //Checks player's path
                moveConf.canMove(playerSword, System.Math.Sign(input.x), System.Math.Sign(input.y), out ray) == 1) //Checks Sword's path
            {
                Debug.Log("Moving...");
                StartCoroutine(moveController.move(transform, input));
                swordPos.DragBehind(input);

            }

            if (input != Vector2.zero && //Checks for player Input
                (moveConf.canMove(this.gameObject, System.Math.Sign(input.x), System.Math.Sign(input.y), out ray) == 0 || moveConf.canMove(this.gameObject, System.Math.Sign(input.x), System.Math.Sign(input.y), out ray) == 2) && //Checks player's path
                moveConf.canMove(playerSword, System.Math.Sign(input.x), System.Math.Sign(input.y), out ray) == 0) //Checks Sword's path
            {
                Debug.Log("Moving...");
                StartCoroutine(moveController.move(transform, input));
            }
            moveCoolingDown = true;
            StartCoroutine(MoveCoolDown());
        }
    }

    private IEnumerator MoveCoolDown(){
        yield return new WaitForSeconds(Constants.MOVE_COOLDOWN);
		moveCoolingDown = false;
    }


}