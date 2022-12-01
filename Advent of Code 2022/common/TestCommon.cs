namespace Advent_of_Code_2022.common;

[TestClass]
public class TestCommon
{
    [TestMethod]
    public void TestBox()
    {
        // Test Constructor
        Assert.ThrowsException<ArgumentException>(() => new Box<int>(new Pos<int>(3, 3), new Pos<int>(0, 0)));
        Assert.ThrowsException<ArgumentException>(() => new Box<int>(new Pos<int>(3, 0), new Pos<int>(2, 3)));
        Assert.ThrowsException<ArgumentException>(() => new Box<int>(new Pos<int>(0, 3), new Pos<int>(3, 2)));

        var window = new Box<int>(new Pos<int>(0, 0), new Pos<int>(3, 3));
        
        // Test IsInside
        Assert.IsTrue(window.IsInside(new Pos<int>(1, 1)));

        for (int x = window.UpperLeft.x; x <= window.LowerRight.x; x++)
        {
            for (int y = window.UpperLeft.y; y <= window.LowerRight.y; y++)
            {
                Assert.IsTrue(window.IsInside(new Pos<int>(x, y)));
            }
        }

        Assert.IsFalse(window.IsInside(new Pos<int>(-1, 1)));
        Assert.IsFalse(window.IsInside(new Pos<int>(4, 1)));
        Assert.IsFalse(window.IsInside(new Pos<int>(1, -1)));
        Assert.IsFalse(window.IsInside(new Pos<int>(1, 4)));

        // Test Width
        Assert.AreEqual(3, new Box<int>(new Pos<int>(-1, 0), new Pos<int>(2, 0)).Width);
        Assert.AreEqual(2, new Box<int>(new Pos<int>(1, 0), new Pos<int>(3, 0)).Width);

        // Test Height
        Assert.AreEqual(3, new Box<int>(new Pos<int>(0, -1), new Pos<int>(0, 2)).Height);
        Assert.AreEqual(2, new Box<int>(new Pos<int>(0, 1), new Pos<int>(0, 3)).Height);
    }
    
}
