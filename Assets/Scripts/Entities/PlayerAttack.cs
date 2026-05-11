using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class PlayerAttack : MonoBehaviour
    {
        // Need to grab reference to take methods from
        private Player playerScript;

        private void Start()
        {
            playerScript = GetComponentInParent<Player>();
        }

        // Wrapper method for Deal Damage
        public void TriggerAttackDamage()
        {
            if (playerScript != null) playerScript.DealDamage();
        }
    }
}