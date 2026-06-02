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
    public int maxEnergy;
    public float currentEnergy;
    [SerializeField] bool isWorking = false;
    [SerializeField] bool hasMaterial = false;

    [Header("Waypoints Singoli")]
    [SerializeField] Transform startW;
    [SerializeField] Transform workbench;
    [SerializeField] Transform speditionTable;
    [SerializeField] Transform rechargeStation;

    [Header("Waypoints per i materiali")]
    [SerializeField] Transform[] cratesW;
    [SerializeField] Transform[] warehousesW;

    [SerializeField] int crateCheck = 0;

    WorkingStates state = WorkingStates.Idle;
    
    NavMeshAgent agent;
    BT_Root workingRoot;

    Status treeState = Status.Running;
    //Eventi
    public static event Action UpdateEnergy;
    public static event Action OnSelectProduct;
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
        //BT_Leaf _Craft = new("Craft", Craft);
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
        //workingLoop.AddChild(_Craft);
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
            UpdateEnergy?.Invoke();
        }
        //mentre, quando finisce e arriva alla rechargeStation la ricarica
        else if (!isWorking && currentEnergy < maxEnergy)
        {
            currentEnergy += Time.deltaTime;
            UpdateEnergy?.Invoke();
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
    public Status GoToStart()
    {
        return MoveTo(startW.position);
    }
    /*una volta raggiunto lo start ottiene l'ordine:
     * setta la variabile "isWorking" a true per iniziare il deploying dell'energia;
     * invoca l'evento che sceglie randomicamente un'rdine nel GameManager;
     * setta il "crateCheck a 0;
     * e passa al prossimo nodo.
     */
    public Status GetOrder()
    {
        isWorking = true;
        OnSelectProduct?.Invoke();
        crateCheck = 0;
        return Status.Success;
    }
    /*una volta scelto l'ordine:
     * controlla se il crateCheck è al massimo;
     * nel caso in cui non è richiesto il materiale lo salta 
     * controlla se ha preso il materiale:
     *      se non lo ha preso, controlla se c'è ne sono abbastanza:
     *          se quel materiale è finito, va a rifornirlo;
     *          se no, va a prendere il materiale richiesto, riduce la quantità presa da quella presente, e passa alla workbench
     *      se no, lo mette sulla workbench e aumenta il Check
     * tutto viene ciclato grazie al controllo iniziale, se il Check è al massimo passa al nodo successivo
     */
    public Status GatherMaterials()
    {
        if (crateCheck >= 3) return Status.Success;
        if (UIManager.instance.currentOrder[crateCheck] == 0)
        {
            crateCheck++;
            return Status.Running;
        }
        if (!hasMaterial)
        {
            if (UIManager.instance.leftQuantity[crateCheck] < UIManager.instance.currentOrder[crateCheck])
            {
                Status wh = MoveTo(warehousesW[crateCheck].position);
                if (wh == Status.Success) UIManager.instance.leftQuantity[crateCheck] += 5;
                return Status.Running;
            }
            else
            {
                Status crates = MoveTo(cratesW[crateCheck].position);
                if (crates == Status.Success)
                {
                    UIManager.instance.leftQuantity[crateCheck] -= UIManager.instance.currentOrder[crateCheck];
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
                hasMaterial = false;
                crateCheck++;
            }
            return Status.Running;
        }
    }
    //nodo per craftare: da togliere in caso riesco a fare tutto nel nodo precedente
    //public Status Craft()
    //{
    //    return MoveTo(workbench.position);
    //}

    /*finito il prodotto lo invia:
     * invoca un evento richiamato nello UIManager per disattivare l'immagine dell'ordine (aggiungere anche il reset delle quantità dell'ordine)
     */
    public Status Send()
    {
        OnSend?.Invoke();
        return MoveTo(speditionTable.position);
    }
    /*una volta consegnato controlla se ha abbastanza energie
     * se le ha, ricomincia il workingLoop, in quanto selector se questa non fallisce ignorerà il resto
     * se non la ha fallisce il nodo e passa alla restingSequence
     */
    public Status EnergyCheck()
    {
        if(currentEnergy >= 0) return Status.Success;
        else return Status.Failure;
    }
    //lo mandiamo a riposare
    public Status GoToRest()
    {
        return MoveTo(rechargeStation.position);
    }
    /*una volta raggiunto la rechargeStation:
     * setta a false "isWorking"
     * e finchè l'energia non è al massimo resta nel nodo, fermandolo alla recharge station
     */
    public Status Recharge()
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
            return Status.Running;
        }
        if (agent.pathPending) return Status.Running;

        if (agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            state = WorkingStates.Idle;
            return Status.Success;
        }
        return Status.Running;
    }
}
