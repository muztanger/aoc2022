namespace Advent_of_Code_2022.Commons;

public class PosN<T>
    where T : INumber<T>, IEquatable<T>
{
    public List<T> values;
    public int Count => values.Count;

    public PosN(params T[] w)
    {
        values = w.ToList<T>();
    }

    public PosN(PosN<T> other)
    {
        values = new List<T>(other.values);
    }

    public PosN(IEnumerable<T> v)
    {
        this.values = v.ToList();
    }

    public static PosN<T> operator *(PosN<T> p1, T n)
    {
        return new PosN<T>(p1.values.Select(z => n * z));
    }

    public static PosN<T> operator +(PosN<T> p1, PosN<T> p2)
    {
        return new PosN<T>(p1.values.Zip(p2.values, (x,y) => x + y).ToList());
    }
    public static PosN<T> operator -(PosN<T> p) => new PosN<T>(p.values.Select(x => -x));

    public static PosN<T> operator -(PosN<T> p1, PosN<T> p2) => p1 + (-p2);

    public override string ToString()
    {
        return $"({string.Join(",", values)})";
    }

    internal T Manhattan(PosN<T> inter)
    {
        T sum = T.Zero;
        foreach (var x in values.Zip(inter.values, (x, y) => T.Abs(x - y)))
        {
            sum += x;
        }
        return sum;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as PosN<T>);
    }

    public bool Equals([AllowNull] PosN<T> other)
    {
        return other != null && this.values.Zip(other.values, (x, y) => x == y).Aggregate((result, z) => result &= z);
    }

    public override int GetHashCode()
    {
        int hash = 43;
        foreach (var v in values)
        {
            hash = hash * 2621 + v.GetHashCode();
        }
        return hash;
    }

    internal TResult Dist<TResult>(PosN<T> p1)
        where TResult : IFloatingPoint<TResult>, IRootFunctions<TResult>
    {
        var delta = p1 - this;
        TResult squareSum = TResult.Zero;
        foreach (var x2 in delta.values.Select(x => TResult.CreateChecked(x * x)))
        {
            squareSum += x2;
        }
        return TResult.Sqrt(squareSum);
    }
}
