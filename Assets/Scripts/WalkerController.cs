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
    public GameObject Enemy;

    public enum WalkerType
    {
        Main,
        Child
    }

    public WalkerType type;

    private MapCreation TheGreatCreator;
    private int lifetime;
    private float childSpawnChance;

    private int previousDir;

    // Use this for initialization
    void Awake()
    {
        TheGreatCreator = GameObject.Find("Map_Creator").GetComponent<MapCreation>();

		//Sets initial main values
        if (type == WalkerType.Main)
        {
            SetLifetime(10);
			SetChildChance(0.3f);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

	//Primary functions for creating the dungeon floors
    public void DrawPaths()
    {
        int lifeCounter = 0;

        while (lifeCounter < lifetime)
        {
			
            TheGreatCreator.ClearWall(this.transform);

            int dir = Mathf.RoundToInt(Random.Range(0.0f, 3.0f)); //Choose Direction
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

            lifeCounter++;

			//Map creation actions for walker death
            if (lifeCounter == lifetime - 1)
            {
                if(type == WalkerType.Main) Player.transform.position = this.transform.position;

                TheGreatCreator.ClearRoom(this.transform, Constants.START_ROOM_SIZE);
            }
        }
    }

	private void InstantiateChildWalker(int life, float spawnChance){

		GameObject newChild = GameObject.Instantiate(ChildWalker, this.transform);
		WalkerController newChildScript = newChild.GetComponent<WalkerController>();

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
