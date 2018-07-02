﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    //COntrols enemy behaviour and delegates to other scripts

    private Transform enemyTransform;
    private BoxCollider2D enemyBoxCollider;
    private ObjectStats stats;
    private MoveObject moveScript;
    private MoveConfirmation moveConf;

    private GameObject player;

    private CameraController gameCamera;

    private Pathfinding.AIDestinationSetter destSetter;
    private Pathfinding.AILerp destLerp;

    private Vector3 destVector;
    private Quaternion destQuad;

    private RectTransform enemyHealthBar;
    private GameObject enemyCanvas;
    private float healthBarMaxWidth;
    private float healthBarCurrWidth;

    public enum EnemyType
    {
        Basic,
        Advanced
    };

    public EnemyType enemyType;

    // Use this for initialization
    void Awake()
    {

        destSetter = GetComponent<Pathfinding.AIDestinationSetter>();
        destLerp = GetComponent<Pathfinding.AILerp>();
        player = GameObject.Find("Player");

        enemyTransform = GetComponent<Transform>();
        enemyBoxCollider = GetComponent<BoxCollider2D>();
        stats = GetComponent<ObjectStats>();
        moveScript = GetComponent<MoveObject>();
        moveConf = GetComponent<MoveConfirmation>();

        gameCamera = GameObject.Find("Main Camera").GetComponent<CameraController>();

        enemyHealthBar = this.transform.Find("EnemyCanvas").transform.Find("EnemyHealthBar").gameObject.GetComponent<RectTransform>();
        enemyCanvas = this.transform.Find("EnemyCanvas").gameObject;
        healthBarMaxWidth = enemyHealthBar.rect.width;
		healthBarCurrWidth = healthBarMaxWidth;

        if (enemyType == EnemyType.Basic)
        {
            stats.SetMaxHealth(Constants.BASIC_ENEMY_HEALTH);
            stats.SetHealth(stats.GetMaxHealth());
            stats.SetDamage(Constants.BASIC_ENEMY_DAMAGE);
        }

        destSetter.target = player.transform;

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (stats.GetHealth() <= 0)
        {
            //Debug.log(this.gameObject + " HAS DIED");
            this.gameObject.SetActive(false);
        }

        if (Input.GetButtonDown("Jump") && !moveScript.isMoving)
        {
            moveEnemy();

        }

        UpdateHealthBar();

    }

    

    public void moveEnemy()
    {
        destLerp.MovementUpdate(Time.deltaTime, out destVector, out destQuad);
        //Debug.log("EnemyTransform: " + enemyTransform.position);
        //Debug.log(destVector);

        Vector2 moveVector = destVector - enemyTransform.position;

        //Debug.log(moveVector.normalized);

        if (Mathf.Abs(moveVector.x) > Mathf.Abs(moveVector.y))
        {
            if (moveVector.x > 0)
            {
                moveVector = new Vector2(1, 0);
            }
            else
            {
                moveVector = new Vector2(-1, 0);
            }
        }
        else
        {
            if (moveVector.y > 0)
            {
                moveVector = new Vector2(0, 1);
            }
            else
            {
                moveVector = new Vector2(0, -1);
            }
        }

        RaycastHit2D ray = new RaycastHit2D();
        //Check for enemy or walls
        if (
            moveConf.canMove(this.gameObject, moveVector.x, moveVector.y, out ray) == 4 ||
            moveConf.canMove(this.gameObject, moveVector.x, moveVector.y, out ray) == 2 ||
            moveConf.canMove(this.gameObject, moveVector.x, moveVector.y, out ray) == 1
            )
        {
            return;
        }

        //Check for player
        if (
            moveConf.canMove(this.gameObject, moveVector.x, moveVector.y, out ray) == 3
            )
        {
            player.GetComponent<PlayerController>().stats.ChangeHealth(-stats.GetDamage());
            return;
        }


        StartCoroutine(moveScript.move(enemyTransform, moveVector));
        destLerp.SearchPath();

        
    }

    private void UpdateHealthBar(){

        if(stats.GetHealth() >= stats.GetMaxHealth()){
            enemyCanvas.SetActive(false);
        }else if(stats.GetHealth() < stats.GetMaxHealth() && !enemyCanvas.activeInHierarchy){
            enemyCanvas.SetActive(true);
        }

        healthBarCurrWidth = MathG.Mapping.Map(stats.GetHealth(), 0f, stats.GetMaxHealth(), 0, healthBarMaxWidth);

		enemyHealthBar.sizeDelta = new Vector2(healthBarCurrWidth, 0.15f);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //gameCamera.startFreeze(0.05f);
        //Debug.log("COLLISION WITH " + other.gameObject.name);

        //Collision with sword
        if (other.gameObject.layer == 9 && !moveScript.isMoving)
        {
            GameObject parent = other.gameObject.transform.parent.gameObject;

            if (parent.layer == 11)
            { //If the parent is the player
                int damage = parent.GetComponent<ObjectStats>().GetDamage();
                stats.ChangeHealth(-damage);

                //Debug.log(this.gameObject.name + "'s current health is " + stats.GetHealth());
            }


            Vector3 direction = other.transform.position - enemyTransform.position;
            //Debug.log("This is the difference vector" + direction);
            if (direction.x > 0 && Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                print("Right?");
                StartCoroutine(moveScript.move(enemyTransform, new Vector2(-1, 0)));
                return;
            }
            if (direction.x < 0 && Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                print("Left?");
                StartCoroutine(moveScript.move(enemyTransform, new Vector2(1, 0)));
                return;
            }
            if (direction.y > 0 && Mathf.Abs(direction.y) >= Mathf.Abs(direction.x))
            {
                print("Top?");
                StartCoroutine(moveScript.move(enemyTransform, new Vector2(0, -1)));
                return;
            }
            if (direction.y < 0 && Mathf.Abs(direction.y) >= Mathf.Abs(direction.z))
            {
                print("Bottom?");
                StartCoroutine(moveScript.move(enemyTransform, new Vector2(0, 1)));
                return;
            }
        }

        //Collision with Wall
        if (other.gameObject.layer == 8)
        {

            int damage = Constants.WALL_DAMAGE;
            stats.ChangeHealth(-damage);

            other.gameObject.SetActive(false);

            //Debug.log(this.gameObject.name + "'s current health is " + stats.GetHealth());

        }

        //Collision with other enemy
        if (other.gameObject.layer == 10)
        {
            Vector3 direction = other.transform.position - enemyTransform.position;

            MoveObject otherScript = other.gameObject.GetComponent<MoveObject>();

            //Debug.log("This is the difference vector" + direction);
            if (!otherScript.isMoving)
            {
                if (direction.x > 0 && Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
                {
                    print("Right?");
                    StartCoroutine(otherScript.move(other.transform, new Vector2(1, 0)));
                    return;
                }
                if (direction.x < 0 && Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
                {
                    print("Left?");
                    StartCoroutine(otherScript.move(other.transform, new Vector2(-1, 0)));
                    return;
                }
                if (direction.y > 0 && Mathf.Abs(direction.y) >= Mathf.Abs(direction.x))
                {
                    print("Top?");
                    StartCoroutine(otherScript.move(other.transform, new Vector2(0, 1)));
                    return;
                }
                if (direction.y < 0 && Mathf.Abs(direction.y) >= Mathf.Abs(direction.z))
                {
                    print("Bottom?");
                    StartCoroutine(otherScript.move(other.transform, new Vector2(0, -1)));
                    return;
                }
            }

        }
    }
}
