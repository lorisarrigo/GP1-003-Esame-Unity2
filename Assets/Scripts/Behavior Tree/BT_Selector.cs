using UnityEngine;

public class BT_Selector : BT_Node
{
    public BT_Selector(string n) { nodeName = n; }

    public override Status Process()
    {
        Status childStatus = children[currentChild].Process();

        Debug.Log(children[currentChild].nodeName);

        if (childStatus == Status.Running) return childStatus;
        if (childStatus == Status.Success)
        {
            currentChild = 0;
            return Status.Success;
        }
        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.Failure;
        }
        return Status.Running;
    }
}
