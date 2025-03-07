using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCenter : Interactable
{
    public GameObject GameObject => gameObject;

    [SerializeField] GameObject upgradeCanvas;

    [SerializeField] Player player;
    [SerializeField] UpgradeButton[] upgradeButtons;

    public float StoppingDistance => 4;

    private void Start()
    {
        UpdateButtonsEnabled();
        Return();
    }

    public override void Interact(Player _player)
    {
        Time.timeScale = 0;
        upgradeCanvas.SetActive(true);
        UpdateButtonsEnabled();


    }

    public override void StopInteract(Player _player)
    {
        
    }

    void UpdateButtonsEnabled()
    {
        foreach (var button in upgradeButtons) {
            if (button.Cost > player.Coins) button.Button.interactable = false;
            else button.Button.interactable = true;
        }
    }
    public void Return()
    {
        Time.timeScale = 1;
        upgradeCanvas.SetActive(false);
    }

    public void IncreaseWaterCapacity(Button _bt)
    {
        UpdateCoinCount(_bt);

        //stats.IncreaseMaximumWater(5);
        UpdateButtonsEnabled();
    }
    public void IncreaseDamage(Button _bt)
    {
        UpdateCoinCount(_bt);
        //player.IncreaseDamage(30);
        UpdateButtonsEnabled();
    }

    public void IncreaseSpeed(Button _bt)
    {
        UpdateCoinCount(_bt);

        player.SetSpeed(player.Speed + 2);
        UpdateButtonsEnabled();
    }

    void UpdateCoinCount(Button _b)
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (upgradeButtons[i].Button == _b)
            {
                player.IncreaseCoinCount(-upgradeButtons[i].Cost);
                upgradeButtons[i].Cost += 1;
                upgradeButtons[i].CostText.text = upgradeButtons[i].Cost.ToString();
                break;
            }
        }
    }
}

[Serializable]
public struct UpgradeButton
{
    public Button Button;
    public int Cost;
    public TextMeshProUGUI CostText;
}