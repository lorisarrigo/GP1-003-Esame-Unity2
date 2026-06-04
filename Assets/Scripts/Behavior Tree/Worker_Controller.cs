using System;
using UnityEngine;
using UnityEngine.AI;
public enum WorkingStates
{ 
    Idle,
    Walking
}
public class Worker_Controller : MonoBehaviour
{
    bool isWorking = false;
    bool hasMaterial = false;

    [Header("Energy")]
    public int maxEnergy;
    public float currentEnergy;

    [Header("Waypoints Singoli")]
    [SerializeField] Transform startW;
    [SerializeField] Transform workbench;
    [SerializeField] Transform speditionTable;
    [SerializeField] Transform rechargeStation;

    [Header("Waypoints per i materiali")]
    [SerializeField] Transform[] cratesW;
    [SerializeField] Transform[] warehousesW;

    int crateCheck = 0;

    WorkingStates state = WorkingStates.Idle;
    
    NavMeshAgent agent;
    BT_Root workingRoot;

    Status treeState = Status.Running;

    //Eventi
    public static event Action OnUpdateEnergy;
    public static event Action OnQuantityLeft;
    public static event Action OnRandomProduct;
    public static event Action OnPlaceWb;
    public static event Action OnDeplace;
    public static event Action OnSend;

    //Singleton
    public static Worker_Controller instance;
    void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }
    void Start()
    {
        currentEnergy = maxEnergy;
        agent = GetComponent<NavMeshAgent>();

        //da qui in poi si crea l'albero

        // -- 1. Dichiaro i nodi --
        workingRoot = new ("Root");

        // 1.1 - Nodi strutturali (Sequence e Selector)
        BT_Sequence workingLoop = new("Work");
        BT_Selector HasEnergy = new ("Energy?");
        BT_Sequence restingSequence = new ("Rest");

        // 1.2 A - Nodi Foglia - "fase Operativa"

        BT_Leaf _GoToStart = new("Start", GoToStart);
        BT_Leaf _GetOrder = new("Order", GetOrder);
        BT_Leaf _GatherMaterials = new("Gather", GatherMaterials); //all'interno farò il ciclo per prendere i materiali e fare il restock per ogni singolo materiale
        BT_Leaf _Send = new("Send", Send);

        //// 1.2 B - Nodi Foglia - "fase Riposo"

        BT_Leaf _EnergyCheck = new ("ECheck", EnergyCheck);
        BT_Leaf _GoToRest = new("GoRest", GoToRest);
        BT_Leaf _Recharge = new ("Recharge", Recharge);


        // -- 2. Creo l'albero --

        workingRoot.AddChild(workingLoop);

        workingLoop.AddChild(_GoToStart);
        workingLoop.AddChild(_GetOrder);
        workingLoop.AddChild(_GatherMaterials);
        workingLoop.AddChild(_Send);
        workingLoop.AddChild(HasEnergy);

            HasEnergy.AddChild(_EnergyCheck);
            HasEnergy.AddChild(restingSequence);

                restingSequence.AddChild(_GoToRest);
                restingSequence.AddChild(_Recharge);


        workingRoot.PrintTree();
    }
    void Update()
    {
        //Deploy e Recharge: quando il worker ottiene l'ordine inizia a scendere l'energia 
        if (isWorking && currentEnergy > 0)
        {
            currentEnergy -= Time.deltaTime;
            OnUpdateEnergy?.Invoke();
        }
        //mentre, quando finisce e arriva alla rechargeStation la ricarica
        else if (!isWorking && currentEnergy < maxEnergy)
        {
            currentEnergy += Time.deltaTime;
            OnUpdateEnergy?.Invoke();
        }

        //permettiamo all'albero di "ciclare all'infinito"
        treeState = workingRoot.Process();
        if(treeState != Status.Running )
        {
            workingRoot.currentChild = 0;
            treeState = Status.Running;
        }
    }
    //va allo startPoint
    Status GoToStart()
    {
        return MoveTo(startW.position);
    }
    /*una volta raggiunto lo start ottiene l'ordine:
     * setta la variabile "isWorking" a true per iniziare il deploying dell'energia;
     * invoca l'evento che sceglie randomicamente un'rdine nel GameManager;
     * setta il "crateCheck a 0;
     * e passa al prossimo nodo.
     */
    Status GetOrder()
    {
        isWorking = true;
        OnRandomProduct?.Invoke();
        crateCheck = 0;
        return Status.Success;
    }
    //una volta mostrato l'ordine controlla se ci sono abbastanza materiali, nel caso non ci siano rifornisce, recupera i materiali e inizia a craftare
    /* spiegazione a punti
     * controlla se il crateCheck è al massimo;
     * nel caso in cui non è richiesto il materiale lo salta 
     * controlla se ha preso il materiale:
     *      se non lo ha preso, controlla se c'è ne sono abbastanza:
     *          se quel materiale è finito, va a rifornirlo;
     *          se no, va a prendere il materiale richiesto, riduce la quantità presa da quella presente, e passa alla workbench
     *      se no, lo mette sulla workbench e aumenta il Check
     * tutto viene ciclato grazie al controllo iniziale, se il Check è al massimo passa al nodo successivo
     */
    Status GatherMaterials()
    {
        return Gather();
    }

    //finito il prodotto lo invia
    Status Send()
    {
        OnDeplace?.Invoke();
        return MoveTo(speditionTable.position);
    }
    /*una volta consegnato controlla se ha abbastanza energie
     * invoca il reset dell'ordine sullo schermo;
     * se le ha, ricomincia il workingLoop, in quanto selector se questa non fallisce ignorerà il resto;
     * se non la ha fallisce il nodo e passa alla restingSequence.
     */
    Status EnergyCheck()
    {
        OnSend?.Invoke();
        if (currentEnergy >= 0) return Status.Success;
        else return Status.Failure;
    }
    //lo mandiamo a riposare
    Status GoToRest()
    {
        return MoveTo(rechargeStation.position);
    }
    /*una volta raggiunto la rechargeStation:
     * setta a false "isWorking"
     * e finchè l'energia non è al massimo resta nel nodo, fermandolo alla recharge station
     */
    Status Recharge()
    {
        isWorking = false;
        if (currentEnergy < maxEnergy) return Status.Running;
        else return Status.Success;
    }

    //lo stato che fa muovere il worker
    Status MoveTo(Vector3 destination)
    {
        if (state == WorkingStates.Idle)
        {
            agent.SetDestination(destination);
            state = WorkingStates.Walking;
        }
        else if (Vector3.SqrMagnitude(agent.pathEndPosition - destination) >= 0.1f)
        {
            state = WorkingStates.Idle;
            return Status.Failure;
        }
        else if (Vector3.SqrMagnitude(destination - transform.position) < 0.1f)
        {
            state = WorkingStates.Idle;
            return Status.Success;
        }
        return Status.Running;
    }
    Status Gather()
    {
        if (crateCheck >= 3) return Status.Success;
        if (Product_Manager.instance.currentOrder[crateCheck] == 0)
        {
            crateCheck++;
            return Status.Running;
        }
        if (!hasMaterial)
        {
            if (Product_Manager.instance.leftQuantity[crateCheck] < Product_Manager.instance.currentOrder[crateCheck])
            {
                Status wh = MoveTo(warehousesW[crateCheck].position);
                int restockQ = Product_Manager.instance.maxQuant - Product_Manager.instance.leftQuantity[crateCheck];
                if (wh == Status.Success)
                {
                    Product_Manager.instance.leftQuantity[crateCheck] += restockQ;
                    Product_Manager.instance.Material[crateCheck].SetActive(true);
                }
                OnQuantityLeft?.Invoke();
                return Status.Running;
            }
            else
            {
                Status crates = MoveTo(cratesW[crateCheck].position);
                if (crates == Status.Success)
                {
                    Product_Manager.instance.leftQuantity[crateCheck] -= Product_Manager.instance.currentOrder[crateCheck];
                    if (Product_Manager.instance.leftQuantity[crateCheck] <= 1)
                        Product_Manager.instance.Material[crateCheck].SetActive(false);
                    OnQuantityLeft?.Invoke();
                    hasMaterial = true;
                }
                return Status.Running;
            }
        }
        else
        {
            Status wb = MoveTo(workbench.position);
            if (wb == Status.Success)
            {
                OnPlaceWb?.Invoke();
                hasMaterial = false;
                crateCheck++;
            }
            return Status.Running;
        }
    }
}
