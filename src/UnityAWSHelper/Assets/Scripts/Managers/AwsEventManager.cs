using UnityEngine;

namespace Managers
{
    public class AwsEventManager : MonoBehaviour
    {
        public static AwsEventManager Instance { get; private set; }

        public delegate void UiStateDelegate();
        public event UiStateDelegate OnCognitoSignUp;
        public event UiStateDelegate OnCognitoConfirm;
        public event UiStateDelegate OnCognitoLogin;
        public event UiStateDelegate OnCognitoDelete;
        public event UiStateDelegate OnCognitoRecover;
        public event UiStateDelegate OnCognitoConfirmRecover;
        public event UiStateDelegate OnCognitoHostedUi;
        public event UiStateDelegate OnCognitoLogout;
        public event UiStateDelegate OnLambda;
        public event UiStateDelegate OnS3;
        public event UiStateDelegate OnDynamoDB;
        public event UiStateDelegate OnApiGateway;

        public void CognitoSignUp() => OnCognitoSignUp?.Invoke();
        public void CognitoConfirm() => OnCognitoConfirm?.Invoke();
        public void CognitoLogin() => OnCognitoLogin?.Invoke();
        public void CognitoDelete() => OnCognitoDelete?.Invoke();
        public void CognitoRecover() => OnCognitoRecover?.Invoke();
        public void CognitoConfirmRecover() => OnCognitoConfirmRecover?.Invoke();
        public void CognitoHostedUi() => OnCognitoHostedUi?.Invoke();
        public void CognitoLogout() => OnCognitoLogout?.Invoke();
        public void Lambda() => OnLambda?.Invoke();
        public void S3() => OnS3?.Invoke();
        public void DynamoDB() => OnDynamoDB?.Invoke();
        public void ApiGateway() => OnApiGateway?.Invoke();
     
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
        
            Instance = this;
        }
    }
}