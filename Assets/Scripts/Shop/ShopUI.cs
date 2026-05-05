using UnityEngine;

// Need to tie this into the overall Management_Game FSM

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance;

    public GameObject shopPanel;
    private Player currentPlayer;

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
    }

    public void BuySpeedUpgrade()
    {
        if (currentPlayer == null) return;

        currentPlayer.moveSpeed += 2;
    }
}
