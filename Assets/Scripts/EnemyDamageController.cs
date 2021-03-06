using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageController : MonoBehaviour
{
    public ThirdPersonController tpc;
    public AnimationController animController;
    public Healthbar healthbar;
    public int damage;
    
    void Start() {
        healthbar.SetHealthbarColor();
    }
    
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && !animController.getBlockingState()) {
            tpc.TakeDamage(damage);
            healthbar.SetHealth(tpc.health);
        }
    }
}
