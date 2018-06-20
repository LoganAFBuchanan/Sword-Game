using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    //COntrols enemy behaviour and delegates to other scripts

    private Transform enemyTransform;
    private BoxCollider2D enemyBoxCollider;
    private ObjectStats stats;
    private MoveObject moveScript;

    private GameObject player;

    private CameraController gameCamera;

    private Pathfinding.AIDestinationSetter destSetter;
    private Pathfinding.AILerp destLerp;

    private Vector3 destVector;
    private Quaternion destQuad;

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

        gameCamera = GameObject.Find("Main Camera").GetComponent<CameraController>();

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
            Debug.Log(this.gameObject + " HAS DIED");
            this.gameObject.SetActive(false);
        }

        

        if(Input.GetButtonDown("Jump")){
            destLerp.MovementUpdate(Time.deltaTime, out destVector, out destQuad);
            Debug.Log("EnemyTransform: " + enemyTransform.position);
            Debug.Log(destVector);

            //Up
            if(destVector.y > transform.position.y){
                StartCoroutine(moveScript.move(enemyTransform, new Vector2(0, 1)));
                return;
            }
            //Down
            if(destVector.y < transform.position.y){
                StartCoroutine(moveScript.move(enemyTransform, new Vector2(0, -1)));
                return;
            }
            //Right
            if(destVector.x > transform.position.x){
                StartCoroutine(moveScript.move(enemyTransform, new Vector2(1, 0)));
                return;
            }
            //Left
            if(destVector.x < transform.position.x){
                StartCoroutine(moveScript.move(enemyTransform, new Vector2(-1, 0)));
                return;
            }
            
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        gameCamera.startFreeze(0.1f);
        Debug.Log("COLLISION WITH " + other.gameObject.name);
        if (other.gameObject.layer == 9 && !moveScript.isMoving)
        {
            GameObject parent = other.gameObject.transform.parent.gameObject;

            if (parent.layer == 11)
            { //If the parent is the player
                int damage = parent.GetComponent<ObjectStats>().GetDamage();
                stats.ChangeHealth(-damage);

                Debug.Log(this.gameObject.name + "'s current health is " + stats.GetHealth());
            }


            Vector3 direction = other.transform.position - enemyTransform.position;
            Debug.Log("This is the difference vector" + direction);
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
        Debug.Log("COLLISION WITH " + other.gameObject.name);
        if (other.gameObject.layer == 8)
        {

            int damage = Constants.WALL_DAMAGE;
            stats.ChangeHealth(-damage);

            other.gameObject.SetActive(false);

            Debug.Log(this.gameObject.name + "'s current health is " + stats.GetHealth());

        }
    }

    
}
