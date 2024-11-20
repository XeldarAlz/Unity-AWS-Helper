using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider.Model;
using Managers;
using TMPro;
using UnityEngine;

namespace UiPanels
{
    public class CognitoConfirmRecover : MonoBehaviour
    {
        [Header("Confirm Recover")]
        [SerializeField] private TMP_InputField _recoverEmail;
        [SerializeField] private TMP_InputField _recoverCode;
        [SerializeField] private TMP_InputField _recoverNewPassword;
        
        public async void ConfirmRecover()
        {
            try
            {
                bool success = await ConfirmForgotPasswordAsync();
                AwsUiManager.Instance.SetFeedbackText($"Confirm recovery status: {success}");
            }
            catch (Exception e)
            {
                AwsUiManager.Instance.SetFeedbackText(e.Message);
            }
        }

        private async Task<bool> ConfirmForgotPasswordAsync()
        {
            var confirmRecoverRequest = new ConfirmForgotPasswordRequest()
            {
                Username = _recoverEmail.text,
                ClientId = AwsSdkManager.Instance.GetAppClientId(),
                ConfirmationCode = _recoverCode.text,
                Password = _recoverNewPassword.text
            };

            var response = await AwsSdkManager.Instance.GetCognitoService()
                .ConfirmForgotPasswordAsync(confirmRecoverRequest);
            
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}