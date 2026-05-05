using UnityEngine;

// Need to tie this into the overall Management_Game FSM

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance;

    public GameObject shopPanel;
    private Player currentPlayer;
    private const bool DEBUG = true;

    private void Awake()
    {
        Instance = this;
        shopPanel.SetActive(false);
    }

    public void OpenShop(Player player)
    {
        currentPlayer = player;
        shopPanel.SetActive(true);

        // Pausing while in the shop
        Time.timeScale = 0.0f;
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        // Unpausing
        Time.timeScale = 1.0f;
    }

    // ----- Upgrades to purchase -----
    // Values here can be changed as needed
    public void BuyIncreaseMaxHealth()
    {
        if (currentPlayer == null) return;

        currentPlayer.playerMaxHealth += 50.0f;
        if (DEBUG) print($"Upgrade purchased. New player max health: {currentPlayer.playerMaxHealth}");
        CloseShop();
        // Purchasing from the shop clears the current room
        if (Management_Rooms.Instance.CurrentRoom != null) Management_Rooms.Instance.CurrentRoom.ForceClearRoom();
    }

    public void BuySpeedUpgrade()
    {
        if (currentPlayer == null) return;

        currentPlayer.moveSpeed += 2;
        if (DEBUG) print($"Upgrade purchased. New player speed: {currentPlayer.moveSpeed}");
        CloseShop();
        if (Management_Rooms.Instance.CurrentRoom != null) Management_Rooms.Instance.CurrentRoom.ForceClearRoom();
    }

    public void BuyDamageUpgrade()
    {
        if (currentPlayer == null) return;

        currentPlayer.playerDamage += 50.0f;
        if (DEBUG) print($"Upgrade purchased. New player damage: {currentPlayer.playerDamage}");
        CloseShop();
        if (Management_Rooms.Instance.CurrentRoom != null) Management_Rooms.Instance.CurrentRoom.ForceClearRoom();
    }
}
