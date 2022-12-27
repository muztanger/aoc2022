using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2022.Commons;

public class Box3<T> where T : INumber<T>
{
    public Box<T> XYPlane { get; set; }
    public Box<T> YZPlane { get; set; }

    public Box3(params Pos3<T>[] positions)
    {
        Assert.IsTrue(positions.Length > 0);
        XYPlane = new Box<T>(positions[0].XY);
        YZPlane = new Box<T>(positions[0].YZ);
        foreach (var p in positions)
        {
            IncreaseToPoint(p);
        }
    }

    public void IncreaseToPoint(Pos3<T> p)
    {
        XYPlane.IncreaseToPoint(p.XY);
        YZPlane.IncreaseToPoint(p.YZ);
    }

    public override string ToString()
    {
        return $"[{XYPlane}, {YZPlane}]";
    }

    public bool IsInside(Pos3<T> pos)
    {
        return XYPlane.Contains(pos.XY) && YZPlane.Contains(pos.YZ);
    }
}
