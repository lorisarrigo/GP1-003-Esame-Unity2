public class BT_Sequence : BT_Node
{
    public BT_Sequence(string n) { nodeName = n; }

    public override Status Process()
    {
        Status childStatus = children[currentChild].Process();

        if (childStatus == Status.Running) return childStatus;
        if (childStatus == Status.Failure) return childStatus;

        currentChild++;

        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.Success;
        }
        return Status.Running;
    }
}
