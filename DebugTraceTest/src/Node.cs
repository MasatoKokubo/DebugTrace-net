// Node.cs
// (C) 2018 Masato Kokubo

namespace DebugTraceTest;

// Node class
public class Node<T> {
    public T Item {get;}
    public Node<T>? Left {get; set;}
    public Node<T>? Right {get; set;}

    public Node(T item) {
        Item = item;
    }

    public Node(T item, Node<T> left, Node<T> right) {
        Item = item; Left = left; Right = right;
    }
}
