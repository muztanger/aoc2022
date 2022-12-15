namespace Advent_of_Code_2022.Commons;

public class Box<T> where T : INumber<T>
{
    public Pos<T> Min { get; set; }
    public Pos<T> Max { get; set; }

    public T Width => T.Abs(Max.x - Min.x);
    public T Height => T.Abs(Max.y - Min.y);

    public Box(params Pos<T>[] positions) 
    {
        Assert.IsTrue(positions.Length > 0);
        Min = new Pos<T>(positions[0]);
        Max = new Pos<T>(positions[0]);
        foreach (var p in positions)
        {
            IncreaseToPoint(p);
        }
    }

    public void IncreaseToPoint(Pos<T> p)
    {
        Min.x = T.Min(Min.x, p.x);
        Min.y = T.Min(Min.y, p.y);
        Max.x = T.Max(Max.x, p.x);
        Max.y = T.Max(Max.y, p.y);
    }

    public override string ToString()
    {
        return $"[{Min}, {Max}]";
    }

    public bool IsInside(Pos<T> pos)
    {
        if (pos.x < Min.x || pos.x > Max.x)
        {
            return false;
        }
        if (pos.y < Min.y || pos.y > Max.y)
        {
            return false;
        }
        return true;
    }
}
