using UnityEngine;

public class ShopZone : MonoBehaviour
{
    private bool playerInShopZone;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInShopZone = true;
        player = other.GetComponent<Player>();
        interactionPopup.SetActive(true);
        print("Player in zone");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        interactionPopup.SetActive(false);
        playerInShopZone = false;
        player = null; // Might explode
    }

    private void Update()
    {
        if (!playerInShopZone) return;

        // Checking for interact key when within the shop zone
        if (inputActions.Player.Interact.WasPressedThisFrame())
        {
            ShopUI.Instance.OpenShop(player);
            interactionPopup.SetActive(false);
        }
        print("Player hit shop button in zone");
    }
}
