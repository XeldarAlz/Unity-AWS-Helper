using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider.Model;
using Managers;
using TMPro;
using UnityEngine;

namespace UiPanels
{
    public class CognitoConfirm : MonoBehaviour
    {
        [Header("Confirm")] 
        [SerializeField] private TMP_InputField _confirmSignUpEmail;
        [SerializeField] private TMP_InputField _confirmSignUpCode;

        public async void ConfirmSignUp()
        {
            try
            {
                bool success = await ConfirmSignupAsync();
                AwsUiManager.Instance.SetFeedbackText($"Confirm status: {success}");
            }
            catch (Exception e)
            {
                AwsUiManager.Instance.SetFeedbackText(e.Message);
            }
        }

        private async Task<bool> ConfirmSignupAsync()
        {
            var signUpRequest = new ConfirmSignUpRequest
            {
                ClientId = AwsSdkManager.Instance.GetAppClientId(),
                ConfirmationCode = _confirmSignUpCode.text,
                Username = _confirmSignUpEmail.text
            };

            var response = await AwsSdkManager.Instance.GetCognitoService().ConfirmSignUpAsync(signUpRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}