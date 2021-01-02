using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Country : MonoBehaviour, IRTSUpdateReciever
{
    public string countryName = "defalut";

    [SerializeField]
    private int inhabitants;

    [SerializeField]
    private List<Country> neighbouringCountries = new List<Country>();

    [SerializeField]
    private float neighbourCastRadiusMagnifier = 1.5f;

    [SerializeField]
    private List<FractionScriptableObject> fractions = new List<FractionScriptableObject>();

    [SerializeField]
    private Dictionary<int, Fraction> activeFractions = new Dictionary<int, Fraction>();

    float oneInhabitantPercentage { get { return 1.0f / inhabitants; } }

    private void Start()
    {
        NeighbourSphereCast();
        Infect(0);
        RTSController.Instance.RegisterUpdateReceiver(this);
    }

    public void UpdateRTSSimulation(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            UpdateFractions();
            foreach (var fraction in activeFractions)
                Spread(fraction.Value.id);
        }
        UpdateColors();
    }

    private void NeighbourSphereCast()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        float radius = renderer.bounds.extents.magnitude * neighbourCastRadiusMagnifier;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, Vector3.one, 0f);
        foreach (var hit in hits)
        {
            Country neighbour;
            if (hit.transform.TryGetComponent<Country>(out neighbour) && neighbour != this)
                neighbouringCountries.Add(neighbour);
        }
    }

    private void UpdateFractions()
    {
        float[] deltas = new float[activeFractions.Count];
        float[] modifiedDeltas = new float[activeFractions.Count];
        int count = 0;
        foreach (var fraction in activeFractions)
        {
            float delta = fraction.Value.growthSpeed;
            deltas[count] = delta;
            modifiedDeltas[count] = delta;
            ++count;
        }
        for (int i = 0; i < activeFractions.Count; i++)
            for (int j = 1; j < activeFractions.Count; j++)
            {
                modifiedDeltas[i] -= deltas[j] * activeFractions[i].influencePercentage;
            }
        count = 0;
        foreach (var fraction in activeFractions)
        {
            fraction.Value.influencePercentage = Mathf.Clamp01(fraction.Value.influencePercentage + modifiedDeltas[count]);
            count++;
        }
        //ClearSmallFractions();
    }

    void ClearSmallFractions()
    {
        List<int> idsToClear = new List<int>();
        foreach (var fraction in activeFractions)
            if (fraction.Value.influencePercentage < oneInhabitantPercentage)
                idsToClear.Add(fraction.Value.id);
        for (int i = 0; i < idsToClear.Count; i++)
        {
            int id = idsToClear[i];
            Destroy(activeFractions[id]);
            activeFractions.Remove(id);
        }

    }

    private void UpdateColors()
    {
        Color color = Color.black;
        foreach (var fraction in activeFractions)
            color += fraction.Value.colorAttribution;
        GetComponentInChildren<Renderer>().material.color = color;
    }

    public void Infect(int id)
    {
        Fraction fraction;
        if (activeFractions.ContainsKey(id))
            fraction = activeFractions[id];
        else
        {
            fraction = gameObject.AddComponent<Fraction>();
            fraction.FillDataFromScriptableObject(fractions[id]);
            activeFractions.Add(fraction.id, fraction);
        }
        fraction.influencePercentage = Mathf.Clamp01(fraction.influencePercentage + oneInhabitantPercentage);
        for (int i = 0; i < activeFractions.Count; i++) // remove one person from existing fractions
            if (activeFractions[i].id != id)
                activeFractions[i].influencePercentage -= oneInhabitantPercentage * activeFractions[i].influencePercentage;
    }

    public void Spread(int id)
    {
        foreach (var neighbour in neighbouringCountries)
        {
            float spreadChanze = activeFractions[id].baseSpreadChance * activeFractions[id].influencePercentage;
            if (UnityEngine.Random.value <= spreadChanze)
            {
                neighbour.Infect(activeFractions[id].id);
                Debug.Log("Spread to " + neighbour.name);
            }
        }
    }

    private void OnDestroy()
    {
        if (RTSController.Instance)
            RTSController.Instance.DeregisterUpdateReceiver(this);
    }
}