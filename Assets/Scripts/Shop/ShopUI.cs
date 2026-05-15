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
        // Removing the interaction popup 
    }

    // ----- Upgrades to purchase -----
    // Values here can be changed as needed
    public void BuyIncreaseMaxHealth()
    {
        if (currentPlayer == null) return;
        if (Score.Instance.score < 500)
        {
            SoundManager.Play(SoundType.SHOP_NO);
            return;
        }

        currentPlayer.SetMaxHealth(currentPlayer.playerActualHealth + 50.0f);
        // Also heals you 
        currentPlayer.HealPlayer(50.0f);

        if (DEBUG) print($"Upgrade purchased. New player max health: {currentPlayer.playerMaxHealth}");
        SoundManager.Play(SoundType.SHOP_YES);
        CloseShop();
        Score.Instance.UpdateScore(-500);
        // Purchasing from the shop clears the current room
        if (Management_Rooms.Instance.CurrentRoom != null) Management_Rooms.Instance.CurrentRoom.ForceClearRoom();
    }

    public void BuySpeedUpgrade()
    {
        if (currentPlayer == null) return;
        if (Score.Instance.score < 1_000)
        {
            SoundManager.Play(SoundType.SHOP_NO);
            return;
        }

        currentPlayer.moveSpeed += 2;
        if (DEBUG) print($"Upgrade purchased. New player speed: {currentPlayer.moveSpeed}");
        SoundManager.Play(SoundType.SHOP_YES);
        CloseShop();
        Score.Instance.UpdateScore(-1_000);
        if (Management_Rooms.Instance.CurrentRoom != null) Management_Rooms.Instance.CurrentRoom.ForceClearRoom();
    }

    public void BuyDamageUpgrade()
    {
        if (currentPlayer == null) return;
        if (Score.Instance.score < 750)
        {
            SoundManager.Play(SoundType.SHOP_NO);
            return;
        }

        currentPlayer.playerDamage += 50.0f;
        if (DEBUG) print($"Upgrade purchased. New player damage: {currentPlayer.playerDamage}");
        SoundManager.Play(SoundType.SHOP_YES);
        CloseShop();
        Score.Instance.UpdateScore(-750);
        if (Management_Rooms.Instance.CurrentRoom != null) Management_Rooms.Instance.CurrentRoom.ForceClearRoom();
    }

    public void BuyAttackSpeed()
    {
        if (currentPlayer == null) return;
        if (Score.Instance.score < 1_500)
        {
            SoundManager.Play(SoundType.SHOP_NO);
            return;
        }

        if (currentPlayer.attackCooldown >= 0) currentPlayer.attackCooldown -= 0.2f;
        if (DEBUG) print($"Upgrade purchased. New attack cooldown {currentPlayer.attackCooldown}");
        SoundManager.Play(SoundType.SHOP_YES);
        CloseShop();
        Score.Instance.UpdateScore(-1_500);
        if (Management_Rooms.Instance.CurrentRoom != null) Management_Rooms.Instance.CurrentRoom.ForceClearRoom();
    }


    public void LeaveShop()
    {
        SoundManager.Play(SoundType.SHOP_YES);
        CloseShop();
        if (Management_Rooms.Instance.CurrentRoom != null) Management_Rooms.Instance.CurrentRoom.ForceClearRoom();
    }
}
