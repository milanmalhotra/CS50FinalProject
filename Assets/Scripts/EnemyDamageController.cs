using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageController : MonoBehaviour
{
    public ThirdPersonController tpc;
    public AnimationController animController;
    
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && !animController.getBlockingState()) {
            tpc.TakeDamage(50);
        }
    }
}
