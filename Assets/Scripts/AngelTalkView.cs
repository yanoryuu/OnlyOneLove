using TMPro;
using UnityEngine;

public class AngelTalkView : MonoBehaviour
{
    [SerializeField] private GameObject angelTalkGroup;
    
    [SerializeField] private TextMeshProUGUI angelText;
    
    //女の子の返答を出力
    public void ShowAITalk(string talk)
    {
        angelText.text = talk;
    }

    //返答画面を表示
    public void Show()
    {
        angelTalkGroup.SetActive(true);
    }

    //返答画面を非表示
    public void Hide()
    {
        angelTalkGroup.SetActive(false);
    }
}
