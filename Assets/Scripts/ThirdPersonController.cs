using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController characterController;
    public GameEnding gameEnding;
    public CanvasGroup youDied;
    public Animator animator;
    public float walkSpeed;
    public float sprintSpeed;
    public int health;
    float speed;
    bool isDead = false;
    Vector3 camRotation;
    Transform cam;
    Vector3 moveDirection;

    [Range(-45, 15)]
    public int minAngle = -30;
    [Range(10, 80)]
    public int maxAngle = 45;
    [Range(50, 500)]
    public int sensitivity = 200;


    void Awake()
    {
        cam = Camera.main.transform;
        Cursor.visible = false;
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Move();
        Rotate();
    }

    void Rotate()
    {
        if (!isDead) {
            transform.Rotate(Vector3.up * sensitivity * Time.deltaTime * Input.GetAxis("Mouse X"));
            camRotation.x -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            camRotation.x = Mathf.Clamp(camRotation.x, minAngle, maxAngle);

            cam.localEulerAngles = camRotation;
        }
    }

    void Move()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        if (characterController.isGrounded && !isDead)
        {
            moveDirection = new Vector3(horizontalMove, 0, verticalMove);
            moveDirection = transform.TransformDirection(moveDirection);
        }

        moveDirection.y -= Time.deltaTime;
        if (Input.GetKey("w") && Input.GetKey("left shift") && (!Input.GetKey("a") || !Input.GetKey("s") || !Input.GetKey("d")))
        {
            speed = sprintSpeed;

        }
        else
        {
            speed = walkSpeed;
        }

        // if (!isDead)
        characterController.Move(moveDirection * speed * Time.deltaTime);
    }

    public void TakeDamage(int damage) {
        health -= damage;

        if (health <= 0)
            DestoryPlayer();
    }

    void DestoryPlayer() {
        isDead = true;
        animator.SetLayerWeight(1, 0);
        animator.SetLayerWeight(2, 0);
        animator.SetLayerWeight(3, 0);
        animator.SetTrigger("isDead");
        StartCoroutine(timeBeforeEnding());
    }

    IEnumerator timeBeforeEnding() {
        yield return new WaitForSeconds(1f);
        gameEnding.ShowUI(youDied);
    }
}
