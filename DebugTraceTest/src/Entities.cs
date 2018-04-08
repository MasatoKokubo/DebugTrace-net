using System;

namespace DebugTraceTest {
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
			FirstName = firstName; LastName  = lastName ;
		}
	}

	public class Contact : ContactBase {
		public DateTime Birthday;

		public Contact(int id, string firstName, string lastName, DateTime birthday) : base(id, firstName, lastName) {
			Birthday = birthday;
		}
	}
}
