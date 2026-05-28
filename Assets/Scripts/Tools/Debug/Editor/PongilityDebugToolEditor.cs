using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PongilityDebugTool))]
public class PongilityDebugToolEditor : Editor
{
    private SerializedProperty SelectedAbilityProperty;
    private SerializedProperty AbilitySpawnerProperty;
    private SerializedProperty Player1InventoryProperty;
    private SerializedProperty Player2InventoryProperty;
    private SerializedProperty MatchManagerProperty;

    private bool ShowRuntimeReferences;

    private void OnEnable()
    {
        SelectedAbilityProperty = serializedObject.FindProperty("SelectedAbility");
        AbilitySpawnerProperty = serializedObject.FindProperty("AbilitySpawner");
        Player1InventoryProperty = serializedObject.FindProperty("Player1Inventory");
        Player2InventoryProperty = serializedObject.FindProperty("Player2Inventory");
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

        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawDebugValuesSection()
    {
        EditorGUILayout.LabelField("Debug Values", EditorStyles.boldLabel);

        DrawAbilityDropdown();
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
        EditorGUILayout.PropertyField(Player1InventoryProperty);
        EditorGUILayout.PropertyField(Player2InventoryProperty);
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