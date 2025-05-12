using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

namespace Ingame
{
    public class InGameView : MonoBehaviour
    {
        [SerializeField] private Button talkButton;
        public Button TalkButton => talkButton;
        
        [SerializeField] private TextMeshProUGUI turnText;
        
        [SerializeField] private TextMeshProUGUI nextEventText;
        public void SetCurrentTurn(int turn)
        {
            turnText.text = $"{turn}日目";
        }

        public void SetNextEventInfo(int restTurn , string eventName)
        {
            nextEventText.text = $"次のイベントまで {restTurn+1}ターン";
        }
        
        public void Initialize()
        {
            
        }

        public void Show()
        {
            
        }

        public void Hide()
        {
            
        }
    }
}