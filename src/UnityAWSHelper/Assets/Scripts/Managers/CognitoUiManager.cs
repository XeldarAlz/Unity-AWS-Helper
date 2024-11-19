using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class CognitoUiManager : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<CognitoPanel, GameObject> _panels;
        [SerializeField] private TextMeshProUGUI _uiFeedbackText;

        public enum CognitoPanel
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
            DynamoDB
        }

        public static CognitoUiManager Instance { get; private set; }

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
            CognitoEventManager.Instance.OnCognitoSignUp += () => ShowPanel(CognitoPanel.SignUp);
            CognitoEventManager.Instance.OnCognitoConfirm += () => ShowPanel(CognitoPanel.Confirm);
            CognitoEventManager.Instance.OnCognitoLogin += () => ShowPanel(CognitoPanel.Login);
            CognitoEventManager.Instance.OnCognitoDelete += () => ShowPanel(CognitoPanel.Delete);
            CognitoEventManager.Instance.OnCognitoRecover += () => ShowPanel(CognitoPanel.Recover);
            CognitoEventManager.Instance.OnCognitoConfirmRecover += () => ShowPanel(CognitoPanel.ConfirmRecover);
            CognitoEventManager.Instance.OnCognitoHostedUi += () => ShowPanel(CognitoPanel.HostedUi);
            CognitoEventManager.Instance.OnCognitoLogout += () => ShowPanel(CognitoPanel.Logout);
            CognitoEventManager.Instance.OnCognitoLambda += () => ShowPanel(CognitoPanel.Lambda);
            CognitoEventManager.Instance.OnCognitoS3 += () => ShowPanel(CognitoPanel.S3);
            CognitoEventManager.Instance.OnCognitoDynamoDB += () => ShowPanel(CognitoPanel.DynamoDB);
        }

        private void OnDisable()
        {
            CognitoEventManager.Instance.OnCognitoSignUp -= () => ShowPanel(CognitoPanel.SignUp);
            CognitoEventManager.Instance.OnCognitoConfirm -= () => ShowPanel(CognitoPanel.Confirm);
            CognitoEventManager.Instance.OnCognitoLogin -= () => ShowPanel(CognitoPanel.Login);
            CognitoEventManager.Instance.OnCognitoDelete -= () => ShowPanel(CognitoPanel.Delete);
            CognitoEventManager.Instance.OnCognitoRecover -= () => ShowPanel(CognitoPanel.Recover);
            CognitoEventManager.Instance.OnCognitoConfirmRecover -= () => ShowPanel(CognitoPanel.ConfirmRecover);
            CognitoEventManager.Instance.OnCognitoHostedUi -= () => ShowPanel(CognitoPanel.HostedUi);
            CognitoEventManager.Instance.OnCognitoLogout -= () => ShowPanel(CognitoPanel.Logout);
            CognitoEventManager.Instance.OnCognitoLambda -= () => ShowPanel(CognitoPanel.Lambda);
            CognitoEventManager.Instance.OnCognitoS3 -= () => ShowPanel(CognitoPanel.S3);
            CognitoEventManager.Instance.OnCognitoDynamoDB -= () => ShowPanel(CognitoPanel.DynamoDB);
        }

        private void Start()
        {
            HideAllPanels();
            SetFeedbackText(string.Empty);
        }

        public void ShowPanel(CognitoPanel panelToShow)
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