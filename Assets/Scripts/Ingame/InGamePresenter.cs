using System.Collections.Generic;
using R3;
using UnityEngine;

namespace Ingame
{
    public class InGamePresenter : MonoBehaviour
    {
        private InGameModel model;
        
        [SerializeField]
        private InGameView view;
        
        [SerializeField]
        private CardPlayPresenter cardPlayPresenter;
        
        [SerializeField]
        private AngelPresenter angelPresenter;

        private void Start()
        {
            model = new InGameModel();
            
            Bind();
            
            ChangeState(InGameEnum.GameState.PlayerTurn);
        }

        private void Bind()
        {
            model.CurrentIngameState.Subscribe(x => InGameManager.Instance.ChangeState(x))
                .AddTo(this);
            
            view.TurnEndButton.OnClickAsObservable()
                .Where(_=>model.CurrentIngameState.Value == InGameEnum.GameState.PlayerTurn)
                .Subscribe(_ =>
                {
                    //会話終了
                })
                .AddTo(this);
            
            view.TalkButton.OnClickAsObservable()
                .Where(_=>model.CurrentIngameState.Value == InGameEnum.GameState.PlayerTurn)
                .Subscribe(_=>ChangeState(InGameEnum.GameState.Talk))
                .AddTo(this);
            
            InGameManager.Instance.CurrentTurn.Subscribe(x=>view.SetCurrentTurn(x))
                .AddTo(this);
        }

        private void ChangeState(InGameEnum.GameState state)
        {
            model.ChangeState(state);
            switch (state)
            {
                case InGameEnum.GameState.PlayerTurn:
                    Debug.Log("State: PlayerTurn");
                    // プレイヤーがカードを選ぶフェーズ
                    break;
                case InGameEnum.GameState.CardEffect:
                    Debug.Log("State: CardEffect");
                    // カード効果の実行処理
                    ChangeState(InGameEnum.GameState.PlayerTurn);
                    break;
                case InGameEnum.GameState.Talk:
                    Debug.Log("State: Dialogue");
                    ChangeState(InGameEnum.GameState.CheckStatus);
                    // 会話演出や分岐表示
                    break;
                case InGameEnum.GameState.CheckStatus:
                    Debug.Log("State: CheckStatus");
                    // 好感度、SPなどの状態更新
                    ChangeState(InGameEnum.GameState.FinishTurn);
                    break;
                case InGameEnum.GameState.FinishTurn:
                    Debug.Log("State: FinishTurn");
                    //ターンを進める
                    InGameManager.Instance.NextTurn();
                    
                    ChangeState(InGameEnum.GameState.PlayerTurn);
                    break;
                
                case InGameEnum.GameState.Confession:
                    Debug.Log("State: Confession");
                    // 告白フェーズ（選択肢や演出）UniTaskで告白フェーズが終われば
                    
                    break;
            }
        }
    } 
}