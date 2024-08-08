using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkMovementController : MonoBehaviour
{
    public float horizontalMoveSpeed = 2f; // Speed for horizontal movement
    public float horizontalRange = 5f; // The range within which the trunk moves left and right

    private bool movingRight = true;

    public Vector2 GetMovementDirection()
    {
        return movingRight ? Vector2.right : Vector2.left;
    }

    void Update()
    {
        // Move the trunk left and right
        if (movingRight)
        {
            transform.position += (Vector3.right * horizontalMoveSpeed * Time.deltaTime);
            if (transform.position.x >= horizontalRange)
                movingRight = false;
        }
        else
        {
            transform.position+= (Vector3.left * horizontalMoveSpeed * Time.deltaTime);
            if (transform.position.x <= -horizontalRange)
                movingRight = true;
        }
    }
}
