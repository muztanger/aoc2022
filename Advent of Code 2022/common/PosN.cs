namespace Advent_of_Code_2022
{
    public class PosN : IEquatable<PosN>
    {
        public List<int> values;
        private readonly int N;
        public PosN(int[] w)
        {
            values = w.ToList();
            N = values.Count;
        }

        public PosN(PosN other)
        {
            values = new List<int>(other.values);
            N = values.Count;
        }

        public PosN(IEnumerable<int> v)
        {
            this.values = v.ToList();
            N = values.Count;
        }

        public static PosN operator *(PosN p1, int n)
        {
            return new PosN(p1.values.Select(z => n * z));
        }

        public static PosN operator +(PosN p1, PosN p2)
        {
            return new PosN(p1.values.Zip(p2.values, (x,y) => x + y).ToList());
        }
        public static PosN operator -(PosN p) => new PosN(p.values.Select(x => -x));

        public static PosN operator -(PosN p1, PosN p2) => p1 + (-p2);

        public override string ToString()
        {
            return $"({string.Join(",", values)})";
        }

        internal int Manhattan(PosN inter)
        {
            return values.Zip(inter.values, (x, y) => Math.Abs(x - y)).Sum(); ;
        }

        bool IEquatable<PosN>.Equals(PosN other)
        {
            return this.values.Zip(other.values, (x, y) => x == y).Aggregate((result, z) => result &= z);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PosN);
        }

        public bool Equals([AllowNull] PosN other)
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

        internal double Dist(PosN p1)
        {
            var delta = p1 - this;
            return Math.Sqrt(delta.values.Select(x => x * x).Sum());
        }
    }
}
