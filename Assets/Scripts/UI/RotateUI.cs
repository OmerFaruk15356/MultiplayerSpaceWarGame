using UnityEngine;

public class RotateUI : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    void Update()
    {
        gameObject.transform.rotation = Camera.main.transform.rotation;
        gameObject.transform.position = transform.parent.position + offset;
    }
}
