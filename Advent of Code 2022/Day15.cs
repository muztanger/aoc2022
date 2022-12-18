using System.Collections.Generic;
using System.Drawing;

namespace Advent_of_Code_2022;

[TestClass]
public class Day15
{
    private static string Part1(IEnumerable<string> input, long checkY, bool isExample)
    {
        
        var result = new StringBuilder();
        var re = new Regex(@"=([-\d]+).*=([-\d]+).*=([-\d]+).*=([-\d]+)");
        var sensors = new List<Pos<long>>();
        var beacons = new List<Pos<long>>();
        foreach (var line in input)
        {
            var match = re.Match(line);
            Assert.IsTrue(match.Success);
            var groups = match.Groups;
            Assert.IsTrue(groups.Count == 5);
            var sensor = new Pos<long>(long.Parse(groups[1].Value), long.Parse(groups[2].Value));
            var beacon = new Pos<long>(long.Parse(groups[3].Value), long.Parse(groups[4].Value));
            sensors.Add(sensor);
            beacons.Add(beacon);
            Console.WriteLine($"sensor={sensor} beacon={beacon}");
        }

        var area = new Box<long>(sensors.First());
        sensors.ForEach(s => area.IncreaseToPoint(s));
        beacons.ForEach(b => area.IncreaseToPoint(b));
        
        var paint = new HashSet<Pos<long>>();
        Assert.AreEqual(sensors.Count, beacons.Count);
        for (int i = 0; i < sensors.Count; i++)//.Where(p => p == new Pos<long>(8, 7))) //TODO
        {
            var sensor = sensors[i];
            var closestBeacon = beacons[i];

            // paint area around beacon and signal
            var dist = sensor.Manhattan(closestBeacon);
            var paintArea = new Box<long>(sensor, sensor - new Pos<long>(dist, dist), sensor + new Pos<long>(dist, dist));
            //for (long y = paintArea.XYPlane.y; y <= paintArea.YZPlane.y; y++)
            {
                long y = checkY;
                for (long x = paintArea.Min.x; x <= paintArea.Max.x; x++)
                {
                    var p = new Pos<long>(x, y);
                    if (dist >= p.Manhattan(sensor))
                    {
                        paint.Add(p);
                        area.IncreaseToPoint(p);
                    }
                }
            }
        }

        var toString = new StringBuilder();
        var count = 0;
        //for (long y = area.XYPlane.y; y <= area.YZPlane.y; y++)
        {
            long y = checkY;
            if (y != area.Min.y)
            {
                if (isExample) toString.AppendLine();
            }
            for (long x = area.Min.x; x <= area.Max.x; x++)
            {
                var p = new Pos<long>(x , y);
                if (sensors.Contains(p))
                {
                    if (y == checkY)
                    {
                        count++;
                    }
                    if (isExample) toString.Append('S');
                }
                else if (beacons.Contains(p))
                {
                    if (isExample) toString.Append('B');
                }
                else if (paint.Contains(p))
                {
                    if (y == checkY)
                    {
                        count++;
                    }
                    if (isExample) toString.Append('#');
                }
                else
                {
                    if (isExample) toString.Append('.');
                }
            }
        }
        if (isExample) Console.WriteLine(toString);

        return count.ToString();
    }
    
    private static string Part2(IEnumerable<string> input, bool isExample, long N)
    {

        var result = new StringBuilder();
        var re = new Regex(@"=([-\d]+).*=([-\d]+).*=([-\d]+).*=([-\d]+)");
        var sensors = new List<Pos<long>>();
        var beacons = new List<Pos<long>>();
        foreach (var line in input)
        {
            var match = re.Match(line);
            Assert.IsTrue(match.Success);
            var groups = match.Groups;
            Assert.IsTrue(groups.Count == 5);
            var sensor = new Pos<long>(long.Parse(groups[1].Value), long.Parse(groups[2].Value));
            var beacon = new Pos<long>(long.Parse(groups[3].Value), long.Parse(groups[4].Value));
            sensors.Add(sensor);
            beacons.Add(beacon);
            Console.WriteLine($"sensor={sensor} beacon={beacon}");
        }

        var area = new Box<long>(sensors.First());
        sensors.ForEach(s => area.IncreaseToPoint(s));
        beacons.ForEach(b => area.IncreaseToPoint(b));

        //var paint = new HashSet<Pos<long>>();
        //Assert.AreEqual(sensors.Count, beacons.Count);

        var distances = new List<long>();
        for (int i = 0; i < sensors.Count; i++)
        {
            distances.Add(sensors[i].Manhattan(beacons[i]));
        }

        var ticks = 0;
        var candidates = new List<Pos<long>>();
        for (long y = 0; y <= N && candidates.Count == 0; y++)
        {
            for (long x = 0; x <= N && candidates.Count == 0; x++)
            {
                ticks++;
                var p = new Pos<long>(x, y);
                var isFound = false;
                for (int i = 0; i < sensors.Count; i++)
                {
                    if (p.Manhattan(sensors[i]) <= distances[i])
                    {
                        //   0123456
                        // 0    #
                        // 1   ##b
                        // 2  #####
                        // 3 ###S###
                        // 4  #####
                        // 5   ###
                        // 6    #

                        // find right border of signal
                        var dp = (p - sensors[i]).Abs();
                        x = sensors[i].x + distances[i] - dp.y;
                        isFound = true;
                        break;
                    }    
                }
                if (!isFound)
                {
                    candidates.Add(p);
                }
            }
        }
        Console.WriteLine($"ticks={ticks}");
        Assert.IsTrue(candidates.Count > 0);
        var distressBeacon = candidates[0];
        return (distressBeacon.x * 4000000 + distressBeacon.y).ToString();
    }


    [TestMethod]
    public void Day15_Part1_Example01()
    {
        var input = """
            Sensor at x=2, y=18: closest beacon is at x=-2, y=15
            Sensor at x=9, y=16: closest beacon is at x=10, y=16
            Sensor at x=13, y=2: closest beacon is at x=15, y=3
            Sensor at x=12, y=14: closest beacon is at x=10, y=16
            Sensor at x=10, y=20: closest beacon is at x=10, y=16
            Sensor at x=14, y=17: closest beacon is at x=10, y=16
            Sensor at x=8, y=7: closest beacon is at x=2, y=10
            Sensor at x=2, y=0: closest beacon is at x=2, y=10
            Sensor at x=0, y=11: closest beacon is at x=2, y=10
            Sensor at x=20, y=14: closest beacon is at x=25, y=17
            Sensor at x=17, y=20: closest beacon is at x=21, y=22
            Sensor at x=16, y=7: closest beacon is at x=15, y=3
            Sensor at x=14, y=3: closest beacon is at x=15, y=3
            Sensor at x=20, y=1: closest beacon is at x=15, y=3
            """;
        var result = Part1(Common.GetLines(input), 10, isExample: true);
        Assert.AreEqual("26", result);
    }
    
    [TestMethod]
    public void Day15_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day15)), 2000000, isExample: false);
        Assert.AreNotEqual("5286569", result);
        Assert.AreEqual("4951427", result);
    }
    
    [TestMethod]
    public void Day15_Part2_Example01()
    {
        var input = """
            Sensor at x=2, y=18: closest beacon is at x=-2, y=15
            Sensor at x=9, y=16: closest beacon is at x=10, y=16
            Sensor at x=13, y=2: closest beacon is at x=15, y=3
            Sensor at x=12, y=14: closest beacon is at x=10, y=16
            Sensor at x=10, y=20: closest beacon is at x=10, y=16
            Sensor at x=14, y=17: closest beacon is at x=10, y=16
            Sensor at x=8, y=7: closest beacon is at x=2, y=10
            Sensor at x=2, y=0: closest beacon is at x=2, y=10
            Sensor at x=0, y=11: closest beacon is at x=2, y=10
            Sensor at x=20, y=14: closest beacon is at x=25, y=17
            Sensor at x=17, y=20: closest beacon is at x=21, y=22
            Sensor at x=16, y=7: closest beacon is at x=15, y=3
            Sensor at x=14, y=3: closest beacon is at x=15, y=3
            Sensor at x=20, y=1: closest beacon is at x=15, y=3
            """;
        var result = Part2(Common.GetLines(input), isExample: true, 20L);
        Assert.AreEqual("56000011", result);
    }
    
    [TestMethod]
    public void Day15_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day15)), isExample: false, 4000000);
        Assert.AreEqual("13029714573243", result);
    }
    
}
