using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Runtime;
using UnityEngine;

namespace Managers
{
    public class CognitoSdkManager : MonoBehaviour
    {
        public static CognitoSdkManager Instance { get; private set; }
    
        public string UserAccessToken { get; set; }
        public string UserIdToken { get; set; }
        public string UserRefreshToken { get; set; }
        
        // Change these with new cognito config on Amazon Cognito Console
        private const string APP_CLIENT_ID = "e702rji3stjh5ril5pfoalcav";
        private const string HOSTED_UI_DOMAIN = "https://evry-testing.auth.eu-west-3.amazoncognito.com";
        private const string COGNITO_IDENTITY_POOL_ID = "eu-west-3:e575706e-3ec2-46d4-ab51-aa9588ef3202";

        private AmazonCognitoIdentityProviderClient _cognitoService;
        private AmazonLambdaClient _lambdaService;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
        
            Instance = this;

            // Change the second parameter with the correct region stated in the Amazon Cognito Console
            _cognitoService = new AmazonCognitoIdentityProviderClient(
                new AnonymousAWSCredentials(), RegionEndpoint.EUWest3);
            
            _lambdaService = new AmazonLambdaClient(new CognitoAWSCredentials(
                COGNITO_IDENTITY_POOL_ID, RegionEndpoint.EUWest3), RegionEndpoint.EUWest3);
        }

        public string GetAppClientId() => APP_CLIENT_ID;
        public string GetHostedUiDomain() => HOSTED_UI_DOMAIN;
        public AmazonCognitoIdentityProviderClient GetCognitoService() => _cognitoService;
        public AmazonLambdaClient GetLambdaService() => _lambdaService;
    }
}