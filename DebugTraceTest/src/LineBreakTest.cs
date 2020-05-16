// LineBreakSpec.cs
// (C) 2018 Masato Kokubo
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp;
using DebugTrace;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace DebugTraceTest {
    [TestClass]
    public class LineBreakTest {
        private static int maximumDataOutputWidth;

        public class Contact {
            public string FirstName {get;}
            public string LastName {get;}
            public DateTime Birthday {get;}
            public string PhoneNumber {get;}

            public Contact(string firstName, string lastName, ValueTuple<int, int, int> birthday, string phoneNumber) {
                FirstName   = firstName;
                LastName    = lastName;
                Birthday    = new DateTime(birthday.Item1, birthday.Item2, birthday.Item3);
                PhoneNumber = phoneNumber;
            } 
        }

        public class Contacts {
            public Contact Contact1 {get;}
            public Contact Contact2 {get;}

            public Contacts(Contact contact1, Contact contact2) {
                Contact1 = contact1;
                Contact2 = contact2;
            }
        }

        [ClassInitialize]
        public static void ClassInit(TestContext context) {
            maximumDataOutputWidth = TraceBase.MaximumDataOutputWidth;
        }

        [ClassCleanup]
        public static void ClassCleanup() {
            TraceBase.MaximumDataOutputWidth = maximumDataOutputWidth;
        }

        [TestMethod]
        public void LineBreakOfArray() {
            // setup
            Trace.Enter();
            TraceBase.MaximumDataOutputWidth = 60;
            var contacts = new Contact[] {
                new Contact("Akane" , "Apple", (2020, 1, 1), "080-1111-1111"),
                new Contact("Yukari", "Apple", (2020, 2, 2), "080-2222-2222")
            };

            // when
            Trace.Print("contacts", contacts);

            // then
            StringAssert.Contains(Trace.LastLog, "FirstName: (Length:");
            StringAssert.Contains(Trace.LastLog, "LastName: (Length:");
            StringAssert.Contains(Trace.LastLog, "Birthday: 2020-");
            StringAssert.Contains(Trace.LastLog, "PhoneNumber: (Length:");
            StringAssert.Contains(Trace.LastLog, "},\n|   DebugTraceTest.Contact {\n");

            // cleanup
            Trace.Leave();
        }

        [TestMethod]
        public void LineBreakOfList() {
            // setup
            Trace.Enter();
            TraceBase.MaximumDataOutputWidth = 60;
            var contacts = new List<Contact> { 
                new Contact("Akane" , "Apple" , (2020, 1, 1), "080-1111-1111"),
                new Contact("Yukari", "Apple" , (2020, 2, 2), "080-2222-2222")
            };

            // when
            Trace.Print("contacts", contacts);

            // then
            StringAssert.Contains(Trace.LastLog, "FirstName: (Length:");
            StringAssert.Contains(Trace.LastLog, "LastName: (Length:");
            StringAssert.Contains(Trace.LastLog, "Birthday: 2020-");
            StringAssert.Contains(Trace.LastLog, "PhoneNumber: (Length:");
            StringAssert.Contains(Trace.LastLog, "},\n|   DebugTraceTest.Contact {\n");

            // cleanup
            Trace.Leave();
        }

        [TestMethod]
        public void LineBreakOfMap() {
            // setup
            Trace.Enter();
            TraceBase.MaximumDataOutputWidth = 60;
            var contacts = new Dictionary<int, Contact> {
                {1, new Contact("Akane" , "Apple" , (2020, 1, 1), "080-1111-1111")},
                {2, new Contact("Yukari", "Apple" , (2020, 2, 2), "080-2222-2222")}
            };

            // when
            Trace.Print("contacts", contacts);

            // then
            StringAssert.Contains(Trace.LastLog, "FirstName: (Length:");
            StringAssert.Contains(Trace.LastLog, "LastName: (Length:");
            StringAssert.Contains(Trace.LastLog, "Birthday: 2020-");
            StringAssert.Contains(Trace.LastLog, "PhoneNumber: (Length:");
            StringAssert.Contains(Trace.LastLog, "},\n|   2: DebugTraceTest.Contact {\n");

            // cleanup
            Trace.Leave();
        }

        [TestMethod]
        public void LineBreakOfReflectionSpec() {
            // setup
            Trace.Enter();
            TraceBase.MaximumDataOutputWidth = 60;
            var contacts = new Contacts(
                new Contact("Akane" , "Apple", (2020, 1, 1), "080-1111-1111"),
                new Contact("Yukari", "Apple", (2020, 2, 2), "080-2222-2222")
            );

            // when
            Trace.Print("contacts", contacts);

            // then
            StringAssert.Contains(Trace.LastLog, "FirstName: (Length:");
            StringAssert.Contains(Trace.LastLog, "LastName: (Length:");
            StringAssert.Contains(Trace.LastLog, "Birthday: 2020-");
            StringAssert.Contains(Trace.LastLog, "PhoneNumber: (Length:");
            StringAssert.Contains(Trace.LastLog, "},\n|   Contact2: DebugTraceTest.Contact {\n");

            // cleanup
            Trace.Leave();
        }
    }
}
