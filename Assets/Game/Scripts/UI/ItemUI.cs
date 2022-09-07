using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using com.palash.immersed;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private RawImage imageField;
    [SerializeField] private Button button;
    
    [Header("Info UIs")]
    [Space(5)]
    [SerializeField] private ElectronicsInfoUI electronicsInfoUI;
    [SerializeField] private FurnituresInfoUI furnitureInfoUI;
    
    [Space(5)]
    private Items itemsDataCache;

    private GameObject currentInfoUI;
    public void ShowItemUI(Items _items, GroupType _groupType, Transform _itemInfoParent)
    {
        itemsDataCache = _items;
        nameField.text = _items.name;
        StartCoroutine(DownloadImage(_items.imagePath, (Texture _texture) =>
        {
            imageField.texture = _texture;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                OnClick(_groupType, _itemInfoParent);
            });
        }));
        
    }

    IEnumerator DownloadImage(string _imagePath, UnityAction<Texture> action)
    {
        string path = Path.Combine(ConstData.ServerBaseURL, _imagePath);
        Debug.Log("ImagePath: "+ path);
        UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(path);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.error != null)
        {
            Debug.LogError("Error in Download: "+ unityWebRequest.error);
            yield break;
        }

        action.Invoke(((DownloadHandlerTexture) (unityWebRequest.downloadHandler)).texture);
    }

    void OnClick(GroupType _groupType, Transform _itemInfoParent)
    {
        _itemInfoParent.gameObject.SetActive(true);
        if (_groupType == GroupType.Electronics)
        {
            ElectronicsInfoUI infoUI = Instantiate(electronicsInfoUI, _itemInfoParent);
            currentInfoUI = infoUI.gameObject;
            infoUI.Show(itemsDataCache as Electronics, OnBuyClick, () =>
            {
                OnCloseClick(_itemInfoParent);
            });
        }
        else  if (_groupType == GroupType.Furniture)
        {
            FurnituresInfoUI infoUI = Instantiate(furnitureInfoUI, _itemInfoParent);
            currentInfoUI = infoUI.gameObject;
            infoUI.Show(itemsDataCache as Furnitures, OnBuyClick,  () =>
            {
                OnCloseClick(_itemInfoParent);
            });
        }

    }

    void OnBuyClick()
    {
        
    }

    void OnCloseClick(Transform _itemInfoParent)
    {
        Destroy(currentInfoUI);
        _itemInfoParent.gameObject.SetActive(false);
    }
}

