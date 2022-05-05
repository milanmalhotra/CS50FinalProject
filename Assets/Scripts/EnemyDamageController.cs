using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageController : MonoBehaviour
{
    public ThirdPersonController tpc;
    public AnimationController animController;
    public int damage;
    
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && !animController.getBlockingState()) {
            tpc.TakeDamage(damage);
        }
    }
}
