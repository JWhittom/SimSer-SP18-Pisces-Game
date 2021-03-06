﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour 
{
    // SerializeFields for assignment in-editor
    /// <summary>
    /// Reference to flag in prefab for recoloring
    /// </summary>
    [Tooltip("The flag portion of the prefab.")]
    [SerializeField]
    public GameObject flag;
    /// <summary>
    /// List of possible spawn points for villagers
    /// </summary>
    [Tooltip("List of possible spawn points for villagers.")]
    [SerializeField]
    public List<GameObject> villagerSpawnPoints;
    /// <summary>
    /// Grave
    /// </summary>
    [Tooltip("Grave prefab")]
    [SerializeField]
    GameObject gravePrefab;
    /// <summary>
    /// Wood rack
    /// </summary>
    [Tooltip("Wood rack in prefab.")]
    [SerializeField]
    GameObject woodRack;
    /// <summary>
    /// Logs on wood rack
    /// </summary>
    [Tooltip("Logs on rack.")]
    [SerializeField]
    List<GameObject> logs;

    /// <summary>
    /// Active villagers
    /// </summary>
    public List<GameObject> villagers = new List<GameObject>();

    // Private fields
    /// <summary>
    /// Is the village active?
    /// </summary>
    bool isActiveTurn_UseProperty;
    /// <summary>
    /// Is the village dead?
    /// </summary>
    bool isDead_UseProperty;
    /// <summary>
    /// Restock rate of wood rack
    /// </summary>
    float woodRackStockRate_UseProperty;
    /// <summary>
    /// Number of logs obtainable per turn
    /// </summary>
    int maxLogsPerTurn_UseProperty;
    /// <summary>
    /// Number of residents in the village - must be between 1-3
    /// </summary>
    int numberOfVillagers_UseProperty;
    /// <summary>
    /// Total logs consumed for daily heating
    /// </summary>
    int totalLogsConsumed_UseProperty;
    /// <summary>
    /// Capacity of wood rack
    /// </summary>
    int woodRackCapacity_UseProperty;
    /// <summary>
    /// Logs invested into wood rack capacity
    /// </summary>
    int woodRackInvestment_UseProperty;
    /// <summary>
    /// Logs stocked in private wood rack
    /// </summary>
    int woodRackStock_UseProperty;
    List<GameObject> graves = new List<GameObject>();

    // Properties
    /// <summary>
    /// Public accessor for active turn state
    /// </summary>
    public bool IsActiveTurn
    {
        get { return isActiveTurn_UseProperty; }
        set { isActiveTurn_UseProperty = value; }
    }
    /// <summary>
    /// Public acessor for whether village is alive or not
    /// </summary>
    public bool IsDead
    {
        get { return isDead_UseProperty; }
        set
        {
            isDead_UseProperty = value;
            if(isDead_UseProperty == true)
            {
                for(int i = 0; i < NumberOfVillagers; i++)
                {
                    graves.Add(Instantiate(gravePrefab, gameObject.transform));
                    graves[i].transform.position = villagers[i].transform.position;
                    villagers[i].SetActive(false);
                }
            }
        }
    }
    /// <summary>
    /// Public accessor for wood rack stock rate
    /// </summary>
    public float WoodRackStockRate
    {
        get { return woodRackStockRate_UseProperty; }
        set { woodRackStockRate_UseProperty = value; }
    }
    /// <summary>
    /// Public accessor for max logs per turn
    /// </summary>
    public int MaxLogsPerTurn
    {
        get { return maxLogsPerTurn_UseProperty; }
        set { maxLogsPerTurn_UseProperty = value; }
    }

    public int WoodRackStock
    {
        get { return woodRackStock_UseProperty; }
        set { woodRackStock_UseProperty = value; }
    }
    /// <summary>
    /// Public accessor for number of villagers
    /// </summary>
    public int NumberOfVillagers
    {
        get { return numberOfVillagers_UseProperty; }
        set
        {
            numberOfVillagers_UseProperty = Mathf.Min(value, 3);
            numberOfVillagers_UseProperty = Mathf.Max(numberOfVillagers_UseProperty, 1);
        }
    }
    /// <summary>
    /// Public accessor for invested logs -- updates the capacity
    /// </summary>
    public int WoodRackInvestment
    {
        get { return woodRackInvestment_UseProperty; }
        set
        {
            woodRackInvestment_UseProperty = value;
            SetCapacity();
        }
    }
    /// <summary>
    /// Public accessor for total logs consumed
    /// </summary>
    public int TotalLogsConsumed
    {
        get { return totalLogsConsumed_UseProperty; }
        set { totalLogsConsumed_UseProperty = value; }
    }
    /// <summary>
    /// Public accessor for THE SHADOW REALM
    /// </summary>
    public int TheShadowRealm
    {
        get; set;
    }

    public int WoodRackCapacity
    {
        get { return woodRackCapacity_UseProperty; }
        set { woodRackCapacity_UseProperty = value; }
    }

    /// <summary>
    /// Set capacity of private wood rack based on wood invested into it
    /// </summary>
    public void SetCapacity()
    {
        WoodRackCapacity = (int)Mathf.Pow(WoodRackInvestment / 2, 1.5f);
    }

    /// <summary>
    /// Take wood from wood rack to use for daily heat
    /// </summary>
    /// <param name="amount">Amount of wood to remove</param>
    public void TakeWoodFromRack(int amount)
    {
        WoodRackStock -= amount;
    }

    /// <summary>
    /// Add wood to wood rack for later use
    /// </summary>
    /// <param name="amount">Amount of wood to add</param>
    public void AddWoodToRack(int amount)
    {
        WoodRackStock += amount;
    }

    /// <summary>
    /// Restock rack between rounds based on present stock
    /// </summary>
    public void RestockRack()
    {
        WoodRackStock += (int)(WoodRackStock * WoodRackStockRate);
        WoodRackStock = Mathf.Min(WoodRackStock, WoodRackCapacity);
    }

    /// <summary>
    /// Make logs/rack visible
    /// </summary>
    public void UpdateWoodRack()
    {
        int numLogs = Mathf.RoundToInt(WoodRackStock/((float)WoodRackCapacity/logs.Count));
        if (WoodRackCapacity > 0)
        {
            woodRack.SetActive(true);
        }
        if(numLogs > 0)
        {
            for (int i = 0; i < numLogs; i++)
                logs[i].SetActive(true);
            for (int i = numLogs; i < logs.Count; i++)
                logs[i].SetActive(false);
        }
        
    }
}
