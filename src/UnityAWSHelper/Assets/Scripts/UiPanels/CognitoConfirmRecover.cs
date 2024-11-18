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
            bool success = await ConfirmForgotPasswordAsync();
            CognitoUiManager.Instance.SetFeedbackText($"Successfully confirmed account recovery!");
        }

        private async Task<bool> ConfirmForgotPasswordAsync()
        {
            var confirmRecoverRequest = new ConfirmForgotPasswordRequest()
            {
                Username = _recoverEmail.text,
                ClientId = CognitoSdkManager.Instance.GetAppClientId(),
                ConfirmationCode = _recoverCode.text,
                Password = _recoverNewPassword.text
            };

            var response = await CognitoSdkManager.Instance.GetCognitoService()
                .ConfirmForgotPasswordAsync(confirmRecoverRequest);
            
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}