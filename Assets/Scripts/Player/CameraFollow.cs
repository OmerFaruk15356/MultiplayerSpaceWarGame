using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    [SerializeField] float followSpeed = 2f;
    [SerializeField] Vector3 offset;
    private Transform target;

    void Update()
    {
        if (target == null)
            return;

        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
