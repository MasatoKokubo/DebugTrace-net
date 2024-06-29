// PrintClassTest.cs
// (C) 2018 Masato Kokubo
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugTrace;
using System.Collections.Generic;

namespace DebugTraceTest;

[TestClass]
public class PrintOptionsTest {
    [TestMethod]
    public void ForceReflection() {
        var p = new Point3(1, 2, 3);
        Trace.Print("p", p);
        StringAssert.Contains(Trace.LastLog, "(1, 2, 3)");

        Trace.Print("p", p, forceReflection:false);
        StringAssert.Contains(Trace.LastLog, "(1, 2, 3)");

        Trace.Print("p", p, forceReflection:true);
        StringAssert.Contains(Trace.LastLog, "{X: 1, Y: 2, Z: 3}");
    }

    [TestMethod]
    public void OutputNonPublic() {
        var fieldsProperties = new FieldsProperties();
        Trace.Print("fieldsProperties", fieldsProperties);

        Assert.IsFalse(Trace.LastLog.Contains(                     "private PrivateField"));
        Assert.IsFalse(Trace.LastLog.Contains(                 "protected ProtectedField"));
        Assert.IsFalse(Trace.LastLog.Contains(                   "internal InternalField"));
        Assert.IsFalse(Trace.LastLog.Contains("protected internal ProtectedInternalField"));
        Assert.IsFalse(Trace.LastLog.Contains(  "private protected PrivateProtectedField"));
        Assert.IsTrue (Trace.LastLog.Contains(                              "PublicField"));

        Assert.IsFalse(Trace.LastLog.Contains(                     "private PrivateProperty"));
        Assert.IsFalse(Trace.LastLog.Contains(                 "protected ProtectedProperty"));
        Assert.IsFalse(Trace.LastLog.Contains(                   "internal InternalProperty"));
        Assert.IsFalse(Trace.LastLog.Contains("protected internal ProtectedInternalProperty"));
        Assert.IsFalse(Trace.LastLog.Contains(  "private protected PrivateProtectedProperty"));
        Assert.IsTrue (Trace.LastLog.Contains(                              "PublicProperty"));

        Trace.Print("fieldsProperties", fieldsProperties, outputNonPublicFields:true);

        Assert.IsTrue (Trace.LastLog.Contains(                     "private PrivateField"));
        Assert.IsTrue (Trace.LastLog.Contains(                 "protected ProtectedField"));
        Assert.IsTrue (Trace.LastLog.Contains(                   "internal InternalField"));
        Assert.IsTrue (Trace.LastLog.Contains("protected internal ProtectedInternalField"));
        Assert.IsTrue (Trace.LastLog.Contains(  "private protected PrivateProtectedField"));
        Assert.IsTrue (Trace.LastLog.Contains(                              "PublicField"));

        Assert.IsFalse(Trace.LastLog.Contains(                     "private PrivateProperty"));
        Assert.IsFalse(Trace.LastLog.Contains(                 "protected ProtectedProperty"));
        Assert.IsFalse(Trace.LastLog.Contains(                   "internal InternalProperty"));
        Assert.IsFalse(Trace.LastLog.Contains("protected internal ProtectedInternalProperty"));
        Assert.IsFalse(Trace.LastLog.Contains(  "private protected PrivateProtectedProperty"));
        Assert.IsTrue (Trace.LastLog.Contains(                              "PublicProperty"));

        Trace.Print("fieldsProperties", fieldsProperties, outputNonPublicProperties:true);

        Assert.IsFalse(Trace.LastLog.Contains(                     "private PrivateField"));
        Assert.IsFalse(Trace.LastLog.Contains(                 "protected ProtectedField"));
        Assert.IsFalse(Trace.LastLog.Contains(                   "internal InternalField"));
        Assert.IsFalse(Trace.LastLog.Contains("protected internal ProtectedInternalField"));
        Assert.IsFalse(Trace.LastLog.Contains(  "private protected PrivateProtectedField"));
        Assert.IsTrue (Trace.LastLog.Contains(                              "PublicField"));

        Assert.IsTrue (Trace.LastLog.Contains(                     "private PrivateProperty"));
        Assert.IsTrue (Trace.LastLog.Contains(                 "protected ProtectedProperty"));
        Assert.IsTrue (Trace.LastLog.Contains(                   "internal InternalProperty"));
        Assert.IsTrue (Trace.LastLog.Contains("protected internal ProtectedInternalProperty"));
        Assert.IsTrue (Trace.LastLog.Contains(  "private protected PrivateProtectedProperty"));
        Assert.IsTrue (Trace.LastLog.Contains(                              "PublicProperty"));

        Trace.Print("fieldsProperties", fieldsProperties, outputNonPublicFields:true, outputNonPublicProperties:true);

        Assert.IsTrue (Trace.LastLog.Contains(                     "private PrivateField"));
        Assert.IsTrue (Trace.LastLog.Contains(                 "protected ProtectedField"));
        Assert.IsTrue (Trace.LastLog.Contains(                   "internal InternalField"));
        Assert.IsTrue (Trace.LastLog.Contains("protected internal ProtectedInternalField"));
        Assert.IsTrue (Trace.LastLog.Contains(  "private protected PrivateProtectedField"));
        Assert.IsTrue (Trace.LastLog.Contains(                              "PublicField"));

        Assert.IsTrue (Trace.LastLog.Contains(                     "private PrivateProperty"));
        Assert.IsTrue (Trace.LastLog.Contains(                 "protected ProtectedProperty"));
        Assert.IsTrue (Trace.LastLog.Contains(                   "internal InternalProperty"));
        Assert.IsTrue (Trace.LastLog.Contains("protected internal ProtectedInternalProperty"));
        Assert.IsTrue (Trace.LastLog.Contains(  "private protected PrivateProtectedProperty"));
        Assert.IsTrue (Trace.LastLog.Contains(                              "PublicProperty"));

    }

    [TestMethod]
    public void MinimumOutputCount() {
        var list = new List<int>() {1, 2, 3};
        Trace.Print("list", list, minimumOutputCount:3);
        StringAssert.Contains(Trace.LastLog, "List<int> Count:3 {1, 2, 3}");

        Trace.Print("list", list, minimumOutputCount:4);
        StringAssert.Contains(Trace.LastLog, "List<int> {1, 2, 3}");
    }

    [TestMethod]
    public void MinimumOutputLength() {
        var str = "ABC";
        Trace.Print("str", str, minimumOutputLength:3);
        StringAssert.Contains(Trace.LastLog, "= (Length:3)\"ABC\"");

        Trace.Print("str", str, minimumOutputLength:4);
        StringAssert.Contains(Trace.LastLog, "= \"ABC\"");
    }

    [TestMethod]
    public void CollectionLimit() {
        var list = new List<int>() {1, 2, 3};
        Trace.Print("list", list, collectionLimit:2);
        StringAssert.Contains(Trace.LastLog, "List<int> {1, 2, ...}");

        Trace.Print("list", list, collectionLimit:3);
        StringAssert.Contains(Trace.LastLog, "List<int> {1, 2, 3}");
    }

    [TestMethod]
    public void StringLimit() {
        var str = "ABC";
        Trace.Print("str", str, stringLimit:2);
        StringAssert.Contains(Trace.LastLog, "= \"AB...\"");

        Trace.Print("str", str, stringLimit:3);
        StringAssert.Contains(Trace.LastLog, "= \"ABC\"");
    }

    [TestMethod]
    public void ReflectionNestLimit() {
        var node =
                new Node<int>(1, null,
                    new Node<int>(2, null,
                        new Node<int>(3, null,
                            new Node<int>(4, null, null))));


        Trace.Print("node Limit:0", node, reflectionNestLimit:0);
        Assert.IsFalse(Trace.LastLog.Contains("Item: 1"));

        Trace.Print("node Limit:1", node, reflectionNestLimit:1);
        Assert.IsTrue (Trace.LastLog.Contains("Item: 1"));
        Assert.IsFalse(Trace.LastLog.Contains("Item: 2"));

        Trace.Print("node Limit:2", node, reflectionNestLimit:2);
        Assert.IsTrue (Trace.LastLog.Contains("Item: 2"));
        Assert.IsFalse(Trace.LastLog.Contains("Item: 3"));

        Trace.Print("node Limit:3", node, reflectionNestLimit:3);
        Assert.IsTrue (Trace.LastLog.Contains("Item: 3"));
        Assert.IsFalse(Trace.LastLog.Contains("Item: 4"));

        Trace.Print("node Limit:4", node);
        Assert.IsTrue (Trace.LastLog.Contains("Item: 4"));
    }
}
