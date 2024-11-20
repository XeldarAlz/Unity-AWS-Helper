using System.Collections;
using System.Web;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UiPanels
{
    public class CognitoHostedUI : MonoBehaviour
    {
        [Header("Hosted UI")]
        [SerializeField] private TextMeshProUGUI _signInText;
        [SerializeField] private Image _profilePicture;

        private string _accessToken;

        private void Start()
        {
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                OnDeepLinkActivated(Application.absoluteURL);
            }
        }

        private void OnEnable()
        {
            Application.deepLinkActivated += OnDeepLinkActivated;
        }

        private void OnDisable()
        {
            Application.deepLinkActivated -= OnDeepLinkActivated;
        }

        private void OnDeepLinkActivated(string url)
        {
            Debug.Log($"DeepLinkActivated:{Application.absoluteURL}");

            string[] splittedUrl = url.Split('#');

            if (splittedUrl.Length == 2)
            {
                Debug.Log($"Splitted url length: {splittedUrl.Length} ");

                string urlParams = splittedUrl[1];

                if (!string.IsNullOrEmpty(urlParams))
                {
                    _accessToken = HttpUtility.ParseQueryString(urlParams).Get("access_token");

                    if (!string.IsNullOrEmpty(_accessToken))
                    {
                        StartCoroutine(GetUserInfo());
                    }
                    else
                    {
                        Debug.Log($"Access token is null");
                    }
               
                    _signInText.text = _accessToken;
                }
            }
        }

        private IEnumerator GetUserInfo()
        {
            using (var webRequest =
                   UnityWebRequest.Get($"{AwsSdkManager.Instance.GetHostedUiDomain()}/oauth2/userInfo"))
            {
                webRequest.SetRequestHeader("Content-Type", "application/x-amz-json-1.1; charset=UTF-8");
                webRequest.SetRequestHeader("Authorization", $"Bearer {_accessToken}");

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log($"Web request success");

                    CognitoHostedUiUser user = JsonUtility.FromJson<CognitoHostedUiUser>(webRequest.downloadHandler.text);
                    Debug.Log($"Web request download handler: {webRequest.downloadHandler.text}");
                    
                    _signInText.text = $"Welcome {user.nickname}, You are now logged in!";
                    
                    if (!string.IsNullOrEmpty(user.picture))
                    {
                        StartCoroutine(GetProfilePicture(user.picture));
                    }
                }
                else
                {
                    Debug.Log($"Web request failed: {webRequest.error}");
                }
            }
        }

        private IEnumerator GetProfilePicture(string pictureURL)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(pictureURL))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log($"Web request failed for profile picture: {uwr.error}");
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    _profilePicture.sprite = sprite;
                }
            }
        }

        public void OpenCognitoHostedUI()
        {
            string callbackUrl = "my-demo-app://login";

            Application.OpenURL(
                $"{AwsSdkManager.Instance.GetHostedUiDomain()}/oauth2/authorize?client_id={AwsSdkManager.Instance.GetAppClientId()}&response_type=token&scope=aws.cognito.signin.user.admin+openid&redirect_uri={callbackUrl}");
        }
        
        public void LoginWithGoogle()
        {
            string callbackUrl = "my-demo-app://login";

            Application.OpenURL(
                $"{AwsSdkManager.Instance.GetHostedUiDomain()}/oauth2/authorize?identity_provider=Google&client_id={AwsSdkManager.Instance.GetAppClientId()}&response_type=token&scope=aws.cognito.signin.user.admin+openid&redirect_uri={callbackUrl}");
        }
    }

    public class CognitoHostedUiUser
    {
        public string nickname;
        public string picture;
    }
}