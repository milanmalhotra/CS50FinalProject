using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationController : MonoBehaviour
{
    public Animator animator;
    public int numOfClicks = 0;
    public float maxComboDelay = 0.9f;
    float lastClickedTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastClickedTime > maxComboDelay) {
            numOfClicks = 0;
        }

        if (Input.GetMouseButtonDown(0)) {
            lastClickedTime = Time.time;
            numOfClicks++;
            if (numOfClicks == 1) {
                animator.SetBool("attack1", true);
            }
            numOfClicks = Mathf.Clamp(numOfClicks, 0, 3);
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
}
