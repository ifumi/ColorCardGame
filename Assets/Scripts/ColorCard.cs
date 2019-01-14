public struct ColorCard
{
    public enum Color
    {
        RED, GREEN, BLUE, YELLOW, NONE
    }

    public enum Type
    {
        STANDARD, DRAW2, SKIP, REVERSE, WILD, WILD4, WILDRANDOM
    }

    public Color color;
    public Type type;
    public int value; // 0-9

    public ColorCard(Type _t, Color _c, int _val)
    {
        color = _c;
        value = _val;
        type = _t;
    }


}

