using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public EnemyController enemyController;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy") {
            enemyController.TakeDamage(50);
        }
    }
}
