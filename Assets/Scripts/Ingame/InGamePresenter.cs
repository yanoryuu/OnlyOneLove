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
        
        [SerializeField] private ChooseTopicView chooseTopicView;
        
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
            InGameManager.Instance.CurrentState.Subscribe(x => InGameManager.Instance.ChangeState(x))
                .AddTo(this);
            
            view.TurnEndButton.OnClickAsObservable()
                .Where(_=>InGameManager.Instance.CurrentState.Value == InGameEnum.GameState.PlayerTurn)
                .Subscribe(_ =>
                {
                    //会話終了
                })
                .AddTo(this);
            
            view.TalkButton.OnClickAsObservable()
                .Where(_=>InGameManager.Instance.CurrentState.Value == InGameEnum.GameState.PlayerTurn)
                .Subscribe(_=>ChangeState(InGameEnum.GameState.CardEffect))
                .AddTo(this);
            
            InGameManager.Instance.CurrentTurn.Subscribe(x=>view.SetCurrentTurn(x))
                .AddTo(this);
            
            //話題選択画面表示
            InGameManager.Instance.CurrentState.Where(x => x == InGameEnum.GameState.ChooseTopic)
                .Subscribe(_ =>
                {
                    chooseTopicView.Show();
                })
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
                
                case InGameEnum.GameState.ChooseTopic:
                    Debug.Log("State: ChooseTopic");
                    cardPlayPresenter.Model.TalkTopic.Take(1)
                        .Subscribe(x =>
                        {
                            chooseTopicView.Hide();
                            ChangeState(InGameEnum.GameState.PlayerTurn);
                        })
                        .AddTo(this);
                    break;
                
                case InGameEnum.GameState.CardEffect:
                    Debug.Log("State: CardEffect");
                    // カード効果の実行処理
                    ChangeState(InGameEnum.GameState.Talk);
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