using System;
using System.Collections.Generic;

namespace DebugTraceExample {
	using DebugTrace;

	class Program {
		static void Main(string[] args) {
		/**/DebugTrace.Enter();
			Sub1();
		/**/DebugTrace.Leave();
		}

		static void Sub1() {
		/**/DebugTrace.Enter();
			Sub2();
		/**/DebugTrace.Leave();
		}
		
		static void Sub2() {
		/**/DebugTrace.Enter();
			Sub3();
		/**/DebugTrace.Leave();
		}
		
		static void Sub3() {
		/**/DebugTrace.Enter();
			Sub4();
		/**/DebugTrace.Leave();
		}
		
		static void Sub4() {
		/**/DebugTrace.Enter();
			Sub5();
		/**/DebugTrace.Leave();
		}
		
		static void Sub5() {
	/**/DebugTrace.Enter();
		{var value =        true  ; /**/DebugTrace.Print("value", value);}
		{var value = (sbyte)  -1  ; /**/DebugTrace.Print("value", value);}
		{var value = (byte)    1  ; /**/DebugTrace.Print("value", value);}
		{var value = (short)  -1  ; /**/DebugTrace.Print("value", value);}
		{var value = (ushort ) 1  ; /**/DebugTrace.Print("value", value);}
		{var value =          -1  ; /**/DebugTrace.Print("value", value);}
		{var value =           1u ; /**/DebugTrace.Print("value", value);}
		{var value =          -1L ; /**/DebugTrace.Print("value", value);}
		{var value =           1uL; /**/DebugTrace.Print("value", value);}
		{var value =      1.2345f ; /**/DebugTrace.Print("value", value);}
		{var value = 1234.456789d ; /**/DebugTrace.Print("value", value);}
		{var value = 1234.456789m ; /**/DebugTrace.Print("value", value);}
		{var value =         'あ' ; /**/DebugTrace.Print("value", value);}
		{var value =         '\0' ; /**/DebugTrace.Print("value", value);}
		{var value =         '\a' ; /**/DebugTrace.Print("value", value);}
		{var value =         '\b' ; /**/DebugTrace.Print("value", value);}
		{var value =         '\t' ; /**/DebugTrace.Print("value", value);}
		{var value =         '\n' ; /**/DebugTrace.Print("value", value);}
		{var value =         '\v' ; /**/DebugTrace.Print("value", value);}
		{var value =         '\f' ; /**/DebugTrace.Print("value", value);}
		{var value =         '\r' ; /**/DebugTrace.Print("value", value);}
		{var value =         '"'  ; /**/DebugTrace.Print("value", value);}
		{var value =         '\'' ; /**/DebugTrace.Print("value", value);}
		{var value =         '\\' ; /**/DebugTrace.Print("value", value);}
		{var value =     '\u0001' ; /**/DebugTrace.Print("value", value);}
		{var value =     '\u007F' ; /**/DebugTrace.Print("value", value);}
		{var value = "あ\0\a\b\t\n\v\f\r\"'\\\u007F"; /**/DebugTrace.Print("value", value);}

		{var value = new bool   [] {false, true, false      }; /**/DebugTrace.Print("value", value);}
		{var value = new sbyte  [] {-128, -1, 0, 1, 127     }; /**/DebugTrace.Print("value", value);}
		{var value = new byte   [] {0, 1, 2, 3, 255         }; /**/DebugTrace.Print("value", value);}
		{var value = new short  [] {-1, 0, 1                }; /**/DebugTrace.Print("value", value);}
		{var value = new ushort [] { 1, 2, 3                }; /**/DebugTrace.Print("value", value);}
		{var value = new int    [] {-1234567890, 0          }; /**/DebugTrace.Print("value", value);}
		{var value = new uint   [] { 1234567890, 0          }; /**/DebugTrace.Print("value", value);}
		{var value = new long   [] {-1234567890123456789, 0 }; /**/DebugTrace.Print("value", value);}
		{var value = new ulong  [] { 12345678901234567890, 0}; /**/DebugTrace.Print("value", value);}
		{var value = new float  [] { 1.2345f, 0.0f          }; /**/DebugTrace.Print("value", value);}
		{var value = new double [] {-1234.456789d, 0.0d     }; /**/DebugTrace.Print("value", value);}
		{var value = new decimal[] { 1234.456789m, 0.0m     }; /**/DebugTrace.Print("value", value);}
		{var value = new char   [] {'あ','\0','\a','\b','\t','\n','\v','\f','\r','"','\'','\\','\u007F'}; /**/DebugTrace.Print("value", value);}
		{var value = new string [] {"あ\0\a\b\t\n\v\f\r\"'\\\u0001\u007F"}; /**/DebugTrace.Print("value", value);}

		{var value = new bool   [,] {{false, true, false      }, {false, true, false      }}; /**/DebugTrace.Print("value", value);}
		{var value = new sbyte  [,] {{-128, -1, 0, 1, 127     }, {-128, -1, 0, 1, 127     }}; /**/DebugTrace.Print("value", value);}
		{var value = new byte   [,] {{0, 1, 2, 3, 255         }, {0, 1, 2, 3, 255         }}; /**/DebugTrace.Print("value", value);}
		{var value = new short  [,] {{-1, 0, 1                }, {-1, 0, 1                }}; /**/DebugTrace.Print("value", value);}
		{var value = new ushort [,] {{ 1, 2, 3                }, { 1, 2, 3                }}; /**/DebugTrace.Print("value", value);}
		{var value = new int    [,] {{-1234567890, 0          }, {-1234567890, 0          }}; /**/DebugTrace.Print("value", value);}
		{var value = new uint   [,] {{ 1234567890, 0          }, { 1234567890, 0          }}; /**/DebugTrace.Print("value", value);}
		{var value = new long   [,] {{-1234567890123456789, 0 }, {-1234567890123456789, 0 }}; /**/DebugTrace.Print("value", value);}
		{var value = new ulong  [,] {{ 12345678901234567890, 0}, { 12345678901234567890, 0}}; /**/DebugTrace.Print("value", value);}
		{var value = new float  [,] {{ 1.2345f, 0.0f          }, { 1.2345f, 0.0f          }}; /**/DebugTrace.Print("value", value);}
		{var value = new double [,] {{-1234.456789d, 0.0d     }, {-1234.456789d, 0.0d     }}; /**/DebugTrace.Print("value", value);}
		{var value = new decimal[,] {{ 1234.456789m, 0.0m     }, { 1234.456789m, 0.0m     }}; /**/DebugTrace.Print("value", value);}
		{var value = new char   [,] {{'あ','\0','\a','\b','\t','\n','\v','\f','\r','"','\'','\\','\u007F'}}; /**/DebugTrace.Print("value", value);}
		{var value = new string [,] {{"あ\0\a\b\t\n\v\f\r\"'\\\u0001\u007F"}}; /**/DebugTrace.Print("value", value);}

	//	{var value = new bool   [2][]; value[0] = bool   [] = new {false, true, false      }; /**/DebugTrace.Print("value", value);}
	//	{var value = new sbyte  [2][]; value[0] = sbyte  [] = new {-128, -1, 0, 1, 127     }; /**/DebugTrace.Print("value", value);}
	//	{var value = new byte   [2][]; value[0] = byte   [] = new {0, 1, 2, 3, 255         }; /**/DebugTrace.Print("value", value);}
	//	{var value = new short  [2][]; value[0] = short  [] = new {-1, 0, 1                }; /**/DebugTrace.Print("value", value);}
	//	{var value = new ushort [2][]; value[0] = ushort [] = new { 1, 2, 3                }; /**/DebugTrace.Print("value", value);}
	//	{var value = new int    [2][]; value[0] = int    [] = new {-1234567890, 0          }; /**/DebugTrace.Print("value", value);}
	//	{var value = new uint   [2][]; value[0] = uint   [] = new { 1234567890, 0          }; /**/DebugTrace.Print("value", value);}
	//	{var value = new long   [2][]; value[0] = long   [] = new {-1234567890123456789, 0 }; /**/DebugTrace.Print("value", value);}
	//	{var value = new ulong  [2][]; value[0] = ulong  [] = new { 12345678901234567890, 0}; /**/DebugTrace.Print("value", value);}
	//	{var value = new float  [2][]; value[0] = float  [] = new { 1.2345f, 0.0f          }; /**/DebugTrace.Print("value", value);}
	//	{var value = new double [2][]; value[0] = double [] = new {-1234.456789d, 0.0d     }; /**/DebugTrace.Print("value", value);}
	//	{var value = new decimal[2][]; value[0] = decimal[] = new { 1234.456789m, 0.0m     }; /**/DebugTrace.Print("value", value);}
	//	{var value = new char   [2][]; value[0] = char   [] = new {'あ','\0','\a','\b','\t','\n','\v','\f','\r','"','\'','\\','\u007F'}; /**/DebugTrace.Print("value", value);}
	//	{var value = new string [2][]; value[0] = string [] = new {"あ\0\a\b\t\n\v\f\r\"'\\\u0001\u007F"}; /**/DebugTrace.Print("value", value);}

		var point = new Point(1, 2);
	/**/DebugTrace.Print("point", point);

		var point3 = new Point3(1, 2, -2);
	/**/DebugTrace.Print("point3", point3);

		var contact = new Contact(1, "Akane", "Apple", new DateTime(1990, 1, 2));
	/**/DebugTrace.Print("contact", contact);

		{
			var points = new Point[] {point, new Point(3, 4), new Point(5, 6)};
		/**/DebugTrace.Print("points", points);

			var point3s = new Point3[] {point3, new Point3(10, 20, -20), new Point3(100, 200, -200)};
		/**/DebugTrace.Print("point3s", point3s);

			var contacts = new Contact[] {
				contact,
				new Contact(2, "Yukari", "Apple", new DateTime(1991, 2, 3)),
				new Contact(3, "Haruka", "Apple", new DateTime(1992, 3, 4)),
				new Contact(4, "Sasha" , "Apple", new DateTime(1993, 4, 5)),
			};
		/**/DebugTrace.Print("contacts", contacts);
		}

		{
			var points = new List<Point>() {point, new Point(3, 4), new Point(5, 6)};
		/**/DebugTrace.Print("points", points);

			var point3s = new List<Point3>() {point3, new Point3(10, 20, -20), new Point3(100, 200, -200)};
		/**/DebugTrace.Print("point3s", point3s);

			var contacts = new List<Contact> {
				contact,
				new Contact(2, "Yukari", "Apple", new DateTime(1991, 2, 3)),
				new Contact(3, "Haruka", "Apple", new DateTime(1992, 3, 4)),
				new Contact(4, "Sasha" , "Apple", new DateTime(1993, 4, 5)),
			};
		/**/DebugTrace.Print("contacts", contacts);
		}

		{
			var points = new Dictionary<int, Point>() {{1, point}, {2, new Point(3, 4)}, {3, new Point(5, 6)}};
		/**/DebugTrace.Print("points", points);

			var point3s = new Dictionary<string, Point3>() {{"A", point3}, {"B", new Point3(10, 20, -20)}, {"C", new Point3(100, 200, -200)}};
		/**/DebugTrace.Print("point3s", point3s);

			var contacts = new Dictionary<int, Contact> {
				{1, contact},
				{2, new Contact(2, "Yukari", "Apple", new DateTime(1991, 2, 3))},
				{3, new Contact(3, "Haruka", "Apple", new DateTime(1992, 3, 4))},
				{4, new Contact(4, "Sasha" , "Apple", new DateTime(1993, 4, 5))},
			};
		/**/DebugTrace.Print("contacts", contacts);
		}

	/**/DebugTrace.Leave();
		}

		public readonly struct  Point3 {
			public int X {get;}
			public int Y {get;}
			public int Z {get;}

			public Point3(int x, int y, int z) {
				X = x;
				Y = y;
				Z = z;
			}

			public override string ToString() {
				return "(X: " + X + ", Y: " + Y + ", Z: " + Z + ")";
			}
		}
	}

	public readonly struct  Point {
		public int X {get;}
		public int Y {get;}

		public Point(int x, int y) {
			X = x;
			Y = y;
		}
	}

	public class Entity {
		public int ID {get; set;}

		public Entity(int id) {
			ID = id;
		}
	}

	public class Contact : Entity {
		public string FirstName  {get; set;}
		public string LastName   {get; set;}
		public DateTime Birthday {get; set;}

		public Contact(int id, string firstName, string lastName, DateTime birthday) : base(id) {
			FirstName = firstName;
			LastName  = lastName ;
			Birthday  = birthday ;
		}
	}
}
