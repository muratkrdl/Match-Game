using UnityEngine;

public class SetGoalMenu : MenuBaseClass
{
    protected override void OnCloseMyMenu()
    {
        GetComponentInChildren<CreateLevelButtonImage>().SetSpriteToZero();
    }
}
