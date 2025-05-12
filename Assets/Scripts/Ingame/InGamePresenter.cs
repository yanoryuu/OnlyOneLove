using System.Collections.Generic;
using R3;
using UnityEngine;

namespace Ingame
{
    public class InGamePresenter : MonoBehaviour
    {
        private InGameModel model;
        
        //インゲームのView
        [SerializeField]
        private InGameView inGameView;
        
        //話題選択のビュー
        [SerializeField] private ChooseTopicView chooseTopicView;
        
        //カードをプレイするビュー
        [SerializeField] private CardPlayView cardPlayView;
        
        [SerializeField]
        private CardPlayPresenter cardPlayPresenter;
        
        [SerializeField]
        private AngelPresenter angelPresenter;
        
        //女の子の返答画面
        [SerializeField]
        private AngelTalkView angelTalkView;

        private void Start()
        {
            model = new InGameModel();
            
            Bind();
            
            ChangeState(InGameEnum.GameState.ChooseTopic);
        }

        private void Bind()
        {
            model.CurrentIngameState.Subscribe(x => InGameManager.Instance.ChangeState(x))
                .AddTo(this);
            
            /*inGameView.TurnEndButton.OnClickAsObservable()
                .Where(_=>InGameManager.Instance.CurrentState.Value == InGameEnum.GameState.PlayerTurn)
                .Subscribe(_ =>
                {
                    //会話終了
                })
                .AddTo(this);*/
            
            inGameView.TalkButton.OnClickAsObservable()
                .Where(_=>InGameManager.Instance.CurrentState.Value == InGameEnum.GameState.PlayerTurn)
                .Subscribe(_=>ChangeState(InGameEnum.GameState.CardEffect))
                .AddTo(this);
            
            InGameManager.Instance.CurrentTurn.Subscribe(x=>inGameView.SetCurrentTurn(x))
                .AddTo(this);
            
            //話題選択ボタン
            chooseTopicView.SetTopicButton.OnClickAsObservable()
                .Where(x=> cardPlayPresenter.Model.TalkTopic.Value!=null)
                .Subscribe(_ =>
                {
                    ChangeState(InGameEnum.GameState.PlayerTurn);
                })
                .AddTo(this);
        }

        private void ChangeState(InGameEnum.GameState state)
        {
            model.ChangeState(state);
            switch (state)
            {
                case InGameEnum.GameState.ChooseTopic:
                    Debug.Log("State: ChooseTopic");
                    chooseTopicView.Show();
                    angelTalkView.Hide();
                    cardPlayView.Hide();
                    inGameView.Hide();
                    break;
                
                case InGameEnum.GameState.PlayerTurn:
                    chooseTopicView.Hide();
                    angelTalkView.Hide();
                    inGameView.Hide();
                    cardPlayView.Show();
                    Debug.Log("State: PlayerTurn");
                    // プレイヤーがカードを選ぶフェーズ
                    break;
                
                case InGameEnum.GameState.CardEffect:
                    Debug.Log("State: CardEffect");
                    chooseTopicView.Hide();
                    angelTalkView.Hide();
                    inGameView.Hide();
                    cardPlayView.Hide();
                    // カード効果の実行処理
                    ChangeState(InGameEnum.GameState.Talk);
                    break;
                
                case InGameEnum.GameState.Talk:
                    Debug.Log("State: Talk");
                    chooseTopicView.Hide();
                    angelTalkView.Show();
                    inGameView.Hide();
                    cardPlayView.Hide();
                    ChangeState(InGameEnum.GameState.CheckStatus);
                    // 会話演出や分岐表示
                    break;
                case InGameEnum.GameState.CheckStatus:
                    Debug.Log("State: CheckStatus");
                    chooseTopicView.Hide();
                    angelTalkView.Hide();
                    inGameView.Hide();
                    cardPlayView.Hide();
                    // 好感度、SPなどの状態更新
                    ChangeState(InGameEnum.GameState.FinishTurn);
                    break;
                case InGameEnum.GameState.FinishTurn:
                    Debug.Log("State: FinishTurn");
                    chooseTopicView.Hide();
                    angelTalkView.Hide();
                    inGameView.Hide();
                    cardPlayView.Hide();
                    //ターンを進める
                    InGameManager.Instance.NextTurn();
                    
                    ChangeState(InGameEnum.GameState.PlayerTurn);
                    break;
                
                case InGameEnum.GameState.Confession:
                    Debug.Log("State: Confession");
                    chooseTopicView.Hide();
                    angelTalkView.Hide();
                    inGameView.Hide();
                    cardPlayView.Hide();
                    // 告白フェーズ（選択肢や演出）UniTaskで告白フェーズが終われば
                    break;
                
                case InGameEnum.GameState.FinishTalk:
                    Debug.Log("State: FinishTalk");
                    //カードのモデルを初期化
                    cardPlayPresenter.Model.Initialize();
                    break;
            }
        }
    } 
}