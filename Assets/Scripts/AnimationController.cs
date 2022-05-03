using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    float timer;
    float lastClickedTime = 0f;

    public int numOfClicks = 0;
    public float maxComboDelay = 0.9f;

    bool isSprinting;
    bool isWalking;
    bool isWalkingBack;
    bool isWalkingRight;
    bool isWalkingLeft;
    bool isBlocking;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        combat();
        handleIdleSwing();
    }

    void movement() {
        isSprinting = animator.GetBool("isSprinting");
        isWalking = animator.GetBool("isWalking");
        isWalkingBack = animator.GetBool("isWalkingBack");
        isWalkingRight = animator.GetBool("isWalkingRight");
        isWalkingLeft = animator.GetBool("isWalkingLeft");
        isBlocking = animator.GetBool("isBlocking");

        bool walkPressed = Input.GetKey("w");
        bool sprintPressed = Input.GetKey("left shift");
        bool walkBackPressed = Input.GetKey("s");
        bool walkRightPressed = Input.GetKey("d");
        bool walkLeftPressed = Input.GetKey("a");
        // if player presses W key
        if (!isWalking && walkPressed) {
            animator.SetBool("isWalking", true);
        }
        // if player is not pressing W key
        if (isWalking && !walkPressed) {
            animator.SetBool("isWalking", false);
            timer = 0f;
        }
        // if player is walking and presses left shift
        if (!isSprinting && (walkPressed && sprintPressed)) {
            animator.SetBool("isSprinting", true);
        }
        //if player stops running or stops walking
        if (isSprinting && (!walkPressed || !sprintPressed)) {
            animator.SetBool("isSprinting", false);
        }
        // if player is pressing s
        if (!isWalkingBack && walkBackPressed) {
            animator.SetBool("isWalkingBack", true);
            timer = 0f;
        }
        // if player is not pressing s
        if (isWalkingBack && !walkBackPressed) {
            animator.SetBool("isWalkingBack", false);
        }
        if(!isWalkingRight && walkRightPressed) {
            animator.SetBool("isWalkingRight", true);
            timer = 0f;
        }
        if(isWalkingRight && !walkRightPressed) {
            animator.SetBool("isWalkingRight", false);
        }
        if(!isWalkingLeft && walkLeftPressed) {
            animator.SetBool("isWalkingLeft", true);
            timer = 0f;
        }
        if(isWalkingLeft && !walkLeftPressed) {
            animator.SetBool("isWalkingLeft", false);
        }
    }

    //Sets a timer and triggers idleSwing animation when timer reaches limit
    void handleIdleSwing() {
        float limit = 10f;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {    
            timer += Time.deltaTime;
            if (timer >= limit) {
                animator.SetTrigger("idleSwing");
                timer = 0f;
            }
        }
    }

    void combat() {
        bool blockPressed = Input.GetMouseButton(1);
        bool blockUnpressed = Input.GetMouseButtonUp(1);
        bool walkPressed = Input.GetKey("w");
        bool walkBackPressed = Input.GetKey("s");

         // if player is holding down right click
        if (blockPressed) {
            animator.SetBool("isBlocking", true);
            animator.SetLayerWeight(1, 1);
            timer = 0f;
        }
        // if player releases right click
        if (blockUnpressed) {
            animator.SetBool("isBlocking", false);
            animator.SetLayerWeight(1, 0);
        }

        if ((walkPressed || walkBackPressed) && blockPressed) {
            animator.SetBool("isBlocking", true);
            animator.SetLayerWeight(1, 1);
        }
        if ((walkPressed || walkBackPressed) && blockUnpressed) {
            animator.SetBool("isBlocking", false);
            animator.SetLayerWeight(1, 0);
        }
        if (Input.GetMouseButton(0)) {
            lastClickedTime = Time.time;
            numOfClicks++;
            if (numOfClicks == 1) {
                animator.SetBool("attack1", true);
            }
            numOfClicks = Mathf.Clamp(numOfClicks, 0, 3);
            timer = 0f;
        }
    }

    public void return1() {
        if (numOfClicks >= 2) {
            animator.SetBool("attack2", true);
        }
        else {
            animator.SetBool("attack1", false);
            numOfClicks = 0;
        }
    }

    public void return2() {
        if (numOfClicks >= 3) {
            animator.SetBool("attack3", true);
        }
        else {
            animator.SetBool("attack2", false);
            numOfClicks = 0;
        }
    }

    public void return3() {
        animator.SetBool("attack1", false);
        animator.SetBool("attack2", false);
        animator.SetBool("attack3", false);
        numOfClicks = 0;
    }

    public bool getBlockingState() {
        return this.isBlocking;
    }
}
