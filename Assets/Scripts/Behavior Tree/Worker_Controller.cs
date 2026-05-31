using UnityEngine;
using UnityEngine.AI;

public enum ActionState
{ 
    Idle,
    Walking
}
public class Worker_Controller : MonoBehaviour
{
    public int maxEnergy;
    public int currentEnergy;

    ActionState state = ActionState.Idle;

    [SerializeField] Transform[] Waypoitns;
    NavMeshAgent agent;
    BT_Root workingRoot;

    Status treeState = Status.Running;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        workingRoot = new ("Root");

        //da qui in poi si crea l'albero

        // -- 1. Dichiaro i nodi --

        // 1.1 - Nodi strutturali (Sequence e Selector)

        //BT_Sequence workingLoop = new("Work");
        //BT_Selector HasEnergy = new ("Energy?");
        //BT_Sequence restRoutine = new ("Rest");

        // 1.2 A - Nodi Foglia - "fase Operativa"

        //BT_Leaf _GoToStart = new("Start", GoToStart);
        //BT_Leaf _GetOrder = new("Order", GetOrder);
        //BT_Leaf _GatherMaterials = new("Gather", GatherMaterials); //all'interno farò il ciclo per prendere i materiali e fare il restock per ogni singolo materiale
        //BT_Leaf _Craft = new("Craft", Craft);
        //BT_Leaf _Send = new("Send", Send);

        // 1.2 B - Nodi Foglia - "fase Riposo"

        //BT_Leaf _EnergyCheck = new ("ECheck", EnergyCheck);
        //BT_Leaf _Rest = new("Rest", Rest);
        //BT_Leaf _Recharge = new ("Recharge", Recharge);


        // -- 2. Creo l'albero --

        //workingRoot.AddChild(workingLoop);

        //workingLoop.AddChild(_GoToStart);
        //workingLoop.AddChild(_GetOrder);
        //workingLoop.AddChild(_GatherMaterials);
        //workingLoop.AddChild(_Craft);
        //workingLoop.AddChild(_Send);
        //workingLoop.AddChild(HasEnergy);

        //    HasEnergy.AddChild(_EnergyCheck);
        //    HasEnergy.AddChild(restRoutine);

        //        restRoutine.AddChild(_Rest);
        //        restRoutine.AddChild(_Recharge);


        workingRoot.PrintTree();
    }
    private void Update()
    {
        treeState = workingRoot.Process();
    }

    //public Status GoToStart()
    //{
        
    //}

    //public Status GetOrder()
    //{
        
    //}

    //public Status GatherMaterials()
    //{
        
    //}
    //public Status Craft()
    //{
        
    //}
    //public Status Send()
    //{
        
    //}
    //public Status EnergyCheck()
    //{
        
    //}
    //public Status Rest()
    //{
        
    //}
    //public Status Recharge()
    //{

    //}
}
