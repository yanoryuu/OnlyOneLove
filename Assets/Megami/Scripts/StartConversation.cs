using UnityEngine;

public class StartConversation : MonoBehaviour
{
    public SampleAdvEngineController advController;
    public string startLabel = "Start"; // �ŏ��ɍĐ��������V�i���I���x�����iUtage��*.adv�V�i���I�̃��x���j
    void Start()
    {
        //string label = !string.IsNullOrEmpty(SceneTransition.NextScenarioLabel) ? SceneTransition.NextScenarioLabel : startLabel;

        if (advController != null && !string.IsNullOrEmpty(startLabel))
        {
            advController.JumpScenario(startLabel);
            //SceneTransition.NextScenarioLabel = null; // �g���I������烊�Z�b�g
        }
        else
        {
            Debug.LogError("advController ���ݒ肳��Ă��Ȃ����A���x������ł�");
        }
    }

    /*public SampleAdvEngineController advController;
    public string startLabel = "Start"; // �ŏ��ɍĐ��������V�i���I���x�����iUtage��*.adv�V�i���I�̃��x���j

    void Start()
    {
        if (advController != null && !string.IsNullOrEmpty(startLabel))
        {
            advController.JumpScenario(startLabel);
        }
        else
        {
            Debug.LogError("advController ���ݒ肳��Ă��Ȃ����AstartLabel ����ł�");
        }
    }*/

}
