using System;
using static DebugTrace.CSharp; // for Debugging

namespace Readme {
    /// <summary>ReadmeExample</summary>
    public class ReadmeExample {
        public static void _Main(string[] args) {
            Trace.Enter(); // for Debugging

            var contact = new [] {
                new Contact(1, "Akane" , "Apple", new DateTime(1991, 2, 3)),
                new Contact(2, "Yukari", "Apple", new DateTime(1992, 3, 4))
            };
            Trace.Print("contact", contact); // for Debugging

            Trace.Leave(); // for Debugging
        }
    }

    /// <summary>Entity</summary>
    public class Entity {
        public int ID;

        public Entity(int id) {
            ID = id;
        }
    }

    /// <summary>ContactBase</summary>
    public class ContactBase : Entity {
        public string FirstName;
        public string LastName;

        public ContactBase(int id, string firstName, string lastName) : base(id) {
            FirstName = firstName; LastName  = lastName ;
        }
    }

    /// <summary>Contact</summary>
    public class Contact : ContactBase {
        public DateTime Birthday;

        public Contact(int id, string firstName, string lastName, DateTime birthday) : base(id, firstName, lastName) {
            Birthday  = birthday ;
        }
    }
}
