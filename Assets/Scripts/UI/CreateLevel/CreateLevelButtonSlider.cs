using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateLevelButtonSlider : MonoBehaviour
{
    [SerializeField] private Slider mySlider;
    [SerializeField] private TextMeshProUGUI valueText;

    [SerializeField] private int startValue;

    private int currentValue;

    public int GetCurrentValue
    {
        get => currentValue;
    }

    private void Start() 
    {
        ValueChanged();
    }

    public void ValueChanged()
    {
        currentValue = (int)mySlider.value;
        valueText.text = currentValue.ToString();
        SoundManager.Instance.PlaySound2DVolume(ConstStrings.SFX_SLIDER, .1f);
    }

}
