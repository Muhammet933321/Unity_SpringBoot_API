using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Vector3 targetPosition;
    private float desiredTimeToReach = 0.03f;
    private float moveSpeed;

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        //float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        //moveSpeed = distanceToTarget / desiredTimeToReach;


        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPosition, desiredTimeToReach);
    }
}
