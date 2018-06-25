using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerController : MonoBehaviour
{

	//Random Walkers are the footmen that trudge about the fresh map
	//They call back to THEGREATCREATOR in order to enact their map creation whims upon their environment
	//The leader of the walkers is called MAIN! They are the progenitor of all CHILD walkers who bow down to MAIN.
	//CHILD walkers have reduced lifespans and can spawn their own CHILD walkers; but it's pretty unlikely.

    public GameObject ChildWalker;
    public GameObject Player;
    

    public enum WalkerType
    {
        Main,
        Child
    }

    public WalkerType type;

    private GameObject MapObject;
    private MapCreation TheGreatCreator;
    private int lifetime;
    private float childSpawnChance;

    private GameObject MainWalker;

    private int previousDir;

    // Use this for initialization
    void Awake()
    {

        MapObject = GameObject.Find("Map_Creator");
        TheGreatCreator = MapObject.GetComponent<MapCreation>();

		//Sets initial main values
        if (type == WalkerType.Main)
        {
            InitializeValues();
        }

        if (type == WalkerType.Child)
        {
            MainWalker = GameObject.Find("MainWalker");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeValues(){
        SetLifetime(35);
		SetChildChance(0.7f);
        
        
        
    }

	//Primary functions for creating the dungeon floors
    public void DrawPaths()
    {
        int lifeCounter = 0;
        if (type == WalkerType.Main)
        {
            Player.transform.position = this.transform.position;
        }
        

        while (lifeCounter <= lifetime)
        {
			
            TheGreatCreator.ClearWall(this.transform);

            int dir = Random.Range(0, 4); //Choose Direction
            Vector3 moveVector = new Vector3(0, 0, 0);

			//Assures that the walker doesn't move back and forth on one spot
            while (dir == previousDir)
            {
                Debug.Log("WHOOPS TRY AGAIN!");
                dir = Mathf.RoundToInt(Random.Range(0.0f, 3.0f));
            }

			/*
			0 = up
			1 = right
			2 = down
			3 = left
			*/
            switch (dir)
            {
                case 0:
                    previousDir = 2;
                    moveVector = new Vector3(0, 1, 0);
                    break;
                case 1:
                    previousDir = 3;
                    moveVector = new Vector3(1, 0, 0);
                    break;
                case 2:
                    previousDir = 0;
                    moveVector = new Vector3(0, -1, 0);
                    break;
                case 3:
                    previousDir = 1;
                    moveVector = new Vector3(-1, 0, 0);
                    break;
            }

            this.transform.position += moveVector;
            


            float spawnRoll = Random.Range(0.0f, 1.0f);

            if(spawnRoll <= childSpawnChance){

                int childLife = Mathf.RoundToInt((lifetime-lifeCounter) * 0.8f);
                float newChildChance = childSpawnChance * 0.2f;

                InstantiateChildWalker(childLife, newChildChance);

            }

            lifeCounter++;
            childSpawnChance *= 0.8f;

            // //TEST STUFF
            // if(type == WalkerType.Main) {
            //     Player.transform.position = this.transform.position;
            //     GameObject tileplaced = GameObject.Instantiate(Tile, this.transform);
            //     tileplaced.transform.SetParent(MapObject.transform);
            // }

			//Map creation actions for walker death
            if (lifeCounter == lifetime)
            {
                if(type == WalkerType.Main) {
                    TheGreatCreator.PlaceExit(this.transform);
                }
                if(type == WalkerType.Child) 
                {
                    
                    TheGreatCreator.PlaceEnemy(this.transform);
                }

                TheGreatCreator.ClearRoom(this.transform, Constants.START_ROOM_SIZE);
            }
        }
    }

	private void InstantiateChildWalker(int life, float spawnChance){

		GameObject newChild = GameObject.Instantiate(ChildWalker, this.transform);
		WalkerController newChildScript = newChild.GetComponent<WalkerController>();
        newChild.transform.position = this.transform.position;
        newChild.transform.SetParent(MapObject.transform);

		newChildScript.SetLifetime(life);
		newChildScript.SetChildChance(spawnChance);

		newChildScript.DrawPaths();

	}

	public void SetLifetime(int input){
		lifetime = input;
	}

	public void SetChildChance(float input){
		childSpawnChance = input;
	}


}
