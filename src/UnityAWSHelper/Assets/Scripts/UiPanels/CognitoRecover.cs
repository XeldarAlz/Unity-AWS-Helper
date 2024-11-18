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
            bool success = await ForgotPasswordAsync();
            CognitoUiManager.Instance.SetFeedbackText($"Recovery status: Successful");
        }
        
        private async Task<bool> ForgotPasswordAsync()
        {
            var recoveryRequest = new ForgotPasswordRequest()
            {
                Username = _recoverEmail.text,
                ClientId = CognitoSdkManager.Instance.GetAppClientId()
            };

            var response = await CognitoSdkManager.Instance.GetCognitoService().ForgotPasswordAsync(recoveryRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}