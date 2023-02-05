using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    private Vector3 originalPosition;
    public Vector3 secondPosition;
    public float movementTime = 5.0f;
    public float waitTime = 3.0f;

    private Vector3 velocity;
    private Vector3 previous;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return StartCoroutine(Wait(5));
        originalPosition = transform.position;
        while (true)
        {
            yield return StartCoroutine(MoveObject(originalPosition, secondPosition, movementTime));
            yield return StartCoroutine(Wait(waitTime));

            yield return StartCoroutine(MoveObject(secondPosition, originalPosition, movementTime));
            yield return StartCoroutine(Wait(waitTime));
        }
    }

    IEnumerator MoveObject(Vector3 startPos, Vector3 endPos, float time)
    {
        var i = 0.0f;
        var rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    void Update()
    {
        velocity.x = (transform.position.x - previous.x) / Time.deltaTime;
        velocity.y = (transform.position.y - previous.y) / Time.deltaTime;
        velocity.z = (transform.position.z - previous.z) / Time.deltaTime;
        previous = transform.position;
    }

    public Vector3 getVelocity(){
        return velocity;
    }
}
