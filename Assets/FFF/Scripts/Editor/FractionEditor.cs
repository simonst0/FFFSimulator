using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Fraction))]
public class FractionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Fraction fraction = target as Fraction;
        if (GUILayout.Button("Spread"))
        {
            fraction.GetComponent<Country>().Spread(fraction.id);
        }
    }
}
