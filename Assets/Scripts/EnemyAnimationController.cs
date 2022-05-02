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
        Debug.Log(timeBetweenAttacks);
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
            animator.SetBool("isMoving", false);
            time = time + 1f * Time.deltaTime;
            if (time >= timeBetweenAttacks) {
                time = 0f;
                Attack();
            }
        }
    }

    float GenerateRandomNum() {
        float num = Random.Range(0f, 2f);
        return num;
    }

    void Attack() {
        float num = GenerateRandomNum();
        Debug.Log(num);
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