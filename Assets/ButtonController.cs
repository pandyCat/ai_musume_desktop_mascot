using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class ButtonController : MonoBehaviour
{
    public GameObject inputGameObject;
    public Button button;
    public GameObject audioGameObject;

    [System.Serializable]
    public class ReplayRequest
    {
        public string fileName;
    }

    public void OnClick()
    {
        Debug.Log("�����ꂽ");
        string inputText = inputGameObject.GetComponent<Text>().text;
        Debug.Log(inputGameObject.GetComponent<Text>().text);
        StartCoroutine(PostRequest(inputText));

        //InputField �R���|�[�l���g���擾
        InputField form = GameObject.Find("InputField").GetComponent<InputField>();
        form.text = "";
    }

    IEnumerator PostRequest(string inputText)
    {
        string api_server = "http://machiko.f5.si:443";
//        string api_server = "http://192.168.1.17:443";

        // ���N�G�X�gURL
        string url = api_server+ "/getResponseFromUnity";

        WWWForm form = new WWWForm();
        form.AddField("text", inputText);

        // ���N�G�X�g�w�b�_
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // ��M�����f�[�^���擾����
                string responseText = www.downloadHandler.text;
                // JSON�f�[�^���f�V���A���C�Y����
                ReplayRequest replayRequest = JsonUtility.FromJson<ReplayRequest>(www.downloadHandler.text);
                string replayMessage = replayRequest.fileName;
                Debug.Log(replayMessage);
                string audioUrl = $"{api_server}/audio?file_name={replayMessage}";
                Debug.Log(audioUrl);


                using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(audioUrl, AudioType.WAV))
                {
                    yield return request.SendWebRequest();
                    if (request.isNetworkError || request.isHttpError)
                    {
                        Debug.Log("���s�I");
                    }
                    else
                    {
                        Debug.Log("�����擾����");
                        AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
                        AudioSource audioSource = audioGameObject.GetComponent<AudioSource>();
                        audioSource.clip = audioClip;
                        audioSource.Play();

                    }
                }
            }
        }
    }
}