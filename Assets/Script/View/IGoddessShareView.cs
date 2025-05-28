using System;
using UnityEngine;
namespace MVP
{
    /// <summary>
    /// View（UI側）が実装すべきインターフェース
    /// </summary>
    public interface IGoddessShareView
    {
        /// <summary>
        /// ユーザーが「IDを送信」したときに発火する
        /// </summary>
        IObservable<string> OnSubmitID { get; }
    }
}