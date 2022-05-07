using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public Animator animator;
    public EnemyController enemyController;

    float time;
    float timeBetweenAttacks;

    void Start() {
        timeBetweenAttacks = enemyController.timeBetweenAttacks;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.hasChanged) {
            animator.SetBool("isMoving", true);
            animator.SetBool("attack1", false);
            animator.SetBool("attack2", false);
        }
        if (enemyController.playerInAttackRange) {
            Attack();
        }
    }

    void Attack() {
        animator.SetBool("isMoving", false);
        float num = Random.Range(0f, 2f);
        time = time + 1f * Time.deltaTime;

        if (time >= timeBetweenAttacks) {
            time = 0f;
            if (num > 1) {
                animator.SetBool("attack1", true);
                animator.SetBool("attack2", false);
            }
            else {
                animator.SetBool("attack1", false);
                animator.SetBool("attack2", true);
            }
        }
    }
}