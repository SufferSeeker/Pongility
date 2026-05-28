using System.Collections.Generic;
using UnityEngine;

public class AbilitySpawner : MonoBehaviour
{
    #region Variables
    [Header("Core References")]
    [SerializeField] private SelectedMatchSettings SelectedMatchSettings;
    [SerializeField] private GameObject AbilityPickupPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private string SpawnPointsParentName = "Ability Spawn Points";
    [SerializeField] private float InitialSpawnDelay = 5f;
    [SerializeField] private float SpawnInterval = 10f;
    [SerializeField] private int MaxActivePickups = 3;
    [SerializeField] private List<AbilityDefinition> SpawnableAbilities = new List<AbilityDefinition>();

    [Header("Runtime State")]
    [SerializeField] private bool IsSpawnerEnabled;
    [SerializeField] private bool CanSpawn;
    [SerializeField] private float CurrentSpawnTimer;
    [SerializeField] private float CurrentRequiredDelay;
    [SerializeField] private List<Transform> SpawnPoints = new List<Transform>();
    [SerializeField] private List<SpawnedPickupData> SpawnedPickups = new List<SpawnedPickupData>();
    #endregion

    #region Unity Methods
    private void Awake()
    {
        FindReferences();
        FindSpawnPoints();
        CheckSpawnerAvailability();

        CurrentSpawnTimer = 0f;
        CurrentRequiredDelay = InitialSpawnDelay;
        CanSpawn = false;
    }

    private void OnEnable()
    {
        MatchManager.OnRoundGameplayStarted += StartSpawnTimer;
        MatchManager.OnRoundEndFreezeStarted += StopSpawnTimer;
        MatchManager.OnRoundCleanupRequested += CleanupSpawnedPickups;
        MatchManager.OnMatchEnded += HandleMatchEnded;
    }

    private void OnDisable()
    {
        MatchManager.OnRoundGameplayStarted -= StartSpawnTimer;
        MatchManager.OnRoundEndFreezeStarted -= StopSpawnTimer;
        MatchManager.OnRoundCleanupRequested -= CleanupSpawnedPickups;
        MatchManager.OnMatchEnded -= HandleMatchEnded;
    }

    private void Update()
    {
        if (IsSpawnerEnabled == false) return;
        if (CanSpawn == false) return;

        CleanDestroyedPickupRecords();

        if (SpawnedPickups.Count >= MaxActivePickups)
        {
            CurrentSpawnTimer = 0f;
            return;
        }

        CurrentSpawnTimer += Time.deltaTime;

        if (CurrentSpawnTimer >= CurrentRequiredDelay)
        {
            SpawnPickup();

            CurrentSpawnTimer = 0f;
            CurrentRequiredDelay = SpawnInterval;
        }
    }
    #endregion

    #region Event Methods
    private void StartSpawnTimer()
    {
        if (IsSpawnerEnabled == false) return;

        CanSpawn = true;
        CurrentSpawnTimer = 0f;
        CurrentRequiredDelay = InitialSpawnDelay;
    }

    private void StopSpawnTimer()
    {
        CanSpawn = false;

        FreezeSpawnedPickups();
    }

    private void HandleMatchEnded()
    {
        CanSpawn = false;

        CleanupSpawnedPickups();
    }
    #endregion

    #region Spawn Logic
    private void SpawnPickup()
    {
        AbilityDefinition SelectedAbility = GetRandomAbilityDefinition();

        SpawnPickup(SelectedAbility);
    }

    private void SpawnPickup(AbilityDefinition SelectedAbility)
    {
        Transform SelectedSpawnPoint = GetRandomEmptySpawnPoint();

        GameObject SpawnedPickup = Instantiate(AbilityPickupPrefab, SelectedSpawnPoint.position, Quaternion.identity);

        AbilityPickup Pickup = SpawnedPickup.GetComponent<AbilityPickup>();
        Pickup.Initialize(SelectedAbility);

        SpawnedPickups.Add(new SpawnedPickupData(SpawnedPickup, SelectedSpawnPoint));
    }

    private Transform GetRandomEmptySpawnPoint()
    {
        List<Transform> EmptySpawnPoints = GetEmptySpawnPoints();

        int RandomIndex = Random.Range(0, EmptySpawnPoints.Count);

        return EmptySpawnPoints[RandomIndex];
    }

    private AbilityDefinition GetRandomAbilityDefinition()
    {
        int RandomIndex = Random.Range(0, SpawnableAbilities.Count);

        return SpawnableAbilities[RandomIndex];
    }

    private List<Transform> GetEmptySpawnPoints()
    {
        List<Transform> EmptySpawnPoints = new List<Transform>();

        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            if (IsSpawnPointOccupied(SpawnPoints[i]) == false)
            {
                EmptySpawnPoints.Add(SpawnPoints[i]);
            }
        }

        return EmptySpawnPoints;
    }

    private bool IsSpawnPointOccupied(Transform SpawnPoint)
    {
        for (int i = 0; i < SpawnedPickups.Count; i++)
        {
            if (SpawnedPickups[i].SpawnPoint == SpawnPoint)
            {
                return true;
            }
        }

        return false;
    }

    public void SpawnSpecificAbilityForDebug(AbilityDefinition SelectedAbility)
    {
        if (IsSpawnerEnabled == false)
        {
            Debug.Log("Ability Spawner is not enabled.");
            return;
        }

        CleanDestroyedPickupRecords();

        if (SpawnedPickups.Count >= MaxActivePickups)
        {
            Debug.Log("Cannot spawn debug pickup. Maximum active pickup count reached.");
            return;
        }

        List<Transform> EmptySpawnPoints = GetEmptySpawnPoints();

        if (EmptySpawnPoints.Count == 0)
        {
            Debug.Log("Cannot spawn debug pickup. No empty spawn point found.");
            return;
        }

        SpawnPickup(SelectedAbility);

        CurrentSpawnTimer = 0f;
        CurrentRequiredDelay = SpawnInterval;

        Debug.Log("Debug spawned pickup: " + SelectedAbility.GetAbilityName());
    }
    #endregion

    #region Cleanup
    private void CleanDestroyedPickupRecords()
    {
        for (int i = SpawnedPickups.Count - 1; i >= 0; i--)
        {
            if (SpawnedPickups[i].PickupObject == null)
            {
                SpawnedPickups.RemoveAt(i);
            }
        }
    }

    private void FreezeSpawnedPickups()
    {
        for (int i = 0; i < SpawnedPickups.Count; i++)
        {
            Collider2D PickupCollider = SpawnedPickups[i].PickupObject.GetComponent<Collider2D>();
            PickupCollider.enabled = false;
        }
    }

    private void CleanupSpawnedPickups()
    {
        for (int i = 0; i < SpawnedPickups.Count; i++)
        {
            Destroy(SpawnedPickups[i].PickupObject);
        }

        SpawnedPickups.Clear();
    }
    #endregion

    #region Getters
    public List<AbilityDefinition> GetSpawnableAbilities()
    {
        return SpawnableAbilities;
    }
    #endregion

    #region Reference Setup
    private void FindReferences()
    {
        SelectedMatchSettings = FindFirstObjectByType<SelectedMatchSettings>();
    }

    private void FindSpawnPoints()
    {
        Transform SpawnPointsParent = GameObject.Find(SpawnPointsParentName).transform;

        for (int i = 0; i < SpawnPointsParent.childCount; i++)
        {
            SpawnPoints.Add(SpawnPointsParent.GetChild(i));
        }
    }

    private void CheckSpawnerAvailability()
    {
        if (SelectedMatchSettings.GameType == GameType.Pongility)
        {
            IsSpawnerEnabled = true;
        }

        else
        {
            IsSpawnerEnabled = false;
        }
    }
    #endregion

    #region Helper Classes
    [System.Serializable]
    private class SpawnedPickupData
    {
        public GameObject PickupObject;
        public Transform SpawnPoint;

        public SpawnedPickupData(GameObject NewPickupObject, Transform NewSpawnPoint)
        {
            PickupObject = NewPickupObject;
            SpawnPoint = NewSpawnPoint;
        }
    }
    #endregion
}