using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.Model;
using Managers;
using UnityEngine;

namespace UiPanels
{
    public class AwsLambda : MonoBehaviour
    {
        public async void InvokeLambdaFunction()
        {
            bool success = await InvokeFunctionAsync();
            Debug.Log($"Lambda function executed: {success}");
        }
        
        private async Task<bool> InvokeFunctionAsync()
        {
            var client = AwsSdkManager.Instance.GetLambdaClient();
            
            // Function name must be the same string from Cognito Lambda Console
            var request = new InvokeRequest
            {
                FunctionName = "myFunctionFromUnity",
                Payload = "{\"param\": \"Custom test parameter sent to cognito\"}"
            };

            var response = await client.InvokeAsync(request);
            string returnValue = System.Text.Encoding.UTF8.GetString(response.Payload.ToArray());
            
            AwsUiManager.Instance.SetFeedbackText(returnValue);
            
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}