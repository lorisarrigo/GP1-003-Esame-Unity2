using System.Collections.Generic;

public enum Status
{
    //lista dei nodi funzionali:
    Running, //"cicla" il nodo finche non ritorna uno degli altri 2 Status
    Success, //"marca" come eseguito l'ordine e passa al nodo successivo (Nodo A -> Nodo B, dove il nodo B viene immediatamente dopo il Nodo A)
    Failure  //"marca" come fallito il Nodo, perciò passa ad un'altro nodo alternativo (Nodo A -> Nodo B, dove il nodo B non viene subito dopo ma sì trova in un Nodo alternativo)
}

public class BT_Node
{
    /*Classe base per qualsiasi tipo di nodo del Behavior Tree ì, contiene le info del nodo:
         * l'implementazione degli Status funzionali;
         * la lista dei nodi;
         * l'indice del nodo che sta eseguendo
         * il nome
         * le funzioni di base dei nodi:
            * BT_Node: costruttore vuoto, viene riempito quando dichiariamo il nodo nel Tree
            * AddChild: aggiunge il nodo alla lista per la struttura del Bheavior tree
            * Process: lo stato principale che vienne overraidato, ritorna il Process del figlio
     */
    public Status Status;

    public List<BT_Node> children = new();

    public int currentChild = 0;

    public string nodeName;

    public BT_Node() { }

    public void AddChild(BT_Node n) { children.Add(n); }

    public virtual Status Process() { return children[currentChild].Process(); }
}
