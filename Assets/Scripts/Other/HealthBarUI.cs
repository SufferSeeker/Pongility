using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    #region Variables
    [Header("Target Settings")]
    [SerializeField] private MatchSide TargetSide;

    [Header("References")]
    [SerializeField] private DamageableTarget Target;
    [SerializeField] private Image HealthBarFill;

    [Header("Smooth Settings")]
    [SerializeField] private float FillDuration = 0.35f;

    [Header("Runtime State")]
    [SerializeField] private Coroutine FillRoutine;
    #endregion

    #region Unity Methods
    private void Awake()
    {

        Target = FindTargetBySide();
        HealthBarFill = transform.Find("Health Bar Fill").GetComponent<Image>();

        HealthBarFill.fillAmount = 1f;
    }

    private void OnEnable()
    {
        Target.OnHealthChanged += UpdateTargetFillAmount;
    }

    private void OnDisable()
    {
        Target.OnHealthChanged -= UpdateTargetFillAmount;
    }
    #endregion

    #region Health Bar Logic
    private void UpdateTargetFillAmount(int CurrentHealth, int MaxHealth)
    {
        float NewTargetFillAmount = (float)CurrentHealth / MaxHealth;

        if (FillRoutine != null)
        {
            StopCoroutine(FillRoutine);
        }

        FillRoutine = StartCoroutine(UpdateHealthBarFillRoutine(NewTargetFillAmount));
    }

    private IEnumerator UpdateHealthBarFillRoutine(float NewTargetFillAmount)
    {
        float StartFillAmount = HealthBarFill.fillAmount;
        float ElapsedTime = 0f;

        while (ElapsedTime < FillDuration)
        {
            ElapsedTime += Time.deltaTime;

            float Progress = ElapsedTime / FillDuration;

            HealthBarFill.fillAmount = Mathf.Lerp(
                StartFillAmount,
                NewTargetFillAmount,
                Progress
            );

            yield return null;
        }

        HealthBarFill.fillAmount = NewTargetFillAmount;
        FillRoutine = null;
    }
    #endregion

    #region Target Setup
    private DamageableTarget FindTargetBySide()
    {
        DamageableTarget[] Targets = FindObjectsByType<DamageableTarget>(FindObjectsSortMode.None);

        for (int i = 0; i < Targets.Length; i++)
        {
            if (Targets[i].GetTargetSide() == TargetSide)
            {
                return Targets[i];
            }
        }

        return null;
    }
    #endregion
}