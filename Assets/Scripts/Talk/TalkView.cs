using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalkView : MonoBehaviour
{
   [SerializeField] private GameObject talkPanel;
   public GameObject TalkPanel => talkPanel;
   
   [SerializeField] private TextMeshProUGUI talkText;
   public TextMeshProUGUI TalkText => talkText;
   
   [SerializeField] private Button nextDialogueButton;
   public Button NextDialogueButton => nextDialogueButton;
   
   public void Show()
   {
      talkPanel.SetActive(true);
   }
   
   public void Hide()
   {
      talkPanel.SetActive(false);
   }

   public void SetText(string text)
   {
      talkText.text = text;
   }

   public void Initialize()
   {
      talkText.text = "";
   }
}
