using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Shop
{
    public class HealZone : MonoBehaviour
    {
        private bool playerInHealZone;
        private Player player;
        private PlayerInputActions inputActions;
        public GameObject interactionPopup;

        private void Awake()
        {
            inputActions = new();
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.CompareTag("Player")) return;

            playerInHealZone = true;
            player = collider.GetComponent<Player>();
            interactionPopup.SetActive(true);
            print("Player in heal zone.");
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (!collider.CompareTag("Player")) return;

            interactionPopup.SetActive(false);
            playerInHealZone = false;
            player = null;
        }

        private void Update()
        {
            if (!playerInHealZone) return;

            // Checking for interact key when within the shop zone
            if (inputActions.Player.Interact.WasPressedThisFrame())
            {
                player.HealPlayer(50.0f);
                interactionPopup.SetActive(false);
                // Clearing the room after healing 
                if (Management_Rooms.Instance.CurrentRoom != null) Management_Rooms.Instance.CurrentRoom.ForceClearRoom();
            }
        }
    }
}