using System.Collections;
using System.Collections.Generic;
using System.IO;
using com.palash.immersed;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FurnituresInfoUI : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI size;
   [SerializeField] private Image color;
   [SerializeField] private Button buyButton, closeButton;


   private static Dictionary<string, AssetBundle> loadedBundles = new Dictionary<string, AssetBundle>();
   // Start is called before the first frame update
   public void Show(Furnitures _furniture, UnityAction _buyCallback, UnityAction _onCloseClick)
   {
       size.text = _furniture.size.ToString();
       color.color = _furniture.color;

       if (_furniture.isAvailable)
       {
           buyButton.onClick.RemoveAllListeners();
           buyButton.onClick.AddListener(() =>
           {
               StartCoroutine(OnClick(_furniture.assetLink, _furniture.assetName, _furniture.assetVersion));
           });
           buyButton.onClick.AddListener(_buyCallback);
       }
       else
       {
           buyButton.interactable = false;
       }

       closeButton.onClick.RemoveAllListeners();
       closeButton.onClick.AddListener(_onCloseClick);
   }
   
   IEnumerator OnClick(string _assetLink, string _name, uint _assetVersion)
   {
       _assetLink = Path.Combine(ConstData.ServerBaseURL, _assetLink);
       AssetBundle bundle = null;
       
       if (loadedBundles.ContainsKey(_assetLink))
       {
           bundle = loadedBundles[_assetLink];
       }
       else
       {
           UnityWebRequest unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(_assetLink, _assetVersion, 0);
           yield return unityWebRequest.SendWebRequest();

           if (unityWebRequest.error != null)
           {
               Debug.LogError("Error in Download: "+ unityWebRequest.error);
               yield break;
           }

           bundle = DownloadHandlerAssetBundle.GetContent(unityWebRequest); 
       }

       DownloadableAsset asset = bundle.LoadAsset<GameObject>(_name).GetComponent<DownloadableAsset>();
       DownloadableAsset obj = Instantiate(asset);
       obj.transform.position += new Vector3(Random.Range(-2, 2), 0, Random.Range(2, 5));
       buyButton.interactable = false;
       
       if (!loadedBundles.ContainsKey(_assetLink))
       loadedBundles.Add(_assetLink, bundle);

   }
}
