using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Flashlight : MonoBehaviour
{
    [SerializeField]private List<GameObject> electricityList = new List<GameObject>();
    [SerializeField] private GameObject prefab;

    private void Start()
    {
        EventManager.instance.AddEventListener<int>("ElectricityNumChange", UpdateElectricity);
        UpdateElectricity(GameManager.instance.player.electrictyNum);
    }

    private void Update()
    {

    }

    public void UpdateElectricity(int num)
    {
        while(electricityList.Count>num)
        {
            DecreaseElectricity();
        }
        while (electricityList.Count < num)
        {
            AddElectricity();
        }
    }

    public void AddElectricity()
    {
        GameObject newElcctricity = Instantiate(prefab,transform);
        electricityList.Add(newElcctricity);
        TurnWhite();
    }

    public void DecreaseElectricity()
    {
        Destroy(electricityList[electricityList.Count - 1]);
        electricityList.RemoveAt(electricityList.Count - 1);
        if (electricityList.Count<=3)
        {
            TurnRed();
        }
    }

    public void TurnRed()
    {
        Image[] images=GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color= Color.red;
        }
    }

    public void TurnWhite()
    {
        Image[] images = GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = Color.white;
        }
    }
}
