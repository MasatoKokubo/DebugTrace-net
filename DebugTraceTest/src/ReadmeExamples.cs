﻿// ReadmeExample1.cs
// (C) 2018 Masato Kokubo
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugTrace; // TODO: Debug

namespace Readme;

/// <summary>ReadmeExample1</summary>
[TestClass]
public class ReadmeExample1 {
    /// <summary>Example1</summary>
    [TestMethod]
    public void Example1() {
        Trace.Enter(); // TODO: Debug

        var contacts = new [] {
            new Contact(1, "Akane" , "Apple", new DateTime(1991, 2, 3)),
            new Contact(2, "Yukari", "Apple", new DateTime(1992, 3, 4))
        };
        Trace.Print("contacts", contacts); // TODO: Debug

        Trace.Leave(); // TODO: Debug
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
        Trace.Enter(); // TODO: Debug

        var task = TaskExample(value);
        task.Wait();
        Trace.Print("task", task); // TODO: Debug

        Trace.Leave(); // TODO: Debug
    }

    // TaskExample
    private async Task<int> TaskExample(int value) {
        var threasdId = Trace.Enter(); // TODO: Debug

        var task = await Task.Run<int>(() => {
            Trace.Enter(); // TODO: Debug
            Thread.Sleep(100);
            var result = value * value;
            Trace.Print("result", result); // TODO: Debug
            Trace.Leave(); // TODO: Debug
            return result;
        });

        Trace.Leave(threasdId); // TODO: Debug
        return task;
    }
}
