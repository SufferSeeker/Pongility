using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PongilityDebugTool))]
public class PongilityDebugToolEditor : Editor
{
    private SerializedProperty SelectedAbilityProperty;
    private SerializedProperty SelectedStatusEffectProperty;
    private SerializedProperty DebugBallSpeedProperty;
    private SerializedProperty DebugDamageAmountProperty;
    private SerializedProperty DebugHealAmountProperty;
    private SerializedProperty DebugTimeAmountSecondsProperty;

    private SerializedProperty AbilitySpawnerProperty;
    private SerializedProperty BallControllerProperty;
    private SerializedProperty Player1InventoryProperty;
    private SerializedProperty Player2InventoryProperty;
    private SerializedProperty Player1DamageableTargetProperty;
    private SerializedProperty Player2DamageableTargetProperty;
    private SerializedProperty Player1StatusEffectReceiverProperty;
    private SerializedProperty Player2StatusEffectReceiverProperty;
    private SerializedProperty MatchManagerProperty;

    private bool ShowRuntimeReferences;

    private void OnEnable()
    {
        SelectedAbilityProperty = serializedObject.FindProperty("SelectedAbility");
        SelectedStatusEffectProperty = serializedObject.FindProperty("SelectedStatusEffect");
        DebugBallSpeedProperty = serializedObject.FindProperty("DebugBallSpeed");
        DebugDamageAmountProperty = serializedObject.FindProperty("DebugDamageAmount");
        DebugHealAmountProperty = serializedObject.FindProperty("DebugHealAmount");
        DebugTimeAmountSecondsProperty = serializedObject.FindProperty("DebugTimeAmountSeconds");

        AbilitySpawnerProperty = serializedObject.FindProperty("AbilitySpawner");
        BallControllerProperty = serializedObject.FindProperty("BallController");
        Player1InventoryProperty = serializedObject.FindProperty("Player1Inventory");
        Player2InventoryProperty = serializedObject.FindProperty("Player2Inventory");
        Player1DamageableTargetProperty = serializedObject.FindProperty("Player1DamageableTarget");
        Player2DamageableTargetProperty = serializedObject.FindProperty("Player2DamageableTarget");
        Player1StatusEffectReceiverProperty = serializedObject.FindProperty("Player1StatusEffectReceiver");
        Player2StatusEffectReceiverProperty = serializedObject.FindProperty("Player2StatusEffectReceiver");
        MatchManagerProperty = serializedObject.FindProperty("MatchManager");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        PongilityDebugTool DebugTool = (PongilityDebugTool)target;

        DrawDebugValuesSection();
        DrawAdvancedRuntimeReferencesSection();

        EditorGUILayout.Space(10f);

        if (Application.isPlaying == false)
        {
            EditorGUILayout.HelpBox("Enter Play Mode to use debug commands.", MessageType.Info);
        }

        EditorGUI.BeginDisabledGroup(Application.isPlaying == false);

        DrawAbilityDebugSection(DebugTool);
        DrawBallDebugSection(DebugTool);
        DrawHealthDebugSection(DebugTool);
        DrawStatusEffectDebugSection(DebugTool);
        DrawMatchDebugSection(DebugTool);

        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawDebugValuesSection()
    {
        EditorGUILayout.LabelField("Debug Values", EditorStyles.boldLabel);

        DrawAbilityDropdown();

        EditorGUILayout.PropertyField(SelectedStatusEffectProperty);
    }

    private void DrawAbilityDropdown()
    {
        AbilitySpawner AbilitySpawnerReference = AbilitySpawnerProperty.objectReferenceValue as AbilitySpawner;

        if (AbilitySpawnerReference == null)
        {
            AbilitySpawnerReference = Object.FindFirstObjectByType<AbilitySpawner>();
        }

        if (AbilitySpawnerReference == null)
        {
            EditorGUILayout.HelpBox("AbilitySpawner could not be found.", MessageType.Warning);
            EditorGUILayout.PropertyField(SelectedAbilityProperty);
            return;
        }

        List<AbilityDefinition> SpawnableAbilities = AbilitySpawnerReference.GetSpawnableAbilities();

        if (SpawnableAbilities == null)
        {
            EditorGUILayout.HelpBox("Spawnable ability list could not be found.", MessageType.Warning);
            EditorGUILayout.PropertyField(SelectedAbilityProperty);
            return;
        }

        if (SpawnableAbilities.Count == 0)
        {
            EditorGUILayout.HelpBox("Spawnable ability list is empty.", MessageType.Warning);
            EditorGUILayout.PropertyField(SelectedAbilityProperty);
            return;
        }

        string[] AbilityNames = new string[SpawnableAbilities.Count + 1];

        AbilityNames[0] = "None";

        for (int i = 0; i < SpawnableAbilities.Count; i++)
        {
            AbilityDefinition Ability = SpawnableAbilities[i];

            if (Ability == null)
            {
                AbilityNames[i + 1] = "Missing Ability";
            }

            else
            {
                AbilityNames[i + 1] = Ability.GetAbilityName();
            }
        }

        AbilityDefinition CurrentAbility = SelectedAbilityProperty.objectReferenceValue as AbilityDefinition;

        int SelectedIndex = 0;

        for (int i = 0; i < SpawnableAbilities.Count; i++)
        {
            if (SpawnableAbilities[i] == CurrentAbility)
            {
                SelectedIndex = i + 1;
            }
        }

        int NewSelectedIndex = EditorGUILayout.Popup("Selected Ability", SelectedIndex, AbilityNames);

        if (NewSelectedIndex == 0)
        {
            SelectedAbilityProperty.objectReferenceValue = null;
        }

        else
        {
            SelectedAbilityProperty.objectReferenceValue = SpawnableAbilities[NewSelectedIndex - 1];
        }
    }

    private void DrawAdvancedRuntimeReferencesSection()
    {
        EditorGUILayout.Space(8f);

        ShowRuntimeReferences = EditorGUILayout.Foldout(ShowRuntimeReferences, "Advanced Runtime References", true);

        if (ShowRuntimeReferences == false) return;
        
        EditorGUI.BeginDisabledGroup(true);

        EditorGUILayout.PropertyField(AbilitySpawnerProperty);
        EditorGUILayout.PropertyField(BallControllerProperty);
        EditorGUILayout.PropertyField(Player1InventoryProperty);
        EditorGUILayout.PropertyField(Player2InventoryProperty);
        EditorGUILayout.PropertyField(Player1DamageableTargetProperty);
        EditorGUILayout.PropertyField(Player2DamageableTargetProperty);
        EditorGUILayout.PropertyField(Player1StatusEffectReceiverProperty);
        EditorGUILayout.PropertyField(Player2StatusEffectReceiverProperty);
        EditorGUILayout.PropertyField(MatchManagerProperty);

        EditorGUI.EndDisabledGroup();
    }

    private void DrawAbilityDebugSection(PongilityDebugTool DebugTool)
    {
        EditorGUILayout.Space(8f);
        EditorGUILayout.LabelField("Ability Debug", EditorStyles.boldLabel);

        if (DrawCenteredButton("Spawn Selected Ability Pickup") == true)
        {
            DebugTool.SpawnSelectedAbilityPickup();
        }

        EditorGUILayout.Space(4f);
        EditorGUILayout.LabelField("Inventory Debug", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Give To Player 1", GUILayout.Width(150f), GUILayout.Height(24f)) == true)
        {
            DebugTool.GiveSelectedAbilityToPlayer1();
        }

        if (GUILayout.Button("Give To Player 2", GUILayout.Width(150f), GUILayout.Height(24f)) == true)
        {
            DebugTool.GiveSelectedAbilityToPlayer2();
        }

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(4f);
        EditorGUILayout.LabelField("Cleanup Debug", EditorStyles.boldLabel);

        if (DrawCenteredButton("Clear Ability Objects") == true)
        {
            DebugTool.ClearAbilityObjects();
        }
    }

    private void DrawBallDebugSection(PongilityDebugTool DebugTool)
    {
        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField("Ball Debug", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(DebugBallSpeedProperty);

        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Stop Ball", GUILayout.Width(120f), GUILayout.Height(24f)) == true)
        {
            DebugTool.StopBall();
        }

        if (GUILayout.Button("Resume Ball", GUILayout.Width(120f), GUILayout.Height(24f)) == true)
        {
            DebugTool.ResumeBall();
        }

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();

        if (DrawCenteredButton("Reset Ball To Center") == true)
        {
            DebugTool.ResetBallToCenter();
        }

        if (DrawCenteredButton("Set Ball Speed") == true)
        {
            DebugTool.SetBallSpeed();
        }
    }

    private void DrawHealthDebugSection(PongilityDebugTool DebugTool)
    {
        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField("Health Debug", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(DebugDamageAmountProperty);
        EditorGUILayout.PropertyField(DebugHealAmountProperty);

        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Damage Player 1", GUILayout.Width(150f), GUILayout.Height(24f)) == true)
        {
            DebugTool.DamagePlayer1();
        }

        if (GUILayout.Button("Damage Player 2", GUILayout.Width(150f), GUILayout.Height(24f)) == true)
        {
            DebugTool.DamagePlayer2();
        }

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Heal Player 1", GUILayout.Width(150f), GUILayout.Height(24f)) == true)
        {
            DebugTool.HealPlayer1();
        }

        if (GUILayout.Button("Heal Player 2", GUILayout.Width(150f), GUILayout.Height(24f)) == true)
        {
            DebugTool.HealPlayer2();
        }


        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawStatusEffectDebugSection(PongilityDebugTool DebugTool)
    {
        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField("Status Effect Debug", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Apply To Player 1", GUILayout.Width(150f), GUILayout.Height(24f)) == true)
        {
            DebugTool.ApplySelectedStatusEffectToPlayer1();
        }

        if (GUILayout.Button("Apply To Player 2", GUILayout.Width(150f), GUILayout.Height(24f)) == true)
        {
            DebugTool.ApplySelectedStatusEffectToPlayer2();
        }

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawMatchDebugSection(PongilityDebugTool DebugTool)
    {
        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField("Match Debug", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Reset Round", GUILayout.Width(150f), GUILayout.Height(24f)) == true)
        {
            DebugTool.ResetRound();
        }

        if (GUILayout.Button("End Match", GUILayout.Width(150f), GUILayout.Height(24f)) == true)
        {
            DebugTool.EndMatch();
        }

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(4f);
        EditorGUILayout.LabelField("Score Debug", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add Score To Player 1", GUILayout.Width(150f), GUILayout.Height(24f)) == true)
        {
            DebugTool.AddScoreToPlayer1();
        }

        if (GUILayout.Button("Add Score To Player 2", GUILayout.Width(150f), GUILayout.Height(24f)) == true)
        {
            DebugTool.AddScoreToPlayer2();
        }

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(4f);
        EditorGUILayout.LabelField("Time Debug", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(DebugTimeAmountSecondsProperty);

        bool HasTimeLimit = DebugTool.HasTimeLimit();

        if (HasTimeLimit == false)
        {
            EditorGUILayout.HelpBox("Time debug commands are disabled because this match has no time limit.", MessageType.Info);
        }

        EditorGUI.BeginDisabledGroup(HasTimeLimit == false);

        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add Time", GUILayout.Width(100f), GUILayout.Height(24f)) == true)
        {
            DebugTool.AddTime();
        }

        if (GUILayout.Button("Decrease Time", GUILayout.Width(100f), GUILayout.Height(24f)) == true)
        {
            DebugTool.DecreaseTime();
        }

        if (GUILayout.Button("Set Time", GUILayout.Width(100f), GUILayout.Height(24f)) == true)
        {
            DebugTool.SetTime();
        }

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();

        EditorGUI.EndDisabledGroup();
    }

    private bool DrawCenteredButton(string ButtonText)
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        bool WasClicked = GUILayout.Button(ButtonText, GUILayout.Width(240f), GUILayout.Height(24f));

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();

        return WasClicked;
    }
}