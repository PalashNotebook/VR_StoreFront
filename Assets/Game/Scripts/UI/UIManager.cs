using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

interface IUIManager
{
    void RefreshUIListings();
}

public class UIManager : MonoBehaviour, IUIManager
{
    [Header("Group related Fields")]
    [SerializeField] private GroupUI groupPrefab;
    [SerializeField] private Transform groupParent;
    [SerializeField] private Transform groupRoot;
    
    [Header("Item related Fields")]
    [Space(5)]
    [SerializeField] private ItemUI itemPrefab;
    [SerializeField] private Transform itemParent;
    [SerializeField] private Transform itemRoot;

    [SerializeField] private Transform itemInfoParent;
    private Data _data;
    private List<ItemUI> allItems = new List<ItemUI>();

    // Start is called before the first frame update
    void Start()
    {
        RefreshUIListings();
    }


    public void RefreshUIListings()
    {
        StartCoroutine(DownloadUIListings((string listingData) =>
        {
            Data model = new Data();
            model = JsonUtility.FromJson<Data>(listingData);
            _data = model;
            DistributeGroupUI(model);
        }));
    }

    IEnumerator DownloadUIListings(UnityAction<string> action)
    {
        string path = Path.Combine(ConstData.ServerBaseURL, "listingData.json");
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(path);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.error != null)
        {
            Debug.LogError("Error in Download: "+ unityWebRequest.error);
            yield break;
        }
        
        action.Invoke(Encoding.UTF8.GetString(unityWebRequest.downloadHandler.data));
    }

    void DistributeGroupUI(Data _dataModel)
    { 
        ShowGroupUI(_dataModel.electronic.type, _dataModel.electronic.assetImage);
       ShowGroupUI(_dataModel.furniture.type, _dataModel.furniture.assetImage);
    }

    void ShowGroupUI(GroupType _type, string _image)
    { 
          //Instantiate
          GroupUI groupUI = Instantiate(groupPrefab);
          
          //Set Parent
          groupUI.transform.SetParent(groupParent);
          
          //Reset Transform
          groupUI.transform.localPosition = groupPrefab.transform.localPosition;
          groupUI.transform.localEulerAngles = groupPrefab.transform.localEulerAngles;
          groupUI.transform.localScale = groupPrefab.transform.localScale;

          //SetData
          groupUI.ShowGroupUI(_type, _image, OnGroupButtonClicked);
    }

    void OnGroupButtonClicked(GroupType _type)
    {
        groupRoot.gameObject.SetActive(false);
        itemRoot.gameObject.SetActive(true);

        int count = 0;

        if (_type == GroupType.Electronics)
            count = _data.electronic.electronics.Length;
        else
            count = _data.furniture.furnitures.Length;

        //Clear Old items
        foreach (var item in allItems)
        {
           Destroy(item.gameObject); 
        }
        allItems.Clear();
        
        for (int i = 0; i < count; i++)
        {
            //Instantiate
            ItemUI itemUI = Instantiate(itemPrefab);

            //Set Parent
            itemUI.transform.SetParent(itemParent);
            
            //Reset Transform
            itemUI.transform.localPosition = itemPrefab.transform.localPosition;
            itemUI.transform.localEulerAngles = itemPrefab.transform.localEulerAngles;
            itemUI.transform.localScale = itemPrefab.transform.localScale;

            if (_type == GroupType.Electronics)
                itemUI.ShowItemUI(_data.electronic.electronics[i], _type, itemInfoParent);
            else
                itemUI.ShowItemUI(_data.furniture.furnitures[i], _type, itemInfoParent);
            
            allItems.Add(itemUI);
        }
    }

    public void OnHomeButtonPressed()
    {
        groupRoot.gameObject.SetActive(true);
        itemRoot.gameObject.SetActive(false);
    }
}
