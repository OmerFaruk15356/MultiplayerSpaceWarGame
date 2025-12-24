using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShipControl : MonoBehaviourPun
{
    [SerializeField] ShipFire shipFire;
    [SerializeField] SetShip setShip;
    private Camera mainCamera;
    private Vector2 moveInput;
    public float currentSpeed = 0f;

    private void Start() {
        if (photonView.IsMine)
        {
            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
            cameraFollow.SetTarget(transform);
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            Run();
            RotateWithMouse();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }

    private void Run()
    {
        if (moveInput.y != 0)
        {
            float adjustedAcceleration = setShip.acceleration;


            if (Mathf.Abs(currentSpeed) < setShip.speed * 0.2f)
            {
                adjustedAcceleration *= 0.5f; 
            }

            currentSpeed += moveInput.y * adjustedAcceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, -setShip.speed / 2, setShip.speed);
        }
        else
        {
            if (currentSpeed > 1f)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, setShip.deceleration * Time.deltaTime);
            }
            else if (currentSpeed < -1f)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, setShip.deceleration * Time.deltaTime);
            }
        }
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);
    }

    private void RotateWithMouse()
    {
        if (mainCamera == null) return;

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 
        Vector2 direction = (Vector2)mousePosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));


        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);


        float adjustedRotationSpeed = setShip.rotationSpeed * (angleDifference / 60f);
        adjustedRotationSpeed = Mathf.Max(adjustedRotationSpeed, setShip.rotationSpeed * 0.75f); 

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, adjustedRotationSpeed * Time.deltaTime);
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            if(context.started)
                shipFire.isFiring = true;
            if(context.canceled)
                shipFire.isFiring = false;
        }
    }
    //Test
}
