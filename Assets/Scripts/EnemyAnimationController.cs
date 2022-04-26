using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public Animator animator;
    public EnemyController enemyController;
    // Update is called once per frame
    void Update()
    {
        if (transform.hasChanged) {
            animator.SetBool("isMoving", true);
        }
        if (enemyController.playerInAttackRange) {
            animator.SetBool("isMoving", false);
        }
    }
}
