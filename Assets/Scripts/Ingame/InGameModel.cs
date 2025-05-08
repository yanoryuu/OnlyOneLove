using R3;
using UnityEngine;

namespace Ingame
{
    public class InGameModel 
    {
        
        private ReactiveProperty<InGameEnum.GameState> currentIngameState = new ReactiveProperty<InGameEnum.GameState>();
        public ReactiveProperty<InGameEnum.GameState>  CurrentIngameState => currentIngameState;
        
        public InGameModel()
        {
            currentIngameState.Value = InGameEnum.GameState.Default;
        }

        public void ChangeState(InGameEnum.GameState state)
        {
            currentIngameState.Value = state;
        }
    }

}