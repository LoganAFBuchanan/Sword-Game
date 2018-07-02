using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreation : MonoBehaviour
{

    //The MapCreator initializes the map and sets the parameters for that map (ie: size and boundaries)
    //It also facilitates the random walkers which generate the rooms and hallways, it holds the functions which create individual rooms and passages

    public GameObject Wall;
    public GameObject Player;
    public GameObject MainWalker;
    public GameObject Enemy;
    public GameObject Gold;
    public GameObject Tile;
    public GameObject Exit;
    public GameObject Fog;

    public GameObject EnemyList;
    public GameObject GoldList;
    public GameObject FogList;

    private Transform mapTransform;

    public int mapWidth;
    public int mapHeight;

    private bool generatingFlag;

    // Use this for initialization
    void Awake()
    {
        mapTransform = GetComponent<Transform>();
        generatingFlag = false;
    }

    void Start()
    {
        InitializeMap();
        MainWalker.GetComponent<WalkerController>().DrawPaths();
        AstarPath.active.Scan();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("ResetMap"))
        {
            ResetMap();
            
        }

    }

    public void ResetMap()
    {

        if (generatingFlag == false)
        {
            generatingFlag = true;
            ClearMap();
            InitializeMap();
            MainWalker.GetComponent<WalkerController>().DrawPaths();
            AstarPath.active.Scan();
            generatingFlag = false;
        }
    }

    private void InitializeMap()
    {
        //Build initialmap of walls
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {

                GameObject newWall = GameObject.Instantiate(Wall, mapTransform);
                GameObject newTile = GameObject.Instantiate(Tile, mapTransform);
                GameObject newFog = GameObject.Instantiate(Fog, FogList.transform);

                newWall.transform.position = new Vector3(x, y, 0);
                newTile.transform.position = new Vector3(x, y, 0);
                newFog.transform.position = new Vector3(x, y, 0);

            }
        }
        //Set MainWalker to the middle of the map
        MainWalker.transform.position = new Vector3(Mathf.RoundToInt(mapWidth / 2), Mathf.RoundToInt(mapHeight / 2), 0);

    }

    private void ClearMap()
    {
        foreach (Transform child in mapTransform)
        {
            if (child.gameObject.layer == 8 || child.gameObject.layer == 12 || child.gameObject.layer == 13)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        foreach (Transform child in MainWalker.transform)
        {
            if (child.gameObject.layer == 12)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        foreach (Transform child in EnemyList.transform)
        {

            GameObject.Destroy(child.gameObject);

        }

        foreach (Transform child in GoldList.transform){
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in FogList.transform){
            GameObject.Destroy(child.gameObject);
        }

        MainWalker.GetComponent<WalkerController>().InitializeValues();
    }

    public void ClearWall(Transform walkerPos)
    {
        foreach (Transform child in mapTransform)
        {

            if (child.transform.position.Equals(walkerPos.position) && child.gameObject.layer == 8)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public void ClearRoom(Transform walkerPos, int size)
    {

        foreach (Transform child in mapTransform)
        {
            if (child.transform.position.x >= walkerPos.position.x - size
            && child.transform.position.x <= walkerPos.position.x + size
            && child.transform.position.y >= walkerPos.position.y - size
            && child.transform.position.y <= walkerPos.position.y + size
            && child.gameObject.layer == 8)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public void SetMarker(Transform walkerPos)
    {
        GameObject.Instantiate(Tile, walkerPos);
    }

    public void PlaceEnemy(Transform walkerPos)
    {
        GameObject newEnemy = GameObject.Instantiate(Enemy, walkerPos);
        newEnemy.transform.parent = EnemyList.transform;
    }

    public void PlaceGold(Transform walkerPos, int goldTier)
    {
        GameObject newGold = GameObject.Instantiate(Gold, walkerPos);
        newGold.transform.parent = GoldList.transform;
        newGold.GetComponent<GoldController>().SetValTier(goldTier);

    }

    public void PlaceExit(Transform walkerPos)
    {
        Exit.transform.position = walkerPos.position;

    }
}
