﻿using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider.Model;
using Managers;
using TMPro;
using UnityEngine;

namespace UiPanels
{
    public class CognitoRecover : MonoBehaviour
    {
        [Header("Recover")]
        [SerializeField] private TMP_InputField _recoverEmail;

        public async void Recover()
        {
            try
            {
                bool success = await ForgotPasswordAsync();
                AwsUiManager.Instance.SetFeedbackText($"Recovery status: {success}");
            }
            catch (Exception e)
            {
                AwsUiManager.Instance.SetFeedbackText(e.Message);
            }
        }
        
        private async Task<bool> ForgotPasswordAsync()
        {
            var recoveryRequest = new ForgotPasswordRequest()
            {
                Username = _recoverEmail.text,
                ClientId = AwsSdkManager.Instance.GetAppClientId()
            };

            var response = await AwsSdkManager.Instance.GetCognitoService().ForgotPasswordAsync(recoveryRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}