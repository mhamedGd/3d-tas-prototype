using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillSet : MonoBehaviour
{
    [SerializeField] SkillBlock[] skillBlocks;
    public void IterateBlocks(Action<SkillBlock> _whatToDo)
    {
        foreach (var sb in skillBlocks) _whatToDo.Invoke(sb);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(Player p)
    {
        for(int i = 0; i < skillBlocks.Length; i++)
        {
            var sb = skillBlocks[i];
            if (i == 0 && p.Coins < sb.parentButton.Cost)
            {
                CheckParentButton(p, sb);
            }
            CheckChildrenButtons(p, sb);

            sb.parentButton.Init();
            sb.parentButton.Button.onClick.AddListener(() =>
            {
                CheckParentButton(p, sb);
                CheckChildrenButtons(p, sb);
            });

            foreach(var cb in sb.childrenButtons)
            {
                cb.Init();
                cb.Button.onClick.AddListener(() =>
                {
                    CheckChildrenButtons(p, sb);
                });
            }
        }
    }

    public void CheckParentButton(Player p, SkillBlock sb)
    {
        sb.parentButton.Button.interactable = true;
        if (p.Coins < sb.parentButton.Cost || sb.parentButton.AlreadyPressed)
        {
            sb.parentButton.Button.interactable = false;
        }
    }

    public void CheckChildrenButtons(Player p, SkillBlock sb)
    {
        if (!sb.parentButton.AlreadyPressed)
        {
            foreach (var cb in sb.childrenButtons) cb.Button.interactable = false;
            return;
        }
        foreach(var cb in sb.childrenButtons)
        {
            if (cb.Cost > p.Coins || cb.AlreadyPressed) cb.Button.interactable = false;
            else cb.Button.interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class SkillBlock
{
    public UpgradeButton parentButton;
    public UpgradeButton[] childrenButtons;
}
