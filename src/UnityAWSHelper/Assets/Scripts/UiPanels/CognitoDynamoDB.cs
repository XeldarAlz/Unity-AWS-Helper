using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Managers;
using UnityEngine;

namespace UiPanels
{
    public class CognitoDynamoDB : MonoBehaviour
    {
        public async void PutItem()
        {
            bool success = await PutItemAsync();
            CognitoUiManager.Instance.SetFeedbackText($"DynamoDB status: {success}");
        }
        
        private static async Task<bool> PutItemAsync()
        {
            // S = String
            // N = Number
            // L = List
            var item = new Dictionary<string, AttributeValue>
            {
                ["email"] = new AttributeValue { S = "testing@gmail.com" },
                ["score"] = new AttributeValue { N = "450" },
                ["itemsFound"] = new AttributeValue()
                {
                    L =
                    {
                        new AttributeValue { N = "1"},
                        new AttributeValue { N = "12"},
                        new AttributeValue { N = "85"}
                    }
                }
            };

            // DynamoDB Table name
            var request = new PutItemRequest
            {
                TableName = "MyTable",
                Item = item,
            };

            var response = await CognitoSdkManager.Instance.GetDynamoDBClient().PutItemAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}