using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class BuildingPlacement : MonoBehaviour
{
    private bool currentlyPlacing;
    private bool currentlyBullDozering;
    private BuildingPreset curBuildingPreset;
    private float indicatorUpDateRate = 0.05f;
    private float lastUpdateTime;
    private Vector3 curIndicatorPos;


    [SerializeField]
    private GameObject PlacementIndicator;
    [SerializeField]
    private GameObject BulldozerIndicator;
   
    private PlayerInputsMaps playerInputs;

    private void Awake()
    {
        playerInputs = new PlayerInputsMaps();
        playerInputs.Player.Interacte_Building.performed += e => InteracteBuilding();
        playerInputs.Player.Cancel_building.performed += e => CancellPacement();
        playerInputs.Player.Rotate_Building.performed += e => RotateBuilding();
        playerInputs.Enable();
    }

    public void begingNewBuildingPlacement(BuildingPreset presert)
    {
        if (currentlyBullDozering) CancelDozerPlacement();     
        currentlyPlacing = true;
        curBuildingPreset = presert;

        PlacementIndicator.GetComponentInChildren<MeshFilter>().mesh = presert.Prefab.GetComponentInChildren<MeshFilter>().sharedMesh;
        PlacementIndicator.GetComponentInChildren<Transform>().localScale = presert.Prefab.transform.GetChild(0).localScale;
        PlacementIndicator.SetActive(true);        
        PlacementIndicator.transform.position = new Vector3(0, -99, 0);
    }

    void CancelBuildingPlacement()
    {
        currentlyPlacing = false;
        PlacementIndicator.SetActive(false);
    }

    void CancelDozerPlacement()
    {
        currentlyBullDozering = false;
        BulldozerIndicator.SetActive(false);
    }

    public void begingBulldozer()
    {
        if (currentlyPlacing) CancelBuildingPlacement();      
        currentlyBullDozering =true;
        BulldozerIndicator.SetActive(true);
        BulldozerIndicator.transform.position=new Vector3 (0, -99, 0);

    }
    public void RotateBuilding()
    {
        if (currentlyPlacing)
        {
            PlacementIndicator.transform.Rotate(Vector3.up * 90);
        }
    }

    void CancellPacement()
    {
        if(currentlyBullDozering)
        {
            CancelDozerPlacement();
        }
        if (currentlyPlacing)
        {
            CancelBuildingPlacement();
        }
    }
    public void InteracteBuilding()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (currentlyPlacing)
            {
                if (!City.Instance.buildings.Find(x => x.transform.position == curIndicatorPos))
                {
                    GameObject buildingObj = Instantiate(curBuildingPreset.Prefab, curIndicatorPos, PlacementIndicator.transform.rotation);
                    City.Instance.OnPlaceBuilding(buildingObj.GetComponent<Building>());
                }
            }
            else if (currentlyBullDozering)
            {
                Building buildingToDestry = City.Instance.buildings.Find(x => x.transform.position == curIndicatorPos);

                if (buildingToDestry != null)
                {
                    City.Instance.OnRemove(buildingToDestry);
                }
            }
        }
    }

    private void Update()
    {
        if(Time.time-lastUpdateTime > indicatorUpDateRate)
        {
            lastUpdateTime= Time.time;
            curIndicatorPos = Selector.Instance.GetCurTilePosition();
            if(currentlyPlacing)
                PlacementIndicator.transform.position = curIndicatorPos;
            else if(currentlyBullDozering)
                BulldozerIndicator.transform.position = curIndicatorPos;
        }
      
    }
}
