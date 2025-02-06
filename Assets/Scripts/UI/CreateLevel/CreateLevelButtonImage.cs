using UnityEngine;
using UnityEngine.UI;

public class CreateLevelButtonImage : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private Image valueImage;

    [SerializeField] private Vector2Int range;

    private int currentValue;

    public Sprite GetCurrentSprite
    {
        get => sprites[currentValue];
    }
    public int SetMaxRange
    {
        set => range.y = value;
    }

    private void Start() 
    {
        currentValue = 0;
        valueImage.sprite = sprites[currentValue];
    }

    public void OnClick_DecreaseChangeNumber()
    {
        ChangeNumber(-1);
    }

    public void OnClick_IncreaseChangeNumber()
    {
        ChangeNumber(1);
    }

    public void SetSpriteToZero()
    {
        currentValue = 0;
        valueImage.sprite = sprites[currentValue];
    }

    private void ChangeNumber(int number)
    {
        if((number < 0 && currentValue == range.x) || (number > 0 && currentValue == range.y)) return;

        currentValue += number;
        valueImage.sprite = sprites[currentValue];
    }
}
