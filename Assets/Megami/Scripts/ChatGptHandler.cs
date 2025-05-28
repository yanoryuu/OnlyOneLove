using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using Utage;

namespace Utage
{
    public class ChatGptHandler : MonoBehaviour
    {
        public AdvEngine engine;

        public void SendInputToGptWithCallback(string input, Action callback)
        {
            SendInputToGptAndResumeWithCallback(input, callback).Forget();
        }

        private async UniTaskVoid SendInputToGptAndResumeWithCallback(string input, Action callback)
        {
            var gptManager = FindObjectOfType<ChatGptNetworkManager>();
            if (gptManager == null)
            {
                Debug.LogError("ChatGptNetworkManager��������܂���");
                engine.Param.SetParameterString("gptReply", "�G���[�FGPT�}�l�[�W����������܂���");
                //engine.UiManager.IsInputTrigCustom = true;
                callback?.Invoke();
                return;
            }

            try
            {
                var response = await gptManager.SendChatRequestAsync(input);
                Debug.Log("GPT�̕ԓ�: " + response.reply);
                engine.Param.SetParameterString("gptReply", response.reply);
                Debug.Log("gptReply�ɐݒ肳�ꂽ�l: " + engine.Param.GetParameterString("gptReply"));
            }
            catch (Exception e)
            {
                Debug.LogError("GPT�G���[: " + e.Message);
                engine.Param.SetParameterString("gptReply", "�G���[���������܂���: " + e.Message);
            }
            finally
            {
            
            
                callback?.Invoke();
            }
        }

    }
}

