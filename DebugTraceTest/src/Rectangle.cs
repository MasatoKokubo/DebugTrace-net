// Rectangle.cs
// (C) 2018 Masato Kokubo

namespace DebugTraceTest {
    // DebugTraceTest.PropertyTest+Point4 struct
    public readonly struct  Rectangle {
        public int X1 {get;}
        public int Y1 {get;}
        public int X2 {get;}
        public int Y2 {get;}
        public Rectangle(int x1, int y1, int x2, int y2) {X1 = x1; Y1 = y1; X2 = x2; Y2 = y2;}
        public override string ToString() {return $"({X1}, {Y1}, {X2}, {Y2})";}
    }
}
