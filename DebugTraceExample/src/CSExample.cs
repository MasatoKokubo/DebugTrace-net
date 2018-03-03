using System;
using System.Collections.Generic;
using static DebugTrace.CSharp;

namespace DebugTraceExample {
	class CSExample {
		static void Main(string[] args) {
		/**/Trace.Enter();
			try {
				Sub1();
			}
			catch (Exception e) {
			/**/Trace.Print("e.ToString()", e.ToString());
			/**/Trace.ResetNest();
			}
		/**/Trace.Leave();
		}

		static void Sub1() {
		/**/Trace.Enter();
			Sub2();
		/**/Trace.Leave();
		}

		static void Sub2() {
		/**/Trace.Enter();
			Sub3();
			throw new Exception("Not Error");
		/**/Trace.Leave();
		}

		static void Sub3() {
	/**/Trace.Enter();
		{var value =        true  ; /**/Trace.Print("value", value);}
		{var value = (sbyte)  -1  ; /**/Trace.Print("value", value);}
		{var value = (byte)    1  ; /**/Trace.Print("value", value);}
		{var value = (short)  -1  ; /**/Trace.Print("value", value);}
		{var value = (ushort ) 1  ; /**/Trace.Print("value", value);}
		{var value =          -1  ; /**/Trace.Print("value", value);}
		{var value =           1u ; /**/Trace.Print("value", value);}
		{var value =          -1L ; /**/Trace.Print("value", value);}
		{var value =           1uL; /**/Trace.Print("value", value);}
		{var value =      1.2345f ; /**/Trace.Print("value", value);}
		{var value = 1234.456789d ; /**/Trace.Print("value", value);}
		{var value = 1234.456789m ; /**/Trace.Print("value", value);}
		{var value =         'あ' ; /**/Trace.Print("value", value);}
		{var value =         '\0' ; /**/Trace.Print("value", value);}
		{var value =         '\a' ; /**/Trace.Print("value", value);}
		{var value =         '\b' ; /**/Trace.Print("value", value);}
		{var value =         '\t' ; /**/Trace.Print("value", value);}
		{var value =         '\n' ; /**/Trace.Print("value", value);}
		{var value =         '\v' ; /**/Trace.Print("value", value);}
		{var value =         '\f' ; /**/Trace.Print("value", value);}
		{var value =         '\r' ; /**/Trace.Print("value", value);}
		{var value =         '"'  ; /**/Trace.Print("value", value);}
		{var value =         '\'' ; /**/Trace.Print("value", value);}
		{var value =         '\\' ; /**/Trace.Print("value", value);}
		{var value =     '\u0001' ; /**/Trace.Print("value", value);}
		{var value =     '\u007F' ; /**/Trace.Print("value", value);}
		{var value = "あ\0\a\b\t\n\v\f\r\"'\\\u007F"; /**/Trace.Print("value", value);}
		{var value = (1, 2)       ; /**/Trace.Print("value", value);}
		{var value = (10, "abc", 1.1F); /**/Trace.Print("value", value);}
		{var value = ((10, 20), "abc", 1.1F); /**/Trace.Print("value", value);}
		{var value = new Tuple<int, int>(1, 2); /**/Trace.Print("value", value);}
		{var value = new Tuple<Tuple<int, int>, string, float>(new Tuple<int, int>(10, 20), "abc", 1.1F); /**/Trace.Print("value", value);}

		{var value = new bool   [] {false, true, false      }; /**/Trace.Print("value", value);}
		{var value = new sbyte  [] {-128, -1, 0, 1, 127     }; /**/Trace.Print("value", value);}
		{var value = new byte   [] {0, 1, 2, 3, 255         }; /**/Trace.Print("value", value);}
		{var value = new short  [] {-1, 0, 1                }; /**/Trace.Print("value", value);}
		{var value = new ushort [] { 1, 2, 3                }; /**/Trace.Print("value", value);}
		{var value = new int    [] {-1234567890, 0          }; /**/Trace.Print("value", value);}
		{var value = new uint   [] { 1234567890, 0          }; /**/Trace.Print("value", value);}
		{var value = new long   [] {-1234567890123456789, 0 }; /**/Trace.Print("value", value);}
		{var value = new ulong  [] { 12345678901234567890, 0}; /**/Trace.Print("value", value);}
		{var value = new float  [] { 1.2345f, 0.0f          }; /**/Trace.Print("value", value);}
		{var value = new double [] {-1234.456789d, 0.0d     }; /**/Trace.Print("value", value);}
		{var value = new decimal[] { 1234.456789m, 0.0m     }; /**/Trace.Print("value", value);}
		{var value = new char   [] {'あ','\0','\a','\b','\t','\n','\v','\f','\r','"','\'','\\','\u007F'}; /**/Trace.Print("value", value);}
		{var value = new string [] {"あ\0\a\b\t\n\v\f\r\"'\\\u0001\u007F"}; /**/Trace.Print("value", value);}
		{var value = new (int, int, int)[] {(-3, -2, -1), (0, 1, 2) }; /**/Trace.Print("value", value);}
		{var value = (new int[] {-3, -2, -1}, new int[] {0, 1, 2}); /**/Trace.Print("value", value);}

		{var value = new bool   [,] {{false, true, false      }, {false, true, false      }}; /**/Trace.Print("value", value);}
		{var value = new sbyte  [,] {{-128, -1, 0, 1, 127     }, {-128, -1, 0, 1, 127     }}; /**/Trace.Print("value", value);}
		{var value = new byte   [,] {{0, 1, 2, 3, 255         }, {0, 1, 2, 3, 255         }}; /**/Trace.Print("value", value);}
		{var value = new short  [,] {{-1, 0, 1                }, {-1, 0, 1                }}; /**/Trace.Print("value", value);}
		{var value = new ushort [,] {{ 1, 2, 3                }, { 1, 2, 3                }}; /**/Trace.Print("value", value);}
		{var value = new int    [,] {{-1234567890, 0          }, {-1234567890, 0          }}; /**/Trace.Print("value", value);}
		{var value = new uint   [,] {{ 1234567890, 0          }, { 1234567890, 0          }}; /**/Trace.Print("value", value);}
		{var value = new long   [,] {{-1234567890123456789, 0 }, {-1234567890123456789, 0 }}; /**/Trace.Print("value", value);}
		{var value = new ulong  [,] {{ 12345678901234567890, 0}, { 12345678901234567890, 0}}; /**/Trace.Print("value", value);}
		{var value = new float  [,] {{ 1.2345f, 0.0f          }, { 1.2345f, 0.0f          }}; /**/Trace.Print("value", value);}
		{var value = new double [,] {{-1234.456789d, 0.0d     }, {-1234.456789d, 0.0d     }}; /**/Trace.Print("value", value);}
		{var value = new decimal[,] {{ 1234.456789m, 0.0m     }, { 1234.456789m, 0.0m     }}; /**/Trace.Print("value", value);}
		{var value = new char   [,] {{'あ','\0','\a','\b','\t','\n','\v','\f','\r','"','\'','\\','\u007F'}}; /**/Trace.Print("value", value);}
		{var value = new string [,] {{"あ\0\a\b\t\n\v\f\r\"'\\\u0001\u007F"}}; /**/Trace.Print("value", value);}

	//	{var value = new bool   [2][]; value[0] = bool   [] = new {false, true, false      }; /**/Trace.Print("value", value);}
	//	{var value = new sbyte  [2][]; value[0] = sbyte  [] = new {-128, -1, 0, 1, 127     }; /**/Trace.Print("value", value);}
	//	{var value = new byte   [2][]; value[0] = byte   [] = new {0, 1, 2, 3, 255         }; /**/Trace.Print("value", value);}
	//	{var value = new short  [2][]; value[0] = short  [] = new {-1, 0, 1                }; /**/Trace.Print("value", value);}
	//	{var value = new ushort [2][]; value[0] = ushort [] = new { 1, 2, 3                }; /**/Trace.Print("value", value);}
	//	{var value = new int    [2][]; value[0] = int    [] = new {-1234567890, 0          }; /**/Trace.Print("value", value);}
	//	{var value = new uint   [2][]; value[0] = uint   [] = new { 1234567890, 0          }; /**/Trace.Print("value", value);}
	//	{var value = new long   [2][]; value[0] = long   [] = new {-1234567890123456789, 0 }; /**/Trace.Print("value", value);}
	//	{var value = new ulong  [2][]; value[0] = ulong  [] = new { 12345678901234567890, 0}; /**/Trace.Print("value", value);}
	//	{var value = new float  [2][]; value[0] = float  [] = new { 1.2345f, 0.0f          }; /**/Trace.Print("value", value);}
	//	{var value = new double [2][]; value[0] = double [] = new {-1234.456789d, 0.0d     }; /**/Trace.Print("value", value);}
	//	{var value = new decimal[2][]; value[0] = decimal[] = new { 1234.456789m, 0.0m     }; /**/Trace.Print("value", value);}
	//	{var value = new char   [2][]; value[0] = char   [] = new {'あ','\0','\a','\b','\t','\n','\v','\f','\r','"','\'','\\','\u007F'}; /**/Trace.Print("value", value);}
	//	{var value = new string [2][]; value[0] = string [] = new {"あ\0\a\b\t\n\v\f\r\"'\\\u0001\u007F"}; /**/Trace.Print("value", value);}

		var point = new Point(1, 2);
	/**/Trace.Print("point", point);

		var point3 = new Point3(1, 2, -2);
	/**/Trace.Print("point3", point3);

		var contact = new Contact(1, "Akane", "Apple", new DateTime(1990, 1, 2));
	/**/Trace.Print("contact", contact);

		{
			var points = new Point[] {point, new Point(3, 4), new Point(5, 6)};
		/**/Trace.Print("points", points);

			var point3s = new Point3[] {point3, new Point3(10, 20, -20), new Point3(100, 200, -200)};
		/**/Trace.Print("point3s", point3s);

			var contacts = new Contact[] {
				contact,
				new Contact(2, "Yukari", "Apple", new DateTime(1991, 2, 3)),
				new Contact(3, "Haruka", "Apple", new DateTime(1992, 3, 4)),
				new Contact(4, "Sasha" , "Apple", new DateTime(1993, 4, 5)),
			};
		/**/Trace.Print("contacts", contacts);
		/**/Trace.Print("(points, point3s, contacts)", (points, point3s, contacts));
		}

		{
			var points = new List<Point>() {point, new Point(3, 4), new Point(5, 6)};
		/**/Trace.Print("points", points);

			var point3s = new List<Point3>() {point3, new Point3(10, 20, -20), new Point3(100, 200, -200)};
		/**/Trace.Print("point3s", point3s);

			var contacts = new List<Contact> {
				contact,
				new Contact(2, "Yukari", "Apple", new DateTime(1991, 2, 3)),
				new Contact(3, "Haruka", "Apple", new DateTime(1992, 3, 4)),
				new Contact(4, "Sasha" , "Apple", new DateTime(1993, 4, 5)),
			};
		/**/Trace.Print("contacts", contacts);
		/**/Trace.Print("(points, point3s, contacts)", (points, point3s, contacts));
		}

		{
			var points = new Dictionary<int, Point>() {{1, point}, {2, new Point(3, 4)}, {3, new Point(5, 6)}};
		/**/Trace.Print("points", points);

			var point3s = new Dictionary<string, Point3>() {{"A", point3}, {"B", new Point3(10, 20, -20)}, {"C", new Point3(100, 200, -200)}};
		/**/Trace.Print("point3s", point3s);

			var contacts = new Dictionary<int, Contact> {
				{1, contact},
				{2, new Contact(2, "Yukari", "Apple", new DateTime(1991, 2, 3))},
				{3, new Contact(3, "Haruka", "Apple", new DateTime(1992, 3, 4))},
				{4, new Contact(4, "Sasha" , "Apple", new DateTime(1993, 4, 5))},
			};
		/**/Trace.Print("contacts", contacts);
		/**/Trace.Print("(points, point3s, contacts)", () => (points, point3s, contacts));
		}

	/**/Trace.Print(() => "aaa" + "bbb");

	/**/Trace.Leave();
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
		public int ID;

		public Entity(int id) {
			ID = id;
		}
	}

	public class ContactBase : Entity {
		public string FirstName;
		public string LastName;

		public ContactBase(int id, string firstName, string lastName) : base(id) {
			FirstName = firstName;
			LastName  = lastName ;
		}
	}

	public class Contact : ContactBase {
		public DateTime Birthday;

		public Contact(int id, string firstName, string lastName, DateTime birthday) : base(id, firstName, lastName) {
			Birthday  = birthday ;
		}
	}
}
