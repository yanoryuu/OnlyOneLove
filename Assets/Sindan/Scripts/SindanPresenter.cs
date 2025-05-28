using UnityEngine;
using Utage;
using Cysharp.Threading.Tasks;

public class SindanPresenter : MonoBehaviour
{
    [SerializeField] private AdvEngine engine;
    [SerializeField] private AdvScenarioPlayer scenarioPlayer;
    [SerializeField] private SindanView view;
    [SerializeField] private SindanModel model;

    private AdvCommandSendMessage currentCommand;

    void Awake()
    {
        scenarioPlayer.SendMessageTarget = this.gameObject;
    }

    // SendMessage コマンドを受け取ったとき
    async void OnDoCommand(AdvCommandSendMessage command)
    {
        Debug.Log($"OnDoCommand: {command.Name} Arg2={command.Arg2}");
        switch (command.Name)
        {
            case "InputField":
                // ユーザー入力を待って、そのまま Utage パラメータにセット
                currentCommand = command;
                {
                    string input = await view.WaitForSubmitAsync();
                    engine.Param.TrySetParameter(command.Arg2, input);
                }
                break;

            case "ChatGpt":
                // モデルに渡して GPT 応答を取得 → Utage パラメータにセット
                currentCommand = command;
                {
                    string prompt = command.Text;
                    string reply  = await model.SendMessageAsync(prompt);
                    engine.Param.TrySetParameter(command.Arg2, reply);
                }
                break;
        }

        // どちらも終わったら待機解除
        command.IsWait = false;
        currentCommand = null;
    }

    // OnWait も両方チェック
    void OnWait(AdvCommandSendMessage command)
    {
        if (command.Name == "InputField" || command.Name == "ChatGpt")
        {
            // ChatGPT のときは実ユーザー入力はなし → ずっと待機されてしまうので false 固定
            if (command.Name == "InputField")
                command.IsWait = view.IsInputActive;
            else
                command.IsWait = true; // ChatGPT は通信中ずっと待機
        }
    }
}
