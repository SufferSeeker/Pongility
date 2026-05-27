using UnityEngine;


public class AbilityLifetime : MonoBehaviour
{
    #region Variables
    [Header("Lifetime Settings")]
    [SerializeField] private float ActiveLifeTime = 6f;

    [Header("State")]
    [SerializeField] private float CurrentLifeTime;
    [SerializeField] private bool IsLifetimeFrozen;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        CurrentLifeTime = 0f;
        IsLifetimeFrozen = false;
    }

    private void OnEnable()
    {
        MatchManager.OnRoundEndFreezeStarted += FreezeLifetime;
        MatchManager.OnRoundCleanupRequested += CleanupForRoundReset;
        MatchManager.OnMatchEnded += FreezeLifetime;
    }

    private void OnDisable()
    {
        MatchManager.OnRoundEndFreezeStarted -= FreezeLifetime;
        MatchManager.OnRoundCleanupRequested -= CleanupForRoundReset;
        MatchManager.OnMatchEnded -= FreezeLifetime;
    }

    private void Update()
    {
        if (IsLifetimeFrozen == true) return;
        if (ActiveLifeTime <= 0f) return;

        CurrentLifeTime += Time.deltaTime;

        if (CurrentLifeTime >= ActiveLifeTime)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Initialization
    public void Initialize(float NewActiveLifeTime)
    {
        ActiveLifeTime = NewActiveLifeTime;
        CurrentLifeTime = 0f;
        IsLifetimeFrozen = false;
    }
    #endregion

    #region Round Events
    private void FreezeLifetime()
    {
        IsLifetimeFrozen = true;
    }

    private void CleanupForRoundReset()
    {
        Destroy(gameObject);
    }
    #endregion
}