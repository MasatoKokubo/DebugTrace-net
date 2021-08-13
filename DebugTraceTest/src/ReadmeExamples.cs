// ReadmeExample1.cs
// (C) 2018 Masato Kokubo
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp; // ToDo: Remove after debugging

namespace Readme {
    /// <summary>ReadmeExample1</summary>
    [TestClass]
    public class ReadmeExample1 {
        /// <summary>Example1</summary>
        [TestMethod]
        public void Example1() {
            Trace.Enter(); // ToDo: Remove after debugging

            var contacts = new [] {
                new Contact(1, "Akane" , "Apple", new DateTime(1991, 2, 3)),
                new Contact(2, "Yukari", "Apple", new DateTime(1992, 3, 4))
            };
            Trace.Print("contacts", contacts); // ToDo: Remove after debugging

            Trace.Leave(); // ToDo: Remove after debugging
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

    /// <summary>ReadmeExample2</summary>
    [TestClass]
    public class ReadmeExample2 {
        /// <summary>Example2</summary>
        [DataTestMethod]
        [DataRow(1)]
        public void Example2(int value) {
            Trace.Enter(); // ToDo: Remove after debugging

            var task = TaskExample(value);
            task.Wait();
            Trace.Print("task", task); // ToDo: Remove after debugging

            Trace.Leave(); // ToDo: Remove after debugging
        }

        // TaskExample
        private async Task<int> TaskExample(int value) {
            var threasdId = Trace.Enter(); // ToDo: Remove after debugging

            var task = await Task.Run<int>(() => {
                Trace.Enter(); // ToDo: Remove after debugging
                Thread.Sleep(100);
                var result = value * value;
                Trace.Print("result", result); // ToDo: Remove after debugging
                Trace.Leave(); // ToDo: Remove after debugging
                return result;
            });

            Trace.Leave(threasdId); // ToDo: Remove after debugging
            return task;
        }
    }
}
