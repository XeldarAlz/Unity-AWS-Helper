using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Managers;
using TMPro;
using UnityEngine;

namespace UiPanels
{
    public class CognitoLogin : MonoBehaviour
    {
        [Header("Login")]
        [SerializeField] private TMP_InputField _signInEmail;
        [SerializeField] private TMP_InputField _signInPassword;

        public async void Login()
        {
            bool success = await InitiateAuthAsync();
            Debug.Log($"Login result: {success}");

            // success = await GetUserAsync();
            if (success)
            {
                DecodeIdToken();
            }
        }

        private void DecodeIdToken()
        {
            string[] splitToken = CognitoSdkManager.Instance.UserIdToken.Split('.');

            if (splitToken.Length == 3)
            {
                string payload = splitToken[1];
                int remainder = payload.Length % 4;

                if (remainder > 0)
                {
                    int missingChars = 4 - remainder;
                    payload += new string('=', missingChars);
                }

                byte[] bytes = System.Convert.FromBase64String(payload);
                string dataStr = System.Text.UTF8Encoding.UTF8.GetString(bytes);
                CognitoHostedUiUser user = JsonUtility.FromJson<CognitoHostedUiUser>(dataStr);
                
                CognitoUiManager.Instance.SetFeedbackText($"Hello {user.nickname}");
            }
        }

        private async Task<bool> InitiateAuthAsync()
        {
            var authParameters = new Dictionary<string, string>();
            authParameters.Add("USERNAME", _signInEmail.text);
            authParameters.Add("PASSWORD", _signInPassword.text);

            var authRequest = new InitiateAuthRequest

            {
                ClientId = CognitoSdkManager.Instance.GetAppClientId(),
                AuthParameters = authParameters,
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            };

            var response = await CognitoSdkManager.Instance.GetCognitoService().InitiateAuthAsync(authRequest);
            
            CognitoSdkManager.Instance.UserAccessToken = response.AuthenticationResult.AccessToken;
            CognitoSdkManager.Instance.UserIdToken = response.AuthenticationResult.IdToken;
            CognitoSdkManager.Instance.UserRefreshToken = response.AuthenticationResult.RefreshToken;
            
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        private async Task<bool> GetUserAsync()
        {
            var getUserRequest = new GetUserRequest()
            {
                AccessToken = CognitoSdkManager.Instance.UserAccessToken
            };

            var response = await CognitoSdkManager.Instance.GetCognitoService().GetUserAsync(getUserRequest);
            var userNickname = response.UserAttributes.Find(attribute => attribute.Name.Equals("nickname")).Value;
            CognitoUiManager.Instance.SetFeedbackText($"Successfully logged in. Welcome {userNickname}!");
        
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}