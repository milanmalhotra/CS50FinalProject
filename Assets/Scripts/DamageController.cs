using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public EnemyController enemyController;
    public AnimationController animationController;
    public Healthbar healthbar;
    public int damage;
    public int currentHealth;

    void Start() {
        currentHealth = 100;
        healthbar.SetHealthbarColor();
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy" && !animationController.getBlockingState()) {
            enemyController.TakeDamage(damage);
            healthbar.SetHealth(enemyController.health);  
        }
    }
}
