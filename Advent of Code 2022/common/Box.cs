namespace Advent_of_Code_2022.common;

public class Box<T> where T : INumber<T>
{
    public Pos<T> UpperLeft { get; set; }
    public Pos<T> LowerRight { get; set; }

    public T Width => T.Abs(LowerRight.x - UpperLeft.x);
    public T Height => T.Abs(LowerRight.y - UpperLeft.y);

    public Box(Pos<T> upperLeft, Pos<T> lowerRight) 
    {
        if (upperLeft.x > lowerRight.x) throw new ArgumentException("upperLeft is right of lowerRight");
        if (upperLeft.y > lowerRight.y) throw new ArgumentException("upperLeft is below lowerRight");
        UpperLeft = upperLeft;
        LowerRight = lowerRight;
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
