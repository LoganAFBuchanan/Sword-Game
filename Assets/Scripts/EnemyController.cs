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

    private bool attackWait;
    private int playerDirection;

    private LayerMask wallLayer;
    private RaycastHit2D[] playerHit;

    private SpriteRenderer spriteRender;
    public Sprite basicSprite;
    public Sprite dragonSprite;

    public enum EnemyType
    {
        Basic,
        Dragon
    };

    public EnemyType enemyType;

    public GameObject fire;

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

        spriteRender = GetComponent<SpriteRenderer>();
        

        InitializeValues();

        destSetter.target = player.transform;

        wallLayer = 8;
    }

    public void InitializeValues()
    {
        if (enemyType == EnemyType.Basic)
        {
            stats.SetMaxHealth(Constants.BASIC_ENEMY_HEALTH);
            stats.SetHealth(stats.GetMaxHealth());
            stats.SetDamage(Constants.BASIC_ENEMY_DAMAGE);
            spriteRender.sprite = basicSprite;
        }
        if (enemyType == EnemyType.Dragon)
        {
            stats.SetMaxHealth(Constants.DRAGON_ENEMY_HEALTH);
            stats.SetHealth(stats.GetMaxHealth());
            stats.SetDamage(Constants.DRAGON_ENEMY_DAMAGE);
            stats.atkRange = Constants.DRAGON_ENEMY_RANGE;
            spriteRender.sprite = dragonSprite;
            attackWait = false;
        }
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

    public void enemyDecision()
    {

        switch (enemyType)
        {
            case EnemyType.Basic:
                moveEnemy();
                break;
            case EnemyType.Dragon:
                if (attackWait)
                {
                    //Fire breath!!!
                    //Create another function that spawns firebreath or whatever
                    BreathFire(playerDirection);
                    Debug.Log("Enemy Breathes Fire now");
                    attackWait = false;
                }
                else if (checkRange(player.gameObject.layer, out playerHit))
                { //Check if the player is in range
                    //Make enemy flash here
                    Debug.Log("Enemy should flash now");
                    attackWait = true;

                }
                else
                {
                    moveEnemy(); //If player is not in range and not waiting to attack then simply move
                }
                break;
        }



    }

    private void UpdateHealthBar()
    {

        if (stats.GetHealth() >= stats.GetMaxHealth())
        {
            enemyCanvas.SetActive(false);
        }
        else if (stats.GetHealth() < stats.GetMaxHealth() && !enemyCanvas.activeInHierarchy)
        {
            enemyCanvas.SetActive(true);
        }

        healthBarCurrWidth = MathG.Mapping.Map(stats.GetHealth(), 0f, stats.GetMaxHealth(), 0, healthBarMaxWidth);

        enemyHealthBar.sizeDelta = new Vector2(healthBarCurrWidth, 0.15f);
    }


    private bool checkRange(LayerMask target, out RaycastHit2D[] checkForPlayer)
    {

        //Use raycasts along the four cardinal directions and check the player layer mask
        //Use raycast all to detect walls if any walls are detected then return false
        //If hit return true

        bool playerDetected = false;
        Vector2 detectedVector = new Vector2(0, 0);
        
        //Debug.Log("Checking Range");
        //Debug.Log("Target = " + target.value);
        

        Vector2 direction;

        Vector2 checkStart, checkEnd;

        checkStart = this.transform.position;

        enemyBoxCollider.enabled = false;
        
        
        //Up
        direction = new Vector2(0, stats.atkRange * Constants.GRIDSIZE);
        Debug.DrawRay(this.transform.position, direction, Color.red, 4.0f);
        checkEnd = checkStart + direction;
        checkForPlayer = Physics2D.LinecastAll(checkStart, checkEnd);
        //Check if player is in the linecast array here
        if(checkForPlayer.Length != 0 && CheckLineCastArray(checkForPlayer)){
            Debug.Log("Player detected Up!");
			playerDetected = true;
            playerDirection = 0;
            
		}
        
        //Right
        direction = new Vector2(stats.atkRange * Constants.GRIDSIZE, 0);
        Debug.DrawRay(this.transform.position, direction, Color.red, 4.0f);
        checkEnd = checkStart + direction;
        checkForPlayer = Physics2D.LinecastAll(checkStart, checkEnd);
        //Check if player is in the linecast array here
        if(checkForPlayer.Length != 0 && CheckLineCastArray(checkForPlayer)){
            Debug.Log("Player detected Right!");
			playerDetected = true;
            playerDirection = 1;
            
		}

        //Down
        direction = new Vector2(0, -stats.atkRange * Constants.GRIDSIZE);
        Debug.DrawRay(this.transform.position, direction, Color.red, 4.0f);
        checkEnd = checkStart + direction;
        checkForPlayer = Physics2D.LinecastAll(checkStart, checkEnd);
        //Check if player is in the linecast array here
        if(checkForPlayer.Length != 0 && CheckLineCastArray(checkForPlayer)){
            Debug.Log("Player detected Down!");
			playerDetected = true;
            playerDirection = 2;
            
		}

        //Left
        direction = new Vector2(-stats.atkRange * Constants.GRIDSIZE, 0);
        Debug.DrawRay(this.transform.position, direction, Color.red, 4.0f);
        checkEnd = checkStart + direction;
        checkForPlayer = Physics2D.LinecastAll(checkStart, checkEnd);
        //Check if player is in the linecast array here
        if(checkForPlayer.Length != 0 && CheckLineCastArray(checkForPlayer)){
            Debug.Log("Player detected Left!");
            Debug.Log(checkForPlayer[0].transform.gameObject.layer);
			playerDetected = true;
            playerDirection = 3;
            
		}

        enemyBoxCollider.enabled = true;

        if(playerDetected){
            Debug.Log("PLAYER FOUND");
            return true;
        }
        

        return false;

    }

    private void BreathFire(int dir){
        
        Vector3 distance = new Vector3(0,0);

        switch(dir){

            case 0:
                distance = new Vector3(0, 1 * Constants.GRIDSIZE, 0);

            break;

            case 1:
                distance = new Vector3(1 * Constants.GRIDSIZE, 0, 0);

            break;

            case 2:
                distance = new Vector3(0, -1 * Constants.GRIDSIZE, 0);

            break;

            case 3:
                distance = new Vector3(-1 * Constants.GRIDSIZE, 0, 0);

            break;

        }

        for(int i = 1; i < stats.atkRange+1; i++){
            GameObject newFire = Instantiate(fire, this.transform);
            newFire.transform.position += distance * i;
        }
        return;

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

    private bool CheckLineCastArray(RaycastHit2D[] array){

        for(int i = 0; i < array.Length; i++){
            if(array[i].transform.gameObject.layer == wallLayer){
                return false;
            }
            if(array[i].transform.gameObject.layer == player.layer){
                return true;
            }
        }

        return false;
    }
}
