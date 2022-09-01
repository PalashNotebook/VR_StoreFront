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
    
    public void ShowItemUI(Items _items)
    {
         
        nameField.text = _items.name;
        StartCoroutine(DownloadImage(_items.imagePath, (Texture _texture) =>
        {
            imageField.texture = _texture;
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
}

public class FurnitureUI
{
    private Furnitures furniture;
    public void Initialize(Furnitures _furniture)
    {
        furniture = _furniture;
    }

    public void OnClick()
    {
        
    }
}
