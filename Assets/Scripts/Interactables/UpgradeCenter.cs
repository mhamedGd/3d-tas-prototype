using System;
using System.Runtime.InteropServices;
using FischlWorks_FogWar;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCenter : Interactable
{
    public GameObject GameObject => gameObject;

    [SerializeField] GameObject upgradeCanvas;

    [SerializeField] Player player;
    [SerializeField] UpgradeButton[] upgradeButtons;

    [SerializeField] SkillSet playerSkillSet;

    public float StoppingDistance => 4;

    [SerializeField] RectTransform descriptionRect;
    [SerializeField] float descriptionLeftMargin;

    private void Awake()
    {
        UpdateButtonsEnabled();
        Return();

        playerSkillSet.Init(player);
    }

    private void Update()
    {
        descriptionRect.anchoredPosition = Input.mousePosition + Vector3.right * (descriptionRect.rect.xMax + descriptionLeftMargin);
    }

    public override void Interact(Player _player)
    {
        Time.timeScale = 0;
        upgradeCanvas.SetActive(true);
        UpdateButtonsEnabled();
        UpdateCoinPrices();
        playerSkillSet.IterateBlocks((sb) => playerSkillSet.CheckParentButton(_player, sb));
        playerSkillSet.IterateBlocks((sb) => playerSkillSet.CheckChildrenButtons(_player, sb));
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
    public void IncreaseLightDiameter(Button _bt)
    {
        UpdateCoinCount(_bt);

        var fogs = FindObjectsByType<csFogWar>(FindObjectsSortMode.None);
        foreach(var f in fogs)
        {
            foreach(var r in f._FogRevealers)
            {
                if(r._RevealerTransform == player.transform)
                {
                    r.SetSightRange(r._SightRange + 1);
                }
            }
        }
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
        //UpdateCoinCount(_bt);

        player.SetSpeed(player.Speed + 2);
        UpdateCoins(_bt);
        //UpdateButtonsEnabled();
    }

    void UpdateCoins(Button _bt)
    {
        playerSkillSet.IterateBlocks((sb) =>
        {
            if(sb.parentButton.Button == _bt)
            {
                player.IncreaseCoinCount(-sb.parentButton.Cost);
            }
        });
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

    void UpdateCoinPrices()
    {
        foreach(var b in upgradeButtons)
        {
            b.CostText.text = b.Cost.ToString();
        }
    }
}

[Serializable]
public class UpgradeButton
{
    public Button Button;
    public int Cost;
    public TextMeshProUGUI CostText;
    [HideInInspector] public bool AlreadyPressed = false;

    public void Init()
    {
        Button.onClick.AddListener(() =>
        {
            AlreadyPressed = true;
        });
    }
}