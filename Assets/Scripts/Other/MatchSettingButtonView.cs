using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MatchSettingButtonSprites
{
    public Sprite NormalSprite;
    public Sprite HighlightedSprite;
    public Sprite PressedSprite;
}

public class MatchSettingButtonView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button TargetButton;
    [SerializeField] private Image TargetImage;

    [Header("Button Sprites")]
    [SerializeField] private MatchSettingButtonSprites[] ButtonSprites;

    [Header("Current State")]
    [SerializeField] private int CurrentVisualIndex;

    private void Awake()
    {
        TargetButton = GetComponent<Button>();
        TargetImage = GetComponent<Image>();
    }

    private void Start()
    {
        ApplyVisual(CurrentVisualIndex);
    }

    public void ApplyVisual(int VisualIndex)
    {
        if (VisualIndex < 0)
        {
            Debug.LogWarning(gameObject.name + " için geçersiz button visual index: " + VisualIndex);
            return;
        }

        if (VisualIndex >= ButtonSprites.Length)
        {
            Debug.LogWarning(gameObject.name + " için geçersiz button visual index: " + VisualIndex);
            return;
        }

        CurrentVisualIndex = VisualIndex;

        ApplyButtonSprites(ButtonSprites[CurrentVisualIndex]);
    }

    public void SetVisualEnabled(bool IsEnabled)
    {
        Color CurrentColor = TargetImage.color;

        if (IsEnabled == true)
        {
            CurrentColor.a = 1f;
        }

        else
        {
            CurrentColor.a = 150f / 255f;
        }

        TargetImage.color = CurrentColor;
    }

    private void ApplyButtonSprites(MatchSettingButtonSprites SelectedButtonSprites)
    {
        TargetImage.sprite = SelectedButtonSprites.NormalSprite;

        SpriteState NewSpriteState = TargetButton.spriteState;

        NewSpriteState.highlightedSprite = SelectedButtonSprites.HighlightedSprite;
        NewSpriteState.pressedSprite = SelectedButtonSprites.PressedSprite;
        NewSpriteState.selectedSprite = SelectedButtonSprites.HighlightedSprite;

        TargetButton.spriteState = NewSpriteState;
    }
}