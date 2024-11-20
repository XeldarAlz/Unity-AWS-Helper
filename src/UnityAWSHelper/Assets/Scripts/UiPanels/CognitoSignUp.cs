using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider.Model;
using Managers;
using TMPro;
using UnityEngine;

namespace UiPanels
{
    public class CognitoSignUp : MonoBehaviour
    {
        [Header("Sign Up")]
        [SerializeField] private TMP_InputField _signUpEmail;
        [SerializeField] private TMP_InputField _signUpPassword;
        [SerializeField] private TMP_InputField _signUpNickname;
    
        public async void SignUp()
        {
            bool success = await SignUpAsync();

            AwsUiManager.Instance.SetFeedbackText(success
                ? "Successfully signed up"
                : "An error occured while signing up");
        }
    
        private async Task<bool> SignUpAsync()
        {
            var usernameAttrs = new AttributeType
            {
                Name = "nickname",
                Value = _signUpNickname.text
            };
        
            var localeAttrs = new AttributeType
            {
                Name = "locale",
                Value = Application.systemLanguage.ToString()
            };
        
            var versionAttrs = new AttributeType
            {
                Name = "custom:app_version",
                Value = Application.unityVersion
            };
        
            var userAttrsList = new List<AttributeType>
            {
                usernameAttrs,
                localeAttrs,
                versionAttrs
            };

            var signUpRequest = new SignUpRequest
            {
                UserAttributes = userAttrsList,
                Username = _signUpEmail.text,
                ClientId = AwsSdkManager.Instance.GetAppClientId(),
                Password = _signUpPassword.text
            };

            var response = await AwsSdkManager.Instance.GetCognitoService().SignUpAsync(signUpRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}