namespace Advent_of_Code_2022.Commons;

public class Box<T> where T : INumber<T>
{
    public Pos<T> UpperLeft { get; set; }
    public Pos<T> LowerRight { get; set; }

    public T Width => T.Abs(LowerRight.x - UpperLeft.x);
    public T Height => T.Abs(LowerRight.y - UpperLeft.y);

    public Box(Pos<T> p1, Pos<T> p2) 
    {
        UpperLeft = new Pos<T>(T.Min(p1.x, p2.x), T.Min(p1.y, p2.y));
        LowerRight = new Pos<T>(T.Max(p1.x, p2.x), T.Max(p1.y, p2.y));
    }

    public void IncreaseToPoint(Pos<T> p)
    {
        UpperLeft.x = T.Min(UpperLeft.x, p.x);
        UpperLeft.y = T.Min(UpperLeft.y, p.y);
        LowerRight.x = T.Max(LowerRight.x, p.x);
        LowerRight.y = T.Max(LowerRight.y, p.y);
    }

    public override string ToString()
    {
        return $"[{UpperLeft}, {LowerRight}]";
    }

    public bool IsInside(Pos<T> pos)
    {
        if (pos.x < UpperLeft.x || pos.x > LowerRight.x)
        {
            return false;
        }
        if (pos.y < UpperLeft.y || pos.y > LowerRight.y)
        {
            return false;
        }
        return true;
    }
}
