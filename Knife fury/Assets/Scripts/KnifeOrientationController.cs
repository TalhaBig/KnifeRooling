using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeOrientationController : MonoBehaviour
{
    private Rigidbody2D knifeRigid;
    private bool isCollidingWithTrunk = false;
    private Vector2 trunkMovementDirection;

    void Start()
    {
        knifeRigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isCollidingWithTrunk)
        {
            AdjustKnifeOrientation();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Trunk"))
        {
            isCollidingWithTrunk = true;
            // Get the trunk's movement direction
            TrunkMovementController trunkController = collider.GetComponent<TrunkMovementController>();
            if (trunkController != null)
            {
                trunkMovementDirection = trunkController.GetMovementDirection();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Trunk"))
        {
            isCollidingWithTrunk = false;
        }
    }

    private void AdjustKnifeOrientation()
    {
        // Calculate the perpendicular direction to the trunk's movement
        Vector2 perpendicularDirection = new Vector2(-trunkMovementDirection.y, trunkMovementDirection.x).normalized;

        // Set the knife's rotation to align with this perpendicular direction
        float angle = Mathf.Atan2(perpendicularDirection.y, perpendicularDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
