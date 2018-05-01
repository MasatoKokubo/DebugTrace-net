using System;

namespace DebugTraceTest {
    // DebugTraceTest.PropertyTest+Point struct
    public readonly struct  Point {
        public int X {get;}
        public int Y {get;}
        public Point(int x, int y) {X = x; Y = y;}
    }

    // DebugTraceTest.PropertyTest+Point3 struct
    public readonly struct  Point3 {
        public int X {get;}
        public int Y {get;}
        public int Z {get;}
        public Point3(int x, int y, int z) {X = x; Y = y; Z = z;}
        public override string ToString() {return $"({X}, {Y}, {Z})";}
    }
}
