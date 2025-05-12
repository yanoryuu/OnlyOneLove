[System.Serializable]
public class AngelParameter
{
    public float affection;
    public float trust;
    public float jealousy;
    public float closeness;

    public AngelParameter Multiply(float aff, float trust, float jeal, float close)
    {
        return new AngelParameter
        {
            affection = this.affection * aff,
            trust     = this.trust     * trust,
            jealousy  = this.jealousy  * jeal,
            closeness = this.closeness * close
        };
    }

    public AngelParameter Add(AngelParameter other)
    {
        return new AngelParameter
        {
            affection = this.affection + other.affection,
            trust     = this.trust     + other.trust,
            jealousy  = this.jealousy  + other.jealousy,
            closeness = this.closeness + other.closeness
        };
    }
}