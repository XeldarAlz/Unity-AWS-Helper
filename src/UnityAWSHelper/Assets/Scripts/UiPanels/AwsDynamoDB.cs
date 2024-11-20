using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Managers;
using UnityEngine;

namespace UiPanels
{
    public class AwsDynamoDB : MonoBehaviour
    {
        public async void UploadItem()
        {
            bool success = await UploadItemAsync();
            AwsUiManager.Instance.SetFeedbackText($"DynamoDB status: {success}");
        }
        
        private static async Task<bool> UploadItemAsync()
        {
            var client = AwsSdkManager.Instance.GetDynamoDBClient();
            
            // S = String
            // N = Number
            // L = List
            var item = new Dictionary<string, AttributeValue>
            {
                ["email"] = new AttributeValue { S = "testingTwo@gmail.com" },
                ["score"] = new AttributeValue { N = "500" },
                // ["itemsFound"] = new AttributeValue()
                // {
                //     L =
                //     {
                //         new AttributeValue { N = "1"},
                //         new AttributeValue { N = "12"},
                //         new AttributeValue { N = "85"}
                //     }
                // }
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