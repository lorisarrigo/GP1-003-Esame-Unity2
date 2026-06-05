public class BT_Sequence : BT_Node
{
    /*Nodo Sequence, comparato alla logica AND:
        * Se uno dei Nodi Leaf fallisce falliscono anche quelli successivi; 
        * avendo un nodo Leaf A e un nodo Leaf B, se A ritorna Success, passa a B, altrimenti va alla "ramificazione" successiva; 
    */
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
