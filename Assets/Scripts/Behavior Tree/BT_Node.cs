using UnityEngine;
using System.Collections.Generic;

public enum Status
{
    Success,
    Running,
    Failure
}

public class BT_Node : MonoBehaviour
{
    public Status Status;

    public List<BT_Node> children = new();

    public int currentChild = 0;

    public string nodeName;

    public BT_Node() { }

    public void AddChild(BT_Node n) { children.Add(n); }

    public virtual Status Process() { return children[currentChild].Process(); }
    void PrintName()
    {
        Debug.Log(nodeName);
    }
}
