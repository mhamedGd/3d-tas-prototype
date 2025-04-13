using UnityEngine;

public class CB_GoodMorning : CB_Base
{
    DaysManager daysManager;
    public override void Start()
    {
        base.Start();
        daysManager = FindAnyObjectByType<DaysManager>();
    }

    public override void Act()
    {
        base.Act();
        daysManager.NewDay();
    }
}
