using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Advent_of_Code_2022.Commons;

[TestClass]
public class TestCommon
{
    [TestMethod]
    public void TestPos()
    {
        Assert.AreEqual(new Pos<int>(1,1), new Pos<int>(1,1));
    }

    [TestMethod]
    public void TestBox()
    {
        // Test Constructor
        //Assert.ThrowsException<ArgumentException>(() => new Box<int>(new Pos<int>(3, 3), new Pos<int>(0, 0)));
        //Assert.ThrowsException<ArgumentException>(() => new Box<int>(new Pos<int>(3, 0), new Pos<int>(2, 3)));
        //Assert.ThrowsException<ArgumentException>(() => new Box<int>(new Pos<int>(0, 3), new Pos<int>(3, 2)));

        var window = new Box<int>(new Pos<int>(0, 0), new Pos<int>(3, 3));

        // Test IsInside
        Assert.IsTrue(window.Contains(new Pos<int>(1, 1)));

        for (int x = window.Min.x; x <= window.Max.x; x++)
        {
            for (int y = window.Min.y; y <= window.Max.y; y++)
            {
                Assert.IsTrue(window.Contains(new Pos<int>(x, y)));
            }
        }

        Assert.IsFalse(window.Contains(new Pos<int>(-1, 1)));
        Assert.IsFalse(window.Contains(new Pos<int>(4, 1)));
        Assert.IsFalse(window.Contains(new Pos<int>(1, -1)));
        Assert.IsFalse(window.Contains(new Pos<int>(1, 4)));

        // Test Width
        Assert.AreEqual(4, new Box<int>(new Pos<int>(-1, 0), new Pos<int>(2, 0)).Width);
        Assert.AreEqual(3, new Box<int>(new Pos<int>(1, 0), new Pos<int>(3, 0)).Width);
        Assert.AreEqual(7, new Box<int>(7, 0).Width);
        Assert.AreEqual(8, new Box<int>(8, 7).Width);

        // Test Height
        Assert.AreEqual(4, new Box<int>(new Pos<int>(0, -1), new Pos<int>(0, 2)).Height);
        Assert.AreEqual(3, new Box<int>(new Pos<int>(0, 1), new Pos<int>(0, 3)).Height);

        Box<int> B() => new Box<int>(5, 5);
        Assert.IsTrue(B().Contains(new Box<int>(5, 5)));
        Assert.IsFalse(B().Contains(new Box<int>(6, 5)));
        Assert.IsFalse(B().Contains(new Box<int>(5, 6)));
        {
            var c = B();
            c.IncreaseToPoint(new Pos<int>(0, -1));
            Assert.IsFalse(B().Contains(c));
        }
        {
            var c = B();
            c.IncreaseToPoint(new Pos<int>(-1, 0));
            Assert.IsFalse(B().Contains(c));
        }

        {
            var a = B().Translate(new Pos<int>(-1, -1));
            var b = B().Translate(new Pos<int>(2, 2));
            Assert.AreEqual(new Box<int>(new Pos<int>(2, 2), new Pos<int>(3, 3)), a.Intersection(b));

            Assert.AreEqual(new Box<int>(5, 2), B().Intersection(B().Translate(new Pos<int>(0, -3))));
            Assert.AreEqual(new Box<int>(2, 5), B().Intersection(B().Translate(new Pos<int>(-3, 0))));
        }
    }

    [TestMethod]
    public void TestPos3()
    {
        var p = new Pos3<int>(0, 0, 0);
        var actual = p.Dist<double>(new Pos3<int>(2, 3, 6));
        Assert.AreEqual(7.0, actual, double.Epsilon);
    }

    [TestMethod]
    public void TestPosN()
    {
        var p1 = new PosN<long>(1, 2, 3, 4);
        var p2 = new PosN<long>(4, 5, 6, 7);
        var actual = p1.Dist<double>(p2);
        Assert.AreEqual(6.0, actual, double.Epsilon);

        Assert.AreEqual(new PosN<long>(5, 7, 9, 11), p1 + p2);
        Assert.AreEqual(new PosN<long>(-3, -3, -3, -3), p1 - p2);
    }

    [TestMethod]
    public void TestAllCombos()
    {
        var bag = new List<List<int>>();
        bag.Add(new List<int> { 1 });
        bag.Add(new List<int> { 2 });
        bag.Add(new List<int> { 3 });
        bag.Add(new List<int> { 1, 2 });
        bag.Add(new List<int> { 1, 3 });
        bag.Add(new List<int> { 2, 3 });
        bag.Add(new List<int> { 1, 2, 3 });
        var c = 0;
        bool Exist(List<int> x)
        {
            foreach (var list in bag)
            {
                var success = true;
                if (x.Count != list.Count) continue;
                for (int i = 0; i < x.Count; i++)
                {
                    if (x[i] != list[i])
                    {
                        success = false;
                        break;
                    }
                }
                if (success) return true;
            }
            return false;
        }
        foreach (var list in Common.GetAllCombos(new List<int> { 1, 2, 3 }))
        {
            Assert.IsTrue(Exist(list), string.Join(", ", list));
            c++;
        }
        Assert.AreEqual(bag.Count, c);
    }
}
