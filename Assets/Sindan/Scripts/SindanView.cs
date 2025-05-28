using UnityEngine;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;

public class SindanView : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private InputField inputField;
    [SerializeField] private Button sendButton;

    private UniTaskCompletionSource<string> tcs;

    public bool IsInputActive => panel.activeSelf;

    void Awake()
    {
        panel.SetActive(false);
        sendButton.onClick.AddListener(() =>
        {
            string text = inputField.text.Trim();
            if (string.IsNullOrEmpty(text)) return;
            tcs?.TrySetResult(text);
        });
    }

    public async UniTask<string> WaitForSubmitAsync()
    {
        tcs = new UniTaskCompletionSource<string>();
        panel.SetActive(true);

        inputField.text = "";
        inputField.ActivateInputField();

        string result = await tcs.Task;

        panel.SetActive(false);
        tcs = null;
        return result;
    }
}
