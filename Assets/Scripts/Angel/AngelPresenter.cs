using UnityEngine;
using R3;

public class AngelPresenter : MonoBehaviour
{
   private AngelModel angelModel;
   
   [SerializeField]
   private AngelView angelView;

   private void Start()
   {
      angelModel = new AngelModel();
   }

   private void Bind()
   {
      
   }

   //次のParameterをここに入力
   public void UpdateAngel(Parameters parameters)
   {
      angelModel.UpdateParameter(parameters);
   }
}