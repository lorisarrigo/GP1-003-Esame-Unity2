public class BT_Leaf : BT_Node
{
    /*Nodo Leaf, le azioni vere e proprie:
        * nell'albero oltre al nome gli passiamo uno Stato che contiene l'azione, quello è il Tick, che essendo Delegate viene eseguito solo al momento del richiamo
    */
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
