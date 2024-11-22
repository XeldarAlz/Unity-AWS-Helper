using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.DynamoDBv2.Model;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UiPanels
{
    public class AwsDynamoDB : MonoBehaviour
    {
        public async void UploadItem()
        {
            try
            {
                bool success = await UploadItemAsync();
                AwsUiManager.Instance.SetFeedbackText($"DynamoDB upload status: {success}");
            }
            catch (Exception e)
            {
                AwsUiManager.Instance.SetFeedbackText(e.Message);
            }
        }

        public async void DownloadItem()
        {
            try
            {
                bool success = await DownloadItemAsync();
                // AwsUiManager.Instance.SetFeedbackText($"DynamoDB download status: {success}");
            }
            catch (Exception e)
            {
                AwsUiManager.Instance.SetFeedbackText(e.Message);
            }
        }
        
        private async Task<bool> DownloadItemAsync()
        {
            try
            {
                var currentUserResponse = await AwsSdkManager.Instance.GetCurrentUserResponse();
                var email = currentUserResponse.UserAttributes.Find(attribute => attribute.Name.Equals("email")).Value;
                var result = await FetchUserData(email);

                if (result != null)
                {
                    string feedback = $"Email: {result["email"].S}, Score: {result["score"].N}";
                    AwsUiManager.Instance.SetFeedbackText(feedback);
                    return true;
                }
                
                return false;
            }
            catch (Exception e)
            {
                AwsUiManager.Instance.SetFeedbackText($"{e.Message}");
                return false;
            }
        }
        
        private async Task<Dictionary<string, AttributeValue>> FetchUserData(string email)
        {
            var client = AwsSdkManager.Instance.GetDynamoDBClient();

            var request = new GetItemRequest
            {
                TableName = "MyTable",
                Key = new Dictionary<string, AttributeValue>
                {
                    ["email"] = new AttributeValue { S = email }
                }
            };

            var response = await client.GetItemAsync(request);
            return response.HttpStatusCode == HttpStatusCode.OK ? response.Item : null;
        }

        
        private static async Task<bool> UploadItemAsync()
        {
            var client = AwsSdkManager.Instance.GetDynamoDBClient();
            var currentUserResponse = await AwsSdkManager.Instance.GetCurrentUserResponse();
            var email = currentUserResponse.UserAttributes.Find(attribute => attribute.Name.Equals("email")).Value;
            
            // S = String
            // N = Number
            // L = List
            var item = new Dictionary<string, AttributeValue>
            {
                ["email"] = new() { S = $"{email}" },
                ["score"] = new() { N = $"{Random.Range(0, 500)}" },
                ["itemsFound"] = new()
                {
                    L =
                    {
                        new AttributeValue { N = $"{Random.Range(0, 10)}"},
                        new AttributeValue { N = $"{Random.Range(0, 100)}"},
                        new AttributeValue { N = $"{Random.Range(0, 1000)}"}
                    }
                }
            };

            // DynamoDB Table name
            var request = new PutItemRequest
            {
                TableName = "MyTable",
                Item = item,
            };

            var response = await client.PutItemAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}