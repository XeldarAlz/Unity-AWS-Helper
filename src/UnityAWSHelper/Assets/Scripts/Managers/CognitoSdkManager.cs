﻿using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.DynamoDBv2;
using Amazon.Lambda;
using Amazon.Runtime;
using Amazon.S3;
using UnityEngine;

namespace Managers
{
    public class CognitoSdkManager : MonoBehaviour
    {
        public static CognitoSdkManager Instance { get; private set; }
    
        public string UserAccessToken { get; set; }
        public string UserIdToken { get; set; }
        public string UserRefreshToken { get; set; }
        
        // Change these with new config on AWS Console
        // COGNITO
        private const string APP_CLIENT_ID = "e702rji3stjh5ril5pfoalcav";
        private const string HOSTED_UI_DOMAIN = "https://evry-testing.auth.eu-west-3.amazoncognito.com";
        private const string USER_POOL_ID = "eu-west-3_VDOEEDFDk";
        private const string IDENTITY_POOL_ID = "eu-west-3:e575706e-3ec2-46d4-ab51-aa9588ef3202";
        private readonly RegionEndpoint _cognitoRegion = RegionEndpoint.EUWest3;
        
        // S3
        private const string PROVIDER_NAME = "cognito-idp.eu-west-3.amazonaws.com"; //
        
        // DYNAMODB
        // Only for testing purposes. Exposing this data is not good practice because it is sensitive data.
        // Should not be exposed directly. Tables needs to be secure!
        private const string DYNAMO_DB_ACCESS_KEY = "AKIAQ4NSBEGGX6LBTJUN";
        private const string DYNAMO_DB_SECRET_KEY = "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";

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
            
            return new AmazonS3Client(credentials, _cognitoRegion);
        }
        public AmazonDynamoDBClient GetDynamoDBClient()
        {
            return new AmazonDynamoDBClient(DYNAMO_DB_ACCESS_KEY, DYNAMO_DB_SECRET_KEY, _cognitoRegion);
        }
    }
}