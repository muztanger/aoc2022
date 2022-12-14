using System;

namespace Advent_of_Code_2022.Commons;

public class Pos<T> : IEquatable<Pos<T>>
    where T : INumber<T>
{
    public T x;
    public T y;
    public Pos(T x, T y)
    {
        this.x = x;
        this.y = y;
    }

    public Pos((T, T) z)
    {
        x = z.Item1;
        y = z.Item2;
    }

    public Pos(Pos<T> other)
    {
        this.x = other.x;
        this.y = other.y;
    }

    public T Dist()
    {
        return T.Abs(y - x);
    }

    public static Pos<T> operator *(Pos<T> p1, T n)
    {
        return new Pos<T>(p1.x * n, p1.y * n);
    }

    public static Pos<T> operator *(T n, Pos<T> p1)
    {
        return p1 * n;
    }

    public static Pos<T> operator +(Pos<T> p1, Pos<T> p2)
    {
        return new Pos<T>(p1.x + p2.x, p1.y + p2.y);
    }
    public static Pos<T> operator -(Pos<T> p) => new Pos<T>(-p.x, -p.y);
    public static Pos<T> operator -(Pos<T> p1, Pos<T> p2) => p1 + (-p2);

    public override string ToString()
    {
        return $"({x}, {y})";
    }

    internal T Manhattan(Pos<T> inter)
    {
        return T.Abs(x - inter.x) + T.Abs(y - inter.y);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        Pos<T>? posObj = obj as Pos<T>;
        if (posObj == null)
            return false;
        else
            return Equals(posObj);
    }

    public bool Equals(Pos<T>? other)
    {
        return other != null &&
               x == other.x &&
               y == other.y;
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() * 7919 + y.GetHashCode();
    }

    public static bool operator ==(Pos<T> pos1, Pos<T> pos2)
    {
        if (((object)pos1) == null || ((object)pos2) == null)
            return Object.Equals(pos1, pos2);

        return pos1.Equals(pos2);
    }

    public static bool operator !=(Pos<T> pos1, Pos<T> pos2)
    {
        if (((object)pos1) == null || ((object)pos2) == null)
            return !Object.Equals(pos1, pos2);

        return !(pos1.Equals(pos2));
    }

    public bool BetweenXY(T z)
    {
        return z >= x && z <= y;
    }

    internal bool Between(Pos<T> p1, Pos<T> p2)
    {
        if (p1.x == p2.x && p2.x == this.x)
        {
            return (p1.y < y && y < p2.y) || (p2.y < y && y < p1.y);
        }
        if (!(new Line<T>(this, p1).OnLine(p2))) return false;

        if (p1.x < this.x) return p2.x > this.x;
        if (p1.x > this.x) return p2.x < this.x;

        return false;
    }

    internal T Dist2(Pos<T> p1)
    {
        var delta = p1 - this;
        return delta.x * delta.x + delta.y * delta.y;
    }

    internal bool Adjacent(Pos<T> other)
    {
        var dp = this - other;
        var x = T.Abs(dp.x);
        var y = T.Abs(dp.y);
        return y <= T.One && x <= T.One;
    }

    internal Pos<T> Sign(Pos<T> other)
    {
        var dp = other - this;
        dp.x = T.Sign(dp.x) switch { -1 => -T.One, 1 => T.One, _ => T.Zero };
        dp.y = T.Sign(dp.y) switch { -1 => -T.One, 1 => T.One, _ => T.Zero };
        return new Pos<T>(dp);
    }
}
