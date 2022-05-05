using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public EnemyController enemyController;
    public AnimationController animationController;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy" && !animationController.getBlockingState()) {
            enemyController.TakeDamage(50);
        }
    }
}
