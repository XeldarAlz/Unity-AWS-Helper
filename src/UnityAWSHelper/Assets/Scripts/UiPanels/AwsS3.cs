using System;
using System.Threading.Tasks;
using Amazon.CognitoIdentity;
using Amazon.S3;
using Amazon.S3.Model;
using Managers;
using UnityEngine;

namespace UiPanels
{
    public class AwsS3 : MonoBehaviour
    {
        public void PickImage()
        {
            try
            {
                NativeGallery.Permission permission = NativeGallery.GetImageFromGallery(async (path) =>
                {
                    bool success = await UploadFileAsync(path);
                    AwsUiManager.Instance.SetFeedbackText($"Upload status: {success}");
                });
            }
            catch (Exception e)
            {
                AwsUiManager.Instance.SetFeedbackText(e.Message);
            }
        }
        
        public static async Task<bool> UploadFileAsync(string filePath)
        {
            var client = AwsSdkManager.Instance.GetS3Client();
            
            // Change the values with the ones from Cognito S3 Bucket you have created
            var request = new PutObjectRequest
            {
                BucketName = "evry-unity-cognito",
                Key = $"{AwsSdkManager.Instance.UserIdentityId}/profile_picture.jpg",
                FilePath = filePath,
            };

            var response = await client.PutObjectAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}