using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class GroupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private RawImage imageField;
    [SerializeField] private Button button;
    private GroupType type;
    public void ShowGroupUI(GroupType _type, string _imagePath, UnityAction<GroupType> _onClick)
    {
        type = _type;
        nameField.text = _type.ToString();
        StartCoroutine(DownloadImage(_imagePath, (Texture _texture) =>
        {
            imageField.texture = _texture;
        }));
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
            {
                _onClick.Invoke(type);
            }
        );
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
