using System;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider.Model;
using Managers;
using UnityEngine;

namespace UiPanels
{
    public class CognitoLogout : MonoBehaviour
    {
        public async void GlobalSignOut()
        {
            try
            {
                bool success = await GlobalSignOutAsync();
                AwsUiManager.Instance.SetFeedbackText($"Global sign-out status: {success}");
            }
            catch (Exception e)
            {
                AwsUiManager.Instance.SetFeedbackText(e.Message);
            }
        }

        public async void PartialSignOut()
        {
            try
            {
                bool success = await PartialSignOutAsync();
                AwsUiManager.Instance.SetFeedbackText($"Partial sign-out status: {success}");
            }
            catch (Exception e)
            {
                AwsUiManager.Instance.SetFeedbackText(e.Message);
            }
        }
        
        private async Task<bool> GlobalSignOutAsync()
        {
            var signOutRequest = new GlobalSignOutRequest()
            {
                AccessToken = AwsSdkManager.Instance.UserAccessToken
            };

            var response = await AwsSdkManager.Instance.GetCognitoService().GlobalSignOutAsync(signOutRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        
        private async Task<bool> PartialSignOutAsync()
        {
            var revokeTokenRequest = new RevokeTokenRequest()
            {
                ClientId = AwsSdkManager.Instance.GetAppClientId(),
                Token = AwsSdkManager.Instance.UserRefreshToken
            };

            var response = await AwsSdkManager.Instance.GetCognitoService().RevokeTokenAsync(revokeTokenRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}