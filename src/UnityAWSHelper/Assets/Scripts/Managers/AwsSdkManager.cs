using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.DynamoDBv2;
using Amazon.Lambda;
using Amazon.Runtime;
using Amazon.S3;
using UnityEngine;

namespace Managers
{
    public class AwsSdkManager : MonoBehaviour
    {
        public static AwsSdkManager Instance { get; private set; }

        public string UserAccessToken
        {
            get => PlayerPrefs.GetString("UserAccessToken", default);
            set => PlayerPrefs.SetString("UserAccessToken", value);
        }
        public string UserIdToken
        {
            get => PlayerPrefs.GetString("UserIdToken", default);
            set => PlayerPrefs.SetString("UserIdToken", value);
        }
        public string UserRefreshToken
        {
            get => PlayerPrefs.GetString("UserRefreshToken", default);
            set => PlayerPrefs.SetString("UserRefreshToken", value);
        }
        public string UserIdentityId { get; set; }
        
        // !!!!! If you want to make this project work, change the values below with your own !!!!!
        // COGNITO
        private const string APP_CLIENT_ID = "e702rji3stjh5ril5pfoalcav";
        private const string HOSTED_UI_DOMAIN = "https://evry-testing.auth.eu-west-3.amazoncognito.com";
        private const string USER_POOL_ID = "eu-west-3_VDOEEDFDk";
        private const string IDENTITY_POOL_ID = "eu-west-3:e575706e-3ec2-46d4-ab51-aa9588ef3202";
        private readonly RegionEndpoint _cognitoRegion = RegionEndpoint.EUWest3;
        
        // S3
        private const string PROVIDER_NAME = "cognito-idp.eu-west-3.amazonaws.com";
        
        // DYNAMODB
        // Only for testing purposes. Exposing this data is not good practice because it is sensitive data.
        // Should not be exposed directly. Tables needs to be secure!
        private const string DYNAMO_DB_ACCESS_KEY = "AKIAQ4NSBEGGX6LBTJUN";
        private const string DYNAMO_DB_SECRET_KEY = "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";
        
        // API Gateway
        private const string API_GATEWAY_URI = "https://023ptlkzz8.execute-api.eu-west-3.amazonaws.com/prod";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
        
            Instance = this;
        }

        public string GetAppClientId() => APP_CLIENT_ID;
        public string GetHostedUiDomain() => HOSTED_UI_DOMAIN;
        public string GetApiGatewayUri() => API_GATEWAY_URI;

        public AmazonCognitoIdentityProviderClient GetCognitoService()
        {
            var cognitoServiceCredentials = new AnonymousAWSCredentials();
            return new AmazonCognitoIdentityProviderClient(cognitoServiceCredentials, _cognitoRegion);
        }
        public AmazonLambdaClient GetLambdaClient()
        {
            var lambdaServiceCredentials = new CognitoAWSCredentials(IDENTITY_POOL_ID, _cognitoRegion);
            return new AmazonLambdaClient(lambdaServiceCredentials, _cognitoRegion);
        }
        public AmazonS3Client GetS3Client()
        {
            var credentials = new CognitoAWSCredentials(IDENTITY_POOL_ID, _cognitoRegion);
            
            credentials.AddLogin(
                $"{PROVIDER_NAME}/{USER_POOL_ID}", 
                UserIdToken);

            UserIdentityId = credentials.GetIdentityId();
            return new AmazonS3Client(credentials, _cognitoRegion);
        }
        public AmazonDynamoDBClient GetDynamoDBClient()
        {
            // Also can be used this way.
            // return new AmazonDynamoDBClient(DYNAMO_DB_ACCESS_KEY, DYNAMO_DB_SECRET_KEY, _cognitoRegion);
            
            var credentials = new CognitoAWSCredentials(
                IDENTITY_POOL_ID,
                _cognitoRegion);
            
            credentials.AddLogin(
                $"{PROVIDER_NAME}/{USER_POOL_ID}", 
                UserIdToken);
            
            return new AmazonDynamoDBClient(credentials, _cognitoRegion);
        }
        public async Task<GetUserResponse> GetCurrentUserResponse()
        {
            var getUserRequest = new GetUserRequest()
            {
                AccessToken = UserAccessToken
            };

            return await GetCognitoService().GetUserAsync(getUserRequest);
        }
    }
}