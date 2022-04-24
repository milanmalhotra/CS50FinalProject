using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isSprintingHash;
    int isWalkingBackHash;
    int idleSwingHash;
    int isBlockingHash;
    int isWalkingLeftHash;
    int isWalkingRightHash;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isSprintingHash = Animator.StringToHash("isSprinting");
        isWalkingBackHash = Animator.StringToHash("isWalkingBack");
        idleSwingHash = Animator.StringToHash("idleSwing");
        isWalkingLeftHash = Animator.StringToHash("isWalkingLeft");
        isWalkingRightHash = Animator.StringToHash("isWalkingRight");
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        bool isSprinting = animator.GetBool(isSprintingHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isWalkingBack = animator.GetBool(isWalkingBackHash);
        bool isWalkingRight = animator.GetBool(isWalkingRightHash);
        bool isWalkingLeft = animator.GetBool(isWalkingLeftHash);

        bool walkPressed = Input.GetKey("w");
        bool sprintPressed = Input.GetKey("left shift");
        bool walkBackPressed = Input.GetKey("s");
        bool walkRightPressed = Input.GetKey("d");
        bool walkLeftPressed = Input.GetKey("a");
        bool blockPressed = Input.GetMouseButton(1);
        bool blockUnpressed = Input.GetMouseButtonUp(1);

        handleIdleSwing();
        
        // if player presses W key
        if (!isWalking && walkPressed) {
            animator.SetBool(isWalkingHash, true);
        }
        // if player is not pressing W key
        if (isWalking && !walkPressed) {
            animator.SetBool(isWalkingHash, false);
            time = 0f;
        }
        // if player is walking and presses left shift
        if (!isSprinting && (walkPressed && sprintPressed)) {
            animator.SetBool(isSprintingHash, true);
        }
        //if player stops running or stops walking
        if (isSprinting && (!walkPressed || !sprintPressed)) {
            animator.SetBool(isSprintingHash, false);
        }
        // if player is pressing s
        if (!isWalkingBack && walkBackPressed) {
            animator.SetBool(isWalkingBackHash, true);
            time = 0f;
        }
        // if player is not pressing s
        if (isWalkingBack && !walkBackPressed) {
            animator.SetBool(isWalkingBackHash, false);
        }
        if(!isWalkingRight && walkRightPressed) {
            animator.SetBool(isWalkingRightHash, true);
            time = 0f;
        }
        if(isWalkingRight && !walkRightPressed) {
            animator.SetBool(isWalkingRightHash, false);
        }
        if(!isWalkingLeft && walkLeftPressed) {
            animator.SetBool(isWalkingLeftHash, true);
            time = 0f;
        }
        if(isWalkingLeft && !walkLeftPressed) {
            animator.SetBool(isWalkingLeftHash, false);
        }
        // if player is holding down right click
        if (blockPressed) {
            animator.SetBool("isBlocking", true);
            animator.SetLayerWeight(1, 1);
            time = 0f;
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
            time = 0f;
        }
    }

    void handleIdleSwing() {
        float threshold = 10f;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {    
            time += Time.deltaTime;
            if (time >= threshold) {
                animator.SetTrigger(idleSwingHash);
                time = 0f;
            }
        }
    }
}
