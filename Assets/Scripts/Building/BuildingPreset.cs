using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Building Preset",menuName ="New Building Preset",order = 0)]
public class BuildingPreset : ScriptableObject
{
    [SerializeField]
    private int cost;
    [SerializeField]
    private int costPerTurn;
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private int population;
    [SerializeField]
    private int jobs;
    [SerializeField]
    private int food;

    public int Cost { get => cost; set => cost = value; }
    public int CostPerTurn { get => costPerTurn; set => costPerTurn = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public int Population { get => population; set => population = value; }
    public int Jobs { get => jobs; set => jobs = value; }
    public int Food { get => food; set => food = value; }
}
