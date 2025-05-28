using UnityEngine;

public class StartConversation : MonoBehaviour
{
    public SampleAdvEngineController advController;
    public string startLabel = "Start"; // 最初に再生したいシナリオラベル名（Utageの*.advシナリオのラベル）
    void Start()
    {
        //string label = !string.IsNullOrEmpty(SceneTransition.NextScenarioLabel) ? SceneTransition.NextScenarioLabel : startLabel;

        if (advController != null && !string.IsNullOrEmpty(startLabel))
        {
            advController.JumpScenario(startLabel);
            //SceneTransition.NextScenarioLabel = null; // 使い終わったらリセット
        }
        else
        {
            Debug.LogError("advController が設定されていないか、ラベルが空です");
        }
    }

    /*public SampleAdvEngineController advController;
    public string startLabel = "Start"; // 最初に再生したいシナリオラベル名（Utageの*.advシナリオのラベル）

    void Start()
    {
        if (advController != null && !string.IsNullOrEmpty(startLabel))
        {
            advController.JumpScenario(startLabel);
        }
        else
        {
            Debug.LogError("advController が設定されていないか、startLabel が空です");
        }
    }*/

}
