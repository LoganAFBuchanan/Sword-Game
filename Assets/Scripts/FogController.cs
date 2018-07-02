using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{

    private Transform playerTransform;

    private Color spriteColor;

    public LayerMask wallLayer;

    // Use this for initialization
    void Awake()
    {
        spriteColor = GetComponent<SpriteRenderer>().color;
        playerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeOpacity()
    {


        if (Vector3.Distance(this.transform.position, playerTransform.position) >= playerTransform.gameObject.GetComponent<PlayerController>().stats.sightRange)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
			
        }
        else if (Vector3.Distance(this.transform.position, playerTransform.position) >= playerTransform.gameObject.GetComponent<PlayerController>().stats.sightRange - 1)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.66f);
			
        }
        else if (Vector3.Distance(this.transform.position, playerTransform.position) >= playerTransform.gameObject.GetComponent<PlayerController>().stats.sightRange - 2)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.33f);
			
        }
        else
        {

            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
			
        }
    }

    public int checkWalls()
    {
		RaycastHit2D[] objectHit = Physics2D.LinecastAll(this.transform.position, playerTransform.position, wallLayer);
        // if (objectHit.Length > 2)
        // {
        //     GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
        //     return;
        // }
        // else if (objectHit.Length > 1)
        // {
        //     GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.66f);
        //     return;
        // }

		return objectHit.Length;
    }
}
