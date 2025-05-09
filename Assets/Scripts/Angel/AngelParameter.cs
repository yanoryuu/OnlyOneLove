// --- AngelParameter.cs ---
using UnityEngine;

[System.Serializable]
public class AngelParameter
{
    /// <summary>
    /// 愛情（Affection）
    /// </summary>
    public float affection;

    /// <summary>
    /// 信頼度（Trust）
    /// </summary>
    public float trust;

    /// <summary>
    /// 嫉妬度（Jealousy）
    /// </summary>
    public float jealousy;

    /// <summary>
    /// 親密度（Closeness）
    /// </summary>
    public float closeness;

    /// <summary>
    /// 各パラメーターを指定したスカラーで乗算し、
    /// 浮動小数点として新しいインスタンスを返します。
    /// </summary>
    /// <param name="affectionFactor">愛情への乗算係数</param>
    /// <param name="trustFactor">信頼度への乗算係数</param>
    /// <param name="jealousyFactor">嫉妬度への乗算係数</param>
    /// <param name="closenessFactor">親密度への乗算係数</param>
    /// <returns>乗算後の新しい AngelParameter</returns>
    public AngelParameter Multiply(
        float affectionFactor,
        float trustFactor,
        float jealousyFactor,
        float closenessFactor)
    {
        return new AngelParameter
        {
            affection = affection * affectionFactor,
            trust     = trust     * trustFactor,
            jealousy  = jealousy  * jealousyFactor,
            closeness = closeness * closenessFactor
        };
    }

    public AngelParameter Add(AngelParameter angelParameter)
    {
        return new AngelParameter()
        {
            affection =affection + angelParameter.affection,
            trust = trust + angelParameter.trust,
            jealousy = jealousy + angelParameter.jealousy,
            closeness = closeness + angelParameter.closeness
        };
    }
    
}