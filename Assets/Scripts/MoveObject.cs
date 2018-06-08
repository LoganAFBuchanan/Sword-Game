using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {

	private enum Orientation
    {
        Horizontal,
        Vertical
    };

    private Orientation gridOrientation;
    [System.NonSerialized] public bool isMoving;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float t;

    private BoxCollider2D boxCollider;

    private MoveConfirmation moveConf;

    private GameObject playerSword;

    protected virtual void Awake(){

        boxCollider = GetComponent<BoxCollider2D>(); 

        moveConf = GetComponent<MoveConfirmation>();

        isMoving = false;

        gridOrientation = Orientation.Vertical;

    }

	public IEnumerator move(Transform transform, Vector2 dir)
    {
        isMoving = true;
        startPosition = transform.position;
        t = 0;

        if (gridOrientation == Orientation.Horizontal)
        {
            endPosition = new Vector3(startPosition.x + System.Math.Sign(dir.x) * Constants.GRIDSIZE,
                startPosition.y, startPosition.z + System.Math.Sign(dir.y) * Constants.GRIDSIZE);
        }
        else
        {
            endPosition = new Vector3(startPosition.x + System.Math.Sign(dir.x) * Constants.GRIDSIZE,
                startPosition.y + System.Math.Sign(dir.y) * Constants.GRIDSIZE, startPosition.z);
        }


        while (t < 1f)
        {
            t += Time.deltaTime * (Constants.MOVESPEED / Constants.GRIDSIZE) * Constants.MOVEFACTOR;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        isMoving = false;
        yield return 0;
    }
}
