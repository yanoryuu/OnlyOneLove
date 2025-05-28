using UnityEngine;

namespace MVP
{
    /// <summary>
    /// サーバーから取得した女神データを保持する Model
    /// </summary>
    public class GoddessModel
    {
        /// <summary>入力された女神ID</summary>
        public string GoddessID { get; set; }

        /// <summary>能力値：Power</summary>
        public float Power { get; set; }

        /// <summary>能力値：Grace</summary>
        public float Grace { get; set; }
    }
}
