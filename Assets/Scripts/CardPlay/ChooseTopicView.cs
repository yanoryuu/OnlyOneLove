using UnityEngine;
using UnityEngine.UI;

public class ChooseTopicView : MonoBehaviour
{
    [SerializeField] private Button setTopicButton;
    
    public Button SetTopicButton => setTopicButton;

    public void Show()
    {
        setTopicButton.gameObject.SetActive(true);
    }

    public void Hide()
    {
        setTopicButton.gameObject.SetActive(false);
    }
}
