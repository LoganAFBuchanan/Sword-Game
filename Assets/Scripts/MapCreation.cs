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

    private Transform mapTransform;

    public int mapWidth;
    public int mapHeight;

    // Use this for initialization
    void Awake()
    {
        mapTransform = GetComponent<Transform>();

        InitializeMap();
        MainWalker.GetComponent<WalkerController>().DrawPaths();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("ResetMap"))
        {
            ClearMap();
            InitializeMap();
            MainWalker.GetComponent<WalkerController>().DrawPaths();
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

                newWall.transform.position = new Vector3(x, y, 0);

            }
        }
        //Set MainWalker to the middle of the map
        MainWalker.transform.position = new Vector3(Mathf.RoundToInt(mapWidth / 2), Mathf.RoundToInt(mapHeight / 2));

    }

    private void ClearMap()
    {
        foreach (Transform child in mapTransform)
        {
            if (child.gameObject.layer == 8)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
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
}
