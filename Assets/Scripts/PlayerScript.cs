using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    public int speed = 300;
    public LayerMask layerMask;
    private Rigidbody playerRB;
    public bool isMoving = false;
    public bool isTouchingStickyBlock;
    private RaycastHit hit;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            playerRB.isKinematic = false;
            playerRB.useGravity = true;
            if (isTouchingStickyBlock)
            {
                playerRB.isKinematic = true;
                playerRB.useGravity = false;
            }
        }
        else
        {
            playerRB.isKinematic = true;
            playerRB.useGravity = false;
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
        Debug.DrawRay(transform.position, direction, Color.green);
        if (Physics.Raycast(transform.position, direction, out hit, 1, layerMask))
        {
            //stop motion if wall in the way
            if (hit.collider.gameObject.tag == "StickyGround")
            {
                isTouchingStickyBlock = true;
                StartCoroutine(Roll(direction, true));
            }
            else
            {
                isTouchingStickyBlock = false;
            }
        }
        //if hit is null, just go
        else
        {
            isTouchingStickyBlock = false;
            StartCoroutine(Roll(direction, false));
        }
    }

    IEnumerator Roll(Vector3 direction, bool Up)
    {
        isMoving = true;
        Vector3 rotationCenter;
        float remainingAngle = 90;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);
        if (Up)
        {
            rotationCenter = transform.position + direction / 2 + Vector3.up / 2;
        }
        else
        {
            rotationCenter = transform.position + direction / 2 + Vector3.down / 2;
        }

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }

        //roll on top of sticky blocks at the top of stack
        if (Up && !Physics.Raycast(transform.position, direction, out hit, 1, layerMask))
        {
            StartCoroutine(Roll(direction, false));
        }

        //fix for imperfect movement
        transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            Mathf.Round(transform.position.z)
        );
        transform.eulerAngles = new Vector3(0, 0, 0);
        isMoving = false;
    }
}
