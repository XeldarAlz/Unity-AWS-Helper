﻿using System;
using System.Collections;
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

        private async void Start()
        {
            try
            {
                bool success = await UseRefreshToken();
                AwsUiManager.Instance.SetFeedbackText($"Auto login attempt: {success}");

                if (success)
                {
                    await GetUserAsync();
                }
            }
            catch (Exception e)
            {
                AwsUiManager.Instance.SetFeedbackText(e.Message);
            }
        }

        public async void Login()
        {
            try
            {
                bool success = await InitiateAuthAsync();

                if (success)
                {
                    DecodeIdToken();
                }

                AwsUiManager.Instance.SetFeedbackText($"Login status: {success}");
            }
            catch (Exception e)
            {
                AwsUiManager.Instance.SetFeedbackText(e.Message);
            }
        }

        private void DecodeIdToken()
        {
            string[] splitToken = AwsSdkManager.Instance.UserIdToken.Split('.');

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
                
                AwsUiManager.Instance.SetFeedbackText($"Hello {user.nickname}");
            }
        }

        private async Task<bool> InitiateAuthAsync()
        {
            var authParameters = new Dictionary<string, string>();
            authParameters.Add("USERNAME", _signInEmail.text);
            authParameters.Add("PASSWORD", _signInPassword.text);

            var authRequest = new InitiateAuthRequest

            {
                ClientId = AwsSdkManager.Instance.GetAppClientId(),
                AuthParameters = authParameters,
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            };

            var response = await AwsSdkManager.Instance.GetCognitoService().InitiateAuthAsync(authRequest);
            
            AwsSdkManager.Instance.UserAccessToken = response.AuthenticationResult.AccessToken;
            AwsSdkManager.Instance.UserIdToken = response.AuthenticationResult.IdToken;
            AwsSdkManager.Instance.UserRefreshToken = response.AuthenticationResult.RefreshToken;
            
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        private async Task<bool> GetUserAsync()
        {
            var currentUserResponse = await AwsSdkManager.Instance.GetCurrentUserResponse();
            var userNickname = currentUserResponse.UserAttributes.Find(attribute => attribute.Name.Equals("nickname")).Value;
            AwsUiManager.Instance.SetFeedbackText($"Successfully logged in. Welcome {userNickname}!");
        
            return currentUserResponse.HttpStatusCode == HttpStatusCode.OK;
        }
        
        private Task<bool> UseRefreshToken()
        {
            var authRequest = new InitiateAuthRequest
            {
                ClientId = AwsSdkManager.Instance.GetAppClientId(),
                AuthParameters = new Dictionary<string, string>
                {
                    { "REFRESH_TOKEN", AwsSdkManager.Instance.UserRefreshToken }
                },
                AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
            };

            var response = AwsSdkManager.Instance.GetCognitoService().InitiateAuthAsync(authRequest).Result;

            if (response.AuthenticationResult != null)
            {
                AwsSdkManager.Instance.UserAccessToken = response.AuthenticationResult.AccessToken;
                AwsSdkManager.Instance.UserIdToken = response.AuthenticationResult.IdToken;
            }
            
            return Task.FromResult(response.HttpStatusCode == HttpStatusCode.OK);
        }
    }
}