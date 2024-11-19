using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Managers;
using UnityEngine;

namespace UiPanels
{
    public class CognitoDelete : MonoBehaviour
    {
        public async void DeleteUser()
        {
            bool success = await DeleteUserAsync();

            if (!success)
            {
                success = await DeleteUserAsync();
            }
            
            CognitoUiManager.Instance.SetFeedbackText($"Delete status: {success}");
        }
        
        private async Task<bool> DeleteUserAsync()
        {
            try
            {
                var deleteUserRequest = new DeleteUserRequest()
                {
                    AccessToken = CognitoSdkManager.Instance.UserAccessToken
                };

                var response = await CognitoSdkManager.Instance.GetCognitoService().DeleteUserAsync(deleteUserRequest);
                return response.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                CognitoUiManager.Instance.SetFeedbackText($"Got exception: {e}, Delete status: {e.Message}");
                Debug.Log(e.Message);
                bool success = await RefreshAuthAsync();
                Debug.Log($"Refresh status: {success}");
                return false;
            }
        }
        
        private async Task<bool> RefreshAuthAsync()
        {
            var authParameters = new Dictionary<string, string>();
            authParameters.Add("REFRESH_TOKEN", CognitoSdkManager.Instance.UserRefreshToken);

            var authRequest = new InitiateAuthRequest
            {
                ClientId = CognitoSdkManager.Instance.GetAppClientId(),
                AuthParameters = authParameters,
                AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
            };

            var response = await CognitoSdkManager.Instance.GetCognitoService().InitiateAuthAsync(authRequest);
            
            CognitoSdkManager.Instance.UserAccessToken = response.AuthenticationResult.AccessToken;
            CognitoSdkManager.Instance.UserRefreshToken = response.AuthenticationResult.RefreshToken;
            
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}