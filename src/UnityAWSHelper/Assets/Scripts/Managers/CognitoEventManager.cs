using UnityEngine;

namespace Managers
{
    public class CognitoEventManager : MonoBehaviour
    {
        public static CognitoEventManager Instance { get; private set; }

        public delegate void CognitoUiStateDelegate();
        public event CognitoUiStateDelegate OnCognitoSignUp;
        public event CognitoUiStateDelegate OnCognitoConfirm;
        public event CognitoUiStateDelegate OnCognitoLogin;
        public event CognitoUiStateDelegate OnCognitoDelete;
        public event CognitoUiStateDelegate OnCognitoRecover;
        public event CognitoUiStateDelegate OnCognitoConfirmRecover;
        public event CognitoUiStateDelegate OnCognitoHostedUi;
        public event CognitoUiStateDelegate OnCognitoLogout;
        public event CognitoUiStateDelegate OnCognitoLambda;

        public void CognitoSignUp() => OnCognitoSignUp?.Invoke();
        public void CognitoConfirm() => OnCognitoConfirm?.Invoke();
        public void CognitoLogin() => OnCognitoLogin?.Invoke();
        public void CognitoDelete() => OnCognitoDelete?.Invoke();
        public void CognitoRecover() => OnCognitoRecover?.Invoke();
        public void CognitoConfirmRecover() => OnCognitoConfirmRecover?.Invoke();
        public void CognitoHostedUi() => OnCognitoHostedUi?.Invoke();
        public void CognitoLogout() => OnCognitoLogout?.Invoke();
        public void CognitoLambda() => OnCognitoLambda?.Invoke();
     
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