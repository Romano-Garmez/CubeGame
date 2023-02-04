using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int speed = 300;
    public LayerMask layerMask;
    private Rigidbody playerRB;
    private bool isMoving = false;

    //for debugging
    private Vector3 rayDirection;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //debug for testing raycasts
        //Debug.DrawRay(transform.position, rayDirection, Color.green);

        if (isMoving)
        {
            return;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            checkRoll(Vector3.right);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            checkRoll(Vector3.left);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            checkRoll(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            checkRoll(Vector3.back);
        }
    }

    void checkRoll(Vector3 direction)
    {
        RaycastHit hit;
        //for debugging
        //rayDirection = direction;

        if (Physics.Raycast(transform.position, direction, out hit, 1, layerMask))
        {
            //stop motion if wall in the way
            if (hit.collider.gameObject.tag == "Tile")
            {
                //Debug.Log("Unable to move");
            }
            else
            {
                StartCoroutine(Roll(direction));
            }
        }
        //if hit is null, just go
        else
        {
            StartCoroutine(Roll(direction));
        }
    }

    IEnumerator Roll(Vector3 direction)
    {
        //Debug.Log("Able to move");
        isMoving = true;
        playerRB.isKinematic = true;
        float remainingAngle = 90;
        Vector3 rotationCenter = transform.position + direction / 2 + Vector3.down / 2;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }
        playerRB.isKinematic = false;
        isMoving = false;
    }
}
