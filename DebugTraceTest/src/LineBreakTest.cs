// LineBreakSpec.cs
// (C) 2018 Masato Kokubo
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp;
using DebugTrace;
using System;
using System.Collections.Generic;

namespace DebugTraceTest;

[TestClass]
public class LineBreakTest {
    private static int maximumDataOutputWidth;

    public class Contact {
        public string FirstName {get;}
        public string LastName {get;}
        public DateTime Birthday {get;}
        public string PhoneNumber {get;}

        public Contact(string firstName, string lastName, (int, int, int) birthday, string phoneNumber) {
            FirstName   = firstName;
            LastName    = lastName;
            Birthday    = new DateTime(birthday.Item1, birthday.Item2, birthday.Item3);
            PhoneNumber = phoneNumber;
        } 
    }

    public class Contacts {
        public Contact? Contact1 {get;}
        public Contact? Contact2 {get;}
        public Contact? Contact3 {get;}
        public Contact? Contact4 {get;}

        public Contacts(Contact? contact1, Contact? contact2, Contact? contact3, Contact? contact4) {
            Contact1 = contact1;
            Contact2 = contact2;
            Contact3 = contact3;
            Contact4 = contact4;
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
        TraceBase.MaximumDataOutputWidth = 80;
        var contacts = new Contact?[] {
            new Contact("Akane" , "Apple", (2020, 1, 1), "080-1111-1111"),
            new Contact("Yukari", "Apple", (2020, 2, 2), "080-2222-2222"),
            null,
            null
        };

        // when
        Trace.Print("contacts", contacts);

        // then
        StringAssert.Contains(Trace.LastLog, "{\n|   DebugTraceTest.Contact {");
        StringAssert.Contains(Trace.LastLog, "  FirstName:");
        StringAssert.Contains(Trace.LastLog, ", LastName:");
        StringAssert.Contains(Trace.LastLog, "  Birthday:");
        StringAssert.Contains(Trace.LastLog, ", PhoneNumber:");
        StringAssert.Contains(Trace.LastLog, "},\n|   DebugTraceTest.Contact {\n");
        StringAssert.Contains(Trace.LastLog, "},\n|   null, null");

        // cleanup
        Trace.Leave();
    }

    [TestMethod]
    public void LineBreakOfList() {
        // setup
        Trace.Enter();
        TraceBase.MaximumDataOutputWidth = 80;
        var contacts = new List<Contact?> { 
            new Contact("Akane" , "Apple" , (2020, 1, 1), "080-1111-1111"),
            new Contact("Yukari", "Apple" , (2020, 2, 2), "080-2222-2222"),
            null,
            null
        };

        // when
        Trace.Print("contacts", contacts);

        // then
        StringAssert.Contains(Trace.LastLog, "{\n|   DebugTraceTest.Contact {");
        StringAssert.Contains(Trace.LastLog, "  FirstName:");
        StringAssert.Contains(Trace.LastLog, ", LastName:");
        StringAssert.Contains(Trace.LastLog, "  Birthday:");
        StringAssert.Contains(Trace.LastLog, ", PhoneNumber:");
        StringAssert.Contains(Trace.LastLog, "},\n|   DebugTraceTest.Contact {\n");
        StringAssert.Contains(Trace.LastLog, "},\n|   null, null");

        // cleanup
        Trace.Leave();
    }

    [TestMethod]
    public void LineBreakOfMap() {
        // setup
        Trace.Enter();
        TraceBase.MaximumDataOutputWidth = 80;
        var contacts = new Dictionary<int, Contact?> {
            {1, new Contact("Akane" , "Apple" , (2020, 1, 1), "080-1111-1111")},
            {2, new Contact("Yukari", "Apple" , (2020, 2, 2), "080-2222-2222")},
            {3, null},
            {4, null}
        };

        // when
        Trace.Print("contacts", contacts);

        // then
        StringAssert.Contains(Trace.LastLog, "{\n|   1: DebugTraceTest.Contact {");
        StringAssert.Contains(Trace.LastLog, "  FirstName:");
        StringAssert.Contains(Trace.LastLog, ", LastName:");
        StringAssert.Contains(Trace.LastLog, "  Birthday:");
        StringAssert.Contains(Trace.LastLog, ", PhoneNumber:");
        StringAssert.Contains(Trace.LastLog, "},\n|   2: DebugTraceTest.Contact {\n");
        StringAssert.Contains(Trace.LastLog, "},\n|   3: null, 4: null");

        // cleanup
        Trace.Leave();
    }

    [TestMethod]
    public void LineBreakOfReflectionSpec() {
        // setup
        Trace.Enter();
        TraceBase.MaximumDataOutputWidth = 80;
        var contacts = new Contacts(
            new Contact("Akane" , "Apple", (2020, 1, 1), "080-1111-1111"),
            new Contact("Yukari", "Apple", (2020, 2, 2), "080-2222-2222"),
            null,
            null
        );

        // when
        Trace.Print("contacts", contacts);

        // then
        StringAssert.Contains(Trace.LastLog, "{\n|   Contact1: DebugTraceTest.Contact {");
        StringAssert.Contains(Trace.LastLog, "  FirstName:");
        StringAssert.Contains(Trace.LastLog, ", LastName:");
        StringAssert.Contains(Trace.LastLog, "  Birthday:");
        StringAssert.Contains(Trace.LastLog, ", PhoneNumber:");
        StringAssert.Contains(Trace.LastLog, "},\n|   Contact2: DebugTraceTest.Contact {\n");
        StringAssert.Contains(Trace.LastLog, "},\n|   DebugTraceTest.Contact Contact3: null, DebugTraceTest.Contact Contact4: null");

        // cleanup
        Trace.Leave();
    }


    /** @since 3.1.1 */
    [TestMethod]
    public void NoLineBreak_NameValue() {
        // setup
        Trace.Enter();
        TraceBase.MaximumDataOutputWidth = 60;
        var foo = "000000000011111111112222222222333333333344444444445555555555";

        // when
        Trace.Print("foo", foo);

        // then
        StringAssert.Contains(Trace.LastLog, "foo = (Length:60)\"0000000000");

        // cleanup
        Trace.Leave();
    }

    /** @since 3.1.1 */
    [TestMethod]
    public void NoLineBreak_Object_NameValue() {
        // setup
        Trace.Enter();
        TraceBase.MaximumDataOutputWidth = 60;
        var foo = new Contact("000000000011111111112222222222333333333344444444445555555555", "", (2021, 1, 1), "");


        // when
        Trace.Print("foo", foo);

        // then
        StringAssert.Contains(Trace.LastLog, "FirstName: (Length:60)\"0000000000");

        // cleanup
        Trace.Leave();
    }

    /** @since 3.1.1 */
    [TestMethod]
    public void NoLineBreak_KeyValue() {
        // setup
        Trace.Enter();
        TraceBase.MaximumDataOutputWidth = 60;
        var foo = new Dictionary<int, string>();
        foo[1] = "000000000011111111112222222222333333333344444444445555555555";


        // when
        Trace.Print("foo", foo);


        // then
        StringAssert.Contains(Trace.LastLog, "1: (Length:60)\"0000000000");

        // cleanup
        Trace.Leave();
    }
}
