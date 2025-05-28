using UnityEngine;
using System.Threading.Tasks;

namespace MVP
{
    /// <summary>
    /// サーバー通信部分の抽象インターフェース
    /// </summary>
    public interface IGoddessService
    {
        /// <summary>
        /// 女神IDを渡してデータを取得（モックではランダム値を返す）
        /// </summary>
        Task<GoddessModel> GetGoddessDataAsync(string id);
    }
}
