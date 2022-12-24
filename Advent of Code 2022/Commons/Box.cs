namespace Advent_of_Code_2022.Commons;

public class Box<T>: IEquatable<Box<T>>
    where T : INumber<T>
{
    public Pos<T> Min { get; set; }
    public Pos<T> Max { get; set; }

    public T Width => T.Abs(Max.x - Min.x) + T.One;
    public T Height => T.Abs(Max.y - Min.y) + T.One;
    public T Area => Width * Height;

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

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        Box<T>? posObj = obj as Box<T>;
        if (posObj == null)
            return false;
        else
            return Equals(posObj);
    }

    public bool Equals(Box<T>? other)
    {
        return other != null &&
               Max == other.Max &&
               Min == other.Min;
    }

    public override int GetHashCode()
    {
        return Min.GetHashCode() * 3779 + Max.GetHashCode();
    }

}
