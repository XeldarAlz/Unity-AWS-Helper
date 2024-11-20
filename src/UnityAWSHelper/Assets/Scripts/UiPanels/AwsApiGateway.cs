using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.Networking;

namespace UiPanels
{
    public class AwsApiGateway : MonoBehaviour
    {
        public void CallApi()
        {
            StartCoroutine(CallApiAsync());
        }

        private IEnumerator CallApiAsync()
        {
            byte[] myData = System.Text.Encoding.UTF8.GetBytes("{\"param\": \"test\"}");

            using (UnityWebRequest www = UnityWebRequest.Put(AwsSdkManager.Instance.GetApiGatewayUri(), myData))
            {
                www.method = "POST";

                yield return www.SendWebRequest();
                bool success = www.result == UnityWebRequest.Result.Success;
                
                Debug.Log($"Call API status: {success}");
                AwsUiManager.Instance.SetFeedbackText(www.downloadHandler.text);
            }
        }
    }
}