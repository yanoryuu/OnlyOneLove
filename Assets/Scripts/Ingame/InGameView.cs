using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

namespace Ingame
{
    public class InGameView : MonoBehaviour
    {
        [SerializeField] private Button turnEndButton;
        public Button TurnEndButton => turnEndButton;

        [SerializeField] private Button talkButton;
        public Button TalkButton => talkButton;
        
        [SerializeField] private TextMeshProUGUI turnText;
        
        [SerializeField] private TextMeshProUGUI nextEventText;
        public void SetCurrentTurn(int turn)
        {
            turnText.text = $"Turn: {turn}";
        }

        public void SetNextEventInfo(int restTurn , string eventName)
        {
            nextEventText.text = $"Next Event: {eventName}, Rest: {restTurn}";
        }
        
        public void Initialize()
        {
            
        }
    }
}