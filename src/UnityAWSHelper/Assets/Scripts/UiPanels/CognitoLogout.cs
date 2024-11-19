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
            bool success = await GlobalSignOutAsync();
            CognitoUiManager.Instance.SetFeedbackText($"Global sign-out status: {success}");
        }

        public async void PartialSignOut()
        {
            bool success = await PartialSignOutAsync();
            CognitoUiManager.Instance.SetFeedbackText($"Partial sign-out status: {success}");
        }
        
        private async Task<bool> GlobalSignOutAsync()
        {
            var signOutRequest = new GlobalSignOutRequest()
            {
                AccessToken = CognitoSdkManager.Instance.UserAccessToken
            };

            var response = await CognitoSdkManager.Instance.GetCognitoService().GlobalSignOutAsync(signOutRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        
        private async Task<bool> PartialSignOutAsync()
        {
            var revokeTokenRequest = new RevokeTokenRequest()
            {
                ClientId = CognitoSdkManager.Instance.GetAppClientId(),
                Token = CognitoSdkManager.Instance.UserRefreshToken
            };

            var response = await CognitoSdkManager.Instance.GetCognitoService().RevokeTokenAsync(revokeTokenRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}