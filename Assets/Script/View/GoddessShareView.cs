using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UnityEngine.SceneManagement;

namespace MVP
{
    /// <summary>
    /// シーン上の UI と紐づく View 実装
    /// </summary>
    public class GoddessShareView : MonoBehaviour, IGoddessShareView
    {
        [Header("UI Elements (TMP)")]
        [SerializeField] TMP_InputField idInputField;
        [SerializeField] Button         submitButton;

        [Header("Confirmation Panel")]
        [SerializeField] GameObject     confirmPanel;
        [SerializeField] Button         yesButton;
        [SerializeField] Button         noButton;

        readonly Subject<string> _onSubmitID = new Subject<string>();
        public IObservable<string> OnSubmitID => _onSubmitID;

        void Awake()
        {
            // パネルは最初非表示
            confirmPanel.SetActive(false);

            // Presenter を生成
            var service   = new MockGoddessService();
            var presenter = new GoddessSharePresenter(this, service);

            // 送信ボタン押下で確認パネル表示
            submitButton.onClick.AddListener(() =>
            {
                confirmPanel.SetActive(true);
            });

            // No ボタンでパネルを閉じる
            noButton.onClick.AddListener(() =>
            {
                confirmPanel.SetActive(false);
            });

            // Yes ボタンでシーンをリロード
            yesButton.onClick.AddListener(() =>
            {
                confirmPanel.SetActive(false);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }

        void OnDestroy()
        {
            _onSubmitID.Dispose();
        }
    }
}