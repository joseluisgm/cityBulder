
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class City : MonoBehaviour
{
    public float curDayTime;
    private float dayTime=24;
    private float minutes;
    public float speedFactor= 1;
    public GameObject sun;
    private float seepdFactor_temp;
    public TextMeshProUGUI timeText;

    public int money;
    public int day;
    public int curPopulation;
    public int curJobs;
    public int curFood;
    public int maxPopulation;
    public int maxJobs;
    public int incomePerJob;
    private int aux;

    public TextMeshProUGUI stastText;
    public List<Building> buildings=new List<Building>();
    public GameObject ButtonSelected;

    public static City Instance;

  

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ButtonSelected.GetComponent<Image>().color =new Color(0.90f,0.23f,0.23f,1);
        UpdateStatsText();
    }
    public void OnPlaceBuilding(Building building)
    {
        money -= building.presert.cost;
        maxPopulation += building.presert.population;
        maxJobs += building.presert.jobs;
        buildings.Add(building);
        UpdateStatsText();
    }

    private void UpdateStatsText()
    {
        stastText.text = string.Format("Day:{0} Money:{1}€ population:{2}/{3} jobs:{4}/{5} food:{6}",new object[7] {day,money,curPopulation,maxPopulation,curJobs,maxJobs,curFood});
    }

    public void OnRemove(Building building)
    {
        maxPopulation -= building.presert.population;
        maxJobs -= building.presert.jobs;

        buildings.Remove(building);
        Destroy(building.gameObject);
        UpdateStatsText();

    }
     public void NextDay()
    {
        day++;
        CalculateMoney();
        CalculatePopulation();
        CalculateJobs();
        CalculateFood();

        UpdateStatsText() ;
    }

    private void CalculateFood()
    {
        curFood = 0;
        foreach (Building building in buildings)
        {
            curFood += building.presert.food;
        }
    }

    private void CalculateJobs()
    {
        curJobs = Mathf.Min(curPopulation, maxJobs);
    }

    private void CalculatePopulation()
    {
        if(curFood>=curPopulation&&curPopulation<maxPopulation)
        {
            curFood -= curPopulation / 4;
            curPopulation = Mathf.Min(curPopulation + (curFood / 4), maxPopulation);
        }else if (curFood < curPopulation)
        {
            curPopulation=curFood;
        }
    }

    private void CalculateMoney()
    {
        money += curJobs * incomePerJob;
        foreach(Building building in buildings)
        {
            money-= building.presert.costPerTurn;
        }
    }
    private void FixedUpdate()
    {
        DayCycle();
    }

    private void DayCycle()
    {
        curDayTime += Time.deltaTime * speedFactor;
        if(curDayTime >= dayTime)
        {
            curDayTime = 0;
            NextDay();
        }

        double hour=Math.Truncate(curDayTime);
        minutes += speedFactor * (Time.deltaTime * 6) * 10;
        double minutesDouble=Math.Truncate(minutes);
        if (minutesDouble >= 60)
            minutes = 0;
        string hourString = hour.ToString("00");
        string minutesString = minutes.ToString("00");
        timeText.text = hourString +":"+ minutesString;

        sun.transform.rotation=Quaternion.Euler(((curDayTime-6)/dayTime)*360,0f,0f);
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 2);

    }

    public void SpeedDayCycle(int factor)
    {
        speedFactor=factor;
    }

    public void ChangeColor (GameObject button)
    {
        if(ButtonSelected != null)
        {
            ButtonSelected.GetComponent<Image>().color = Color.white;
            ButtonSelected = button;
            ButtonSelected.GetComponent<Image>().color = new Color(0.90f, 0.23f, 0.23f,1);
        }
       
    }
}
