using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DamageableTarget Target;
    [SerializeField] private Image HealthBarFill;

    [Header("Smooth Settings")]
    [SerializeField] private float FillChangeSpeed = 5f;

    private float TargetFillAmount = 1f;

    private void OnEnable()
    {
        Target.OnHealthChanged += UpdateTargetFillAmount;
    }

    private void OnDisable()
    {
        Target.OnHealthChanged -= UpdateTargetFillAmount;
    }

    private void Start()
    {
        HealthBarFill.fillAmount = 1f;
        TargetFillAmount = 1f;
    }

    private void Update()
    {
        HealthBarFill.fillAmount = Mathf.Lerp(
            HealthBarFill.fillAmount,
            TargetFillAmount,
            FillChangeSpeed * Time.deltaTime
        );
    }

    private void UpdateTargetFillAmount(int CurrentHealth, int MaxHealth)
    {
        TargetFillAmount = (float)CurrentHealth / MaxHealth;
    }
}