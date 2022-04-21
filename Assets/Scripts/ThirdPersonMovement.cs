using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController characterController;
    public float walkSpeed;
    public float sprintSpeed;
    float speed;
    Vector3 camRotation;
    Transform cam;
    Vector3 moveDirection;

    [Range(-45, 15)]
    public int minAngle = -30;
    [Range(10, 80)]
    public int maxAngle = 45;
    [Range(50, 500)]
    public int sensitivity = 200;
    CursorLockMode lockMode;


    void Awake()
    {
        cam = Camera.main.transform;
        lockMode = CursorLockMode.Locked;
        Cursor.lockState = lockMode;
    }

    void Update()
    {
        Move();
        Rotate();
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up * sensitivity * Time.deltaTime * Input.GetAxis("Mouse X"));
        camRotation.x -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        camRotation.x = Mathf.Clamp(camRotation.x, minAngle, maxAngle);

        cam.localEulerAngles = camRotation;
    }

    void Move()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(horizontalMove, 0, verticalMove);
            moveDirection = transform.TransformDirection(moveDirection);
        }

        moveDirection.y -= Time.deltaTime;
        if (Input.GetKey("w") && Input.GetKey("left shift"))
        {
            speed = sprintSpeed;

        }
        if (Input.GetKey("w") && !Input.GetKey("left shift"))
        {
            speed = walkSpeed;
        }

        characterController.Move(moveDirection * speed * Time.deltaTime);
    }
}
