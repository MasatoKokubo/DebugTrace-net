using System;
using static DebugTrace.CSharp; // for Debugging

namespace Readme {
    /// <summary>ReadmeExample</summary>
    public class ReadmeExample {
        public static void _Main(string[] args) {
            Trace.Enter(); // for Debugging

            var contact = new Contact(1, "Akane", "Apple", new DateTime(1991, 2, 3));
            Trace.Print(nameof(contact), contact); // for Debugging

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
