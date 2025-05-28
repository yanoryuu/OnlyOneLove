using UniRx;
using UnityEngine;
using System.ComponentModel;
using Component = UnityEngine.Component;

namespace MVP
{
    /// <summary>
    /// MVP の Presenter 層
    /// </summary>
    public class GoddessSharePresenter
    {
        readonly IGoddessShareView _view;
        readonly IGoddessService   _service;
        readonly GoddessModel      _model = new GoddessModel();

        public GoddessSharePresenter(IGoddessShareView view, IGoddessService service)
        {
            _view    = view;
            _service = service;

            // 「送信」イベントを購読し、View (Component) のライフサイクルに紐づける
            _view.OnSubmitID
                .Subscribe(id => FetchData(id))
                .AddTo((Component)view);
        }

        async void FetchData(string id)
        {
            _model.GoddessID = id;
            var data = await _service.GetGoddessDataAsync(id);
            _model.Power  = data.Power;
            _model.Grace  = data.Grace;
            Debug.Log($"[Goddess] ID={_model.GoddessID}, Power={_model.Power:F1}, Grace={_model.Grace:F1}");
        }
    }
}