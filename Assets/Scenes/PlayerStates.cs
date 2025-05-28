using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPLayerStates", menuName = "Game/PlayerData", order = 2)]
public class PlayerStates : ScriptableObject
{
    // 基本情報
    public float Health;
    public float MaxHealth;
    public float walkSpeed;
    public float runSpeed;

    // ステータス
    public float damage; // ダメージ量
    public float attackSpeed; // 攻撃速度 (発動間隔)
    public int projectileCount; // 発射数
    public float range; // 射程距離
    public float cooldown; // クールダウン
    public float areaMultiplier; // 範囲倍率
    public float knockbackPower; // ノックバック力
    public float speed;
    public float interactionRange;
    public float duration;
    public float AutoRecoverHPPercent;
    
    //レベルアップ
    public float levelUpExpMultiplier;
    public int startLevel;
    public float startExp;
    public float startLevelUpExp;
    
    // 特殊効果
    [Range(0, 100)] public float criticalChance; // クリティカル発生率 (%)
    public float criticalMultiplier; // クリティカル倍率
    // デバッグ用
    [HideInInspector] public int id; // ユニークID (自動設定)
}