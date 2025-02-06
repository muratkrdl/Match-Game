using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateLevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueText;

    [SerializeField] private Vector2Int range;

    [SerializeField] private int startValue;

    private int currentValue;

    public int GetCurrentValue
    {
        get => currentValue;
    }

    private void Start() 
    {
        currentValue = startValue;
        valueText.text = currentValue.ToString();
    }

    public void OnClick_DecreaseChangeNumber()
    {
        ChangeNumber(-1);
    }

    public void OnClick_IncreaseChangeNumber()
    {
        ChangeNumber(1);
    }

    private void ChangeNumber(int number)
    {
        if((number < 0 && currentValue == range.x) || (number > 0 && currentValue == range.y)) return;

        currentValue += number;
        valueText.text = currentValue.ToString();
    }
}
