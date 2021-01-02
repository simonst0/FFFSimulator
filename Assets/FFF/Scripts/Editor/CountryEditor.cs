using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Country)), CanEditMultipleObjects]
public class CountryEditor : Editor
{
    int infectionId = 1;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Country country = target as Country;
        infectionId = EditorGUILayout.IntField("Infection id", infectionId);
        if (GUILayout.Button("Infect"))
            country.Infect(infectionId);
    }
}
 