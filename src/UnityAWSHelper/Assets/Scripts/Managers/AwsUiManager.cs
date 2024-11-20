using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class AwsUiManager : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<UiPanel, GameObject> _panels;
        [SerializeField] private TextMeshProUGUI _uiFeedbackText;

        public enum UiPanel
        {
            SignUp,
            Confirm,
            Login,
            Delete,
            Recover,
            ConfirmRecover,
            HostedUi,
            Logout,
            Lambda,
            S3,
            DynamoDB,
            ApiGateway
        }

        public static AwsUiManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            AwsEventManager.Instance.OnCognitoSignUp += () => ShowPanel(UiPanel.SignUp);
            AwsEventManager.Instance.OnCognitoConfirm += () => ShowPanel(UiPanel.Confirm);
            AwsEventManager.Instance.OnCognitoLogin += () => ShowPanel(UiPanel.Login);
            AwsEventManager.Instance.OnCognitoDelete += () => ShowPanel(UiPanel.Delete);
            AwsEventManager.Instance.OnCognitoRecover += () => ShowPanel(UiPanel.Recover);
            AwsEventManager.Instance.OnCognitoConfirmRecover += () => ShowPanel(UiPanel.ConfirmRecover);
            AwsEventManager.Instance.OnCognitoHostedUi += () => ShowPanel(UiPanel.HostedUi);
            AwsEventManager.Instance.OnCognitoLogout += () => ShowPanel(UiPanel.Logout);
            AwsEventManager.Instance.OnLambda += () => ShowPanel(UiPanel.Lambda);
            AwsEventManager.Instance.OnS3 += () => ShowPanel(UiPanel.S3);
            AwsEventManager.Instance.OnDynamoDB += () => ShowPanel(UiPanel.DynamoDB);
            AwsEventManager.Instance.OnApiGateway += () => ShowPanel(UiPanel.ApiGateway);
        }

        private void OnDisable()
        {
            AwsEventManager.Instance.OnCognitoSignUp -= () => ShowPanel(UiPanel.SignUp);
            AwsEventManager.Instance.OnCognitoConfirm -= () => ShowPanel(UiPanel.Confirm);
            AwsEventManager.Instance.OnCognitoLogin -= () => ShowPanel(UiPanel.Login);
            AwsEventManager.Instance.OnCognitoDelete -= () => ShowPanel(UiPanel.Delete);
            AwsEventManager.Instance.OnCognitoRecover -= () => ShowPanel(UiPanel.Recover);
            AwsEventManager.Instance.OnCognitoConfirmRecover -= () => ShowPanel(UiPanel.ConfirmRecover);
            AwsEventManager.Instance.OnCognitoHostedUi -= () => ShowPanel(UiPanel.HostedUi);
            AwsEventManager.Instance.OnCognitoLogout -= () => ShowPanel(UiPanel.Logout);
            AwsEventManager.Instance.OnLambda -= () => ShowPanel(UiPanel.Lambda);
            AwsEventManager.Instance.OnS3 -= () => ShowPanel(UiPanel.S3);
            AwsEventManager.Instance.OnDynamoDB -= () => ShowPanel(UiPanel.DynamoDB);
            AwsEventManager.Instance.OnApiGateway -= () => ShowPanel(UiPanel.ApiGateway);
        }

        private void Start()
        {
            HideAllPanels();
            SetFeedbackText(string.Empty);
        }

        public void ShowPanel(UiPanel panelToShow)
        {
            foreach (var panel in _panels)
            {
                panel.Value.SetActive(panel.Key == panelToShow);
            }

            SetFeedbackText(string.Empty);
        }

        private void HideAllPanels()
        {
            foreach (var panel in _panels.Values)
            {
                panel.SetActive(false);
            }
        }

        public void SetFeedbackText(string text) => _uiFeedbackText.text = text;
    }
}