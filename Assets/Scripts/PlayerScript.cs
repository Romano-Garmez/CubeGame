using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int speed = 300;
    private Rigidbody playerRB;
    private Vector3 rayDirection;
    private bool isMoving = false;
    public LayerMask layerMask;
    private RaycastHit hit;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, rayDirection, Color.green);
        if (isMoving)
        {
            return;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            StartCoroutine(Roll(Vector3.right));
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            StartCoroutine(Roll(Vector3.left));
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            StartCoroutine(Roll(Vector3.forward));
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            StartCoroutine(Roll(Vector3.back));
        }
    }

    IEnumerator Roll(Vector3 direction)
    {
        isMoving = true;

        playerRB.isKinematic = true;
        float remainingAngle = 90;
        Vector3 rotationCenter = transform.position + direction / 2 + Vector3.down / 2;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);
        rayDirection = transform.position + direction;
        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }
        playerRB.isKinematic = false;
        isMoving = false;
        // transform.position = new Vector3(
        //     Mathf.Round(transform.position.x),
        //     Mathf.Round(transform.position.y),
        //     Mathf.Round(transform.position.z)
        // );
    }
}
