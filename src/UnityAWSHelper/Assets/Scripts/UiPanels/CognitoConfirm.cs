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
            bool success = await ConfirmSignupAsync();
        
            CognitoUiManager.Instance.SetFeedbackText(success
                ? "Successfully confirmed user"
                : "An error occured while confirming user");
        }

        private async Task<bool> ConfirmSignupAsync()
        {
            var signUpRequest = new ConfirmSignUpRequest
            {
                ClientId = CognitoSdkManager.Instance.GetAppClientId(),
                ConfirmationCode = _confirmSignUpCode.text,
                Username = _confirmSignUpEmail.text
            };

            var response = await CognitoSdkManager.Instance.GetCognitoService().ConfirmSignUpAsync(signUpRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}