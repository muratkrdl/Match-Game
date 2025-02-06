using System;
using TMPro;
using UnityEngine;

public class CreateLevelMenuSetValues : MonoBehaviour
{
    [SerializeField] private CreateLevelButton rows;
    [SerializeField] private CreateLevelButton columns;
    [SerializeField] private CreateLevelButton colorAmount;
    [SerializeField] private CreateLevelButton conditionA;
    [SerializeField] private CreateLevelButton conditionB;
    [SerializeField] private CreateLevelButton conditionC;

    [SerializeField] private CreateLevelButtonImage choosedSprite;
    [SerializeField] private CreateLevelButtonSlider choosedScore;
    [SerializeField] private CreateLevelButtonSlider choosedMove;

    public void PublishOnClickStart()
    {
        MenuManager.Instance.OnClickPlay?.Invoke(new(columns.GetCurrentValue, rows.GetCurrentValue), 
        colorAmount.GetCurrentValue, choosedSprite.GetCurrentSprite, choosedScore.GetCurrentValue, choosedMove.GetCurrentValue, 
        new int[] 
        {  
            conditionA.GetCurrentValue,
            conditionB.GetCurrentValue,
            conditionC.GetCurrentValue,
        });
    }

    public void SetChoosedSpriteMaxValues()
    {
        choosedSprite.SetMaxRange = colorAmount.GetCurrentValue-1;
    }

}
