using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform target;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float stoppingDistance = 0.5f;
    
    void Update()
    {
        if (target != null)
        {
            MoveAndRotateTowardsTarget();
        }
    }

    public  void MoveAndRotateTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        //direction.y = 0f;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget > stoppingDistance)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
