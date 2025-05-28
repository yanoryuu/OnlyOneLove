using System.Threading.Tasks;
using UnityEngine;

namespace MVP
{
    /// <summary>
    /// サーバー通信モック実装
    /// </summary>
    public class MockGoddessService : IGoddessService
    {
        public Task<GoddessModel> GetGoddessDataAsync(string id)
        {
            // ダミーでランダムなパラメーターを返す
            var model = new GoddessModel
            {
                GoddessID = id,
                Power     = Random.Range(1f, 10f),
                Grace     = Random.Range(1f, 10f),
            };
            return Task.FromResult(model);
        }
    }
}