using System;
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
            try
            {
                StartCoroutine(CallApiAsync());
            }
            catch (Exception e)
            {
                AwsUiManager.Instance.SetFeedbackText(e.Message);
            }
        }

        private IEnumerator CallApiAsync()
        {
            byte[] myData = System.Text.Encoding.UTF8.GetBytes("{\"param\": \"test\"}");

            using (UnityWebRequest www = UnityWebRequest.Put(AwsSdkManager.Instance.GetApiGatewayUri(), myData))
            {
                www.method = "POST";

                yield return www.SendWebRequest();
                bool success = www.result == UnityWebRequest.Result.Success;
                AwsUiManager.Instance.SetFeedbackText(www.downloadHandler.text);
            }
        }
    }
}