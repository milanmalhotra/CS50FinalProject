using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isSprintingHash;
    int isJumpingHash;
    int isWalkingBackHash;
    int idleSwingHash;
    int isBlockingHash;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isSprintingHash = Animator.StringToHash("isSprinting");
        isJumpingHash = Animator.StringToHash("isJumping");
        isWalkingBackHash = Animator.StringToHash("isWalkingBack");
        idleSwingHash = Animator.StringToHash("idleSwing");
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        bool isSprinting = animator.GetBool(isSprintingHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isJumping = animator.GetBool(isJumpingHash);
        bool isWalkingBack = animator.GetBool(isWalkingBackHash);

        bool walkPressed = Input.GetKey("w");
        bool sprintPressed = Input.GetKey("left shift");
        bool jumpPressed = Input.GetKey("space");
        bool walkBackPressed = Input.GetKey("s");
        bool blockPressed = Input.GetMouseButtonDown(1);
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
        // if player presses space
        if (!isJumping && jumpPressed) {
            animator.SetBool(isJumpingHash, true);
        }
        // if player is not pressing space
        if (isJumping && !jumpPressed) {
            animator.SetBool(isJumpingHash, false);
        }
        // if player is pressing s
        if (!isWalkingBack && walkBackPressed) {
            animator.SetBool(isWalkingBackHash, true);
        }
        // if player is not pressing s
        if (isWalkingBack && !walkBackPressed) {
            animator.SetBool(isWalkingBackHash, false);
        }
        // if player is holding down right click
        if (blockPressed) {
            animator.SetBool("isBlocking", true);
        }
        // if player releases right click
        if (blockUnpressed) {
            animator.SetBool("isBlocking", false);
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
