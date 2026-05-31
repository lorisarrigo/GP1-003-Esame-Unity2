using UnityEngine;
using UnityEngine.AI;

public enum WorkingStates
{ 
    Idle,
    Walking
}
public class Worker_Controller : MonoBehaviour
{
    public int maxEnergy;
    public int currentEnergy;

    [Header("Waypoints Singoli")]
    [SerializeField] Transform startW;
    [SerializeField] Transform rechargeStation;
    [SerializeField] Transform workbench;
    [SerializeField] Transform speditionTable;


    [Header("Waypoints per i materiali")]
    [SerializeField] Transform[] crates; //le casse in basso dove si trovano i materiali
    [SerializeField] Transform[] warehouses; //i magazzini in alto dove il lavoratore farà il restock


    WorkingStates state = WorkingStates.Idle;
    
    NavMeshAgent agent;
    BT_Root workingRoot;

    Status treeState = Status.Running;

    public static Worker_Controller instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        currentEnergy = maxEnergy;
        agent = GetComponent<NavMeshAgent>();

        workingRoot = new BT_Root("Root");

        //da qui in poi si crea l'albero

        // -- 1. Dichiaro i nodi --

        // 1.1 - Nodi strutturali (Sequence e Selector)
        BT_Sequence workingLoop = new("Work");
        BT_Selector HasEnergy = new ("Energy?");
        BT_Sequence restRoutine = new ("Rest");

        // 1.2 A - Nodi Foglia - "fase Operativa"

        BT_Leaf _GoToStart = new("Start", GoToStart);
        //BT_Leaf _GetOrder = new("Order", GetOrder);
        //BT_Leaf _GatherMaterials = new("Gather", GatherMaterials); //all'interno farò il ciclo per prendere i materiali e fare il restock per ogni singolo materiale
        BT_Leaf _Craft = new("Craft", Craft);
        BT_Leaf _Send = new("Send", Send);

        //// 1.2 B - Nodi Foglia - "fase Riposo"

        //BT_Leaf _EnergyCheck = new ("ECheck", EnergyCheck);
        BT_Leaf _Rest = new("Rest", Rest);
        //BT_Leaf _Recharge = new ("Recharge", Recharge);


        // -- 2. Creo l'albero --

        workingRoot.AddChild(workingLoop);

        workingLoop.AddChild(_GoToStart);
        //workingLoop.AddChild(_GetOrder);
        //workingLoop.AddChild(_GatherMaterials);
        workingLoop.AddChild(_Craft);
        workingLoop.AddChild(_Send);
        workingLoop.AddChild(HasEnergy);

        //    HasEnergy.AddChild(_EnergyCheck);
            HasEnergy.AddChild(restRoutine);

                restRoutine.AddChild(_Rest);
        //        restRoutine.AddChild(_Recharge);


        workingRoot.PrintTree();
    }
    private void Update()
    {
        treeState = workingRoot.Process();
        if(treeState != Status.Running )
        {
            workingRoot.currentChild = 0;
            treeState = Status.Running;
        }
    }

    private Status MoveTo(Vector3 destination)
    {
        if (state == WorkingStates.Idle)
        {
            agent.SetDestination(destination);
            state = WorkingStates.Walking;
            return Status.Running;
        }
        if(agent.pathPending) return Status.Running;

        if(agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            state = WorkingStates.Idle;
            return Status.Success;
        }
        return Status.Running;
    }

    public Status GoToStart()
    {
        return MoveTo(startW.position);
    }

    //public Status GetOrder()
    //{
    //    Debug.Log("ottengo l'ordine");
    //}

    //public Status GatherMaterials()
    //{
      
    //}
    public Status Craft()
    {
        return MoveTo(workbench.position);
    }
    public Status Send()
    {
        return MoveTo(speditionTable.position);
    }
    //public Status EnergyCheck()
    //{
        
    //}
    public Status Rest()
    {
        return MoveTo(rechargeStation.position);
    }
    //public Status Recharge()
    //{

    //}
}
