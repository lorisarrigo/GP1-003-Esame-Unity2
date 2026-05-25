using UnityEngine;

public class BT_Root : BT_Node
{
    public BT_Root(string n) { nodeName = n; }
    public void PrintTree()
    {
        Debug.Log(nodeName);
    }
}
