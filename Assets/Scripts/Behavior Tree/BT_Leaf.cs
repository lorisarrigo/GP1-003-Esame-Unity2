public class BT_Leaf : BT_Node
{
    public delegate Status Tick();
    public Tick ProcessMethod;

    public BT_Leaf(string n, Tick pm)
    {
        nodeName = n;
        ProcessMethod = pm;
    }
    public override Status Process()
    {
        if (ProcessMethod != null)
            return ProcessMethod();
        return Status.Failure;
    }
}
