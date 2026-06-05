using System;
using UnityEngine;
using UnityEngine.AI;
public enum WorkingStates
{ 
    Walking,
    Idle,
}
public class Worker_Controller : MonoBehaviour
{
    //Controller dell'IA del lavoratore, contiene il Bheavior tree e le varie azioni che deve svolgere 

    //setting componenti e variabili
    NavMeshAgent agent;
    BT_Root workingRoot;

    WorkingStates state = WorkingStates.Idle;
    Status treeState = Status.Running;
    
    /*bool per controllare se sta lavorando e se ha il materiale:
     * isWorking permette di switchare tra l'affaticamento e la ricarica
     * hasMaterial permette di riconoscere se il lavoratore ha preso o meno un il materiale corrente
     */
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

    //questa variabile è un indice che permette al controller di capire quale materiale deve prendere
    int crateCheck = 0;

    //Eventi (in parte ad ogniuno ho segnato gli script iscritti agli eventi
    public static event Action OnUpdateEnergy; //UIManager
    public static event Action OnQuantityLeft; //UIManager
    public static event Action OnRandomProduct; //GameManager
    public static event Action OnPlaceWb; //GameManager
    public static event Action OnDeplace; //GameManager
    public static event Action OnSend; //UIManager

    /*Singleton
     * la classe viene viene richiamata in:
        * UIManager per aggiornare la fillbar
     */
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
        // 1.1 - Nodo Radice (Root)
        workingRoot = new ("Root");

        // 1.2 - Nodi strutturali (Sequence e Selector)
        BT_Sequence workingLoop = new("Work");
        BT_Selector HasEnergy = new ("Energy?");
        BT_Sequence restingSequence = new ("Rest");

        // 1.3 A - Nodi Foglia - "fase Operativa"

        BT_Leaf _GoToStart = new("Start", GoToStart);
        BT_Leaf _GetOrder = new("Order", GetOrder);
        BT_Leaf _GatherMaterials = new("Gather", GatherMaterials);
        BT_Leaf _Send = new("Send", Send);

        // 1.3 B - Nodi Foglia - "fase Riposo"

        BT_Leaf _EnergyCheck = new ("ECheck", EnergyCheck);
        BT_Leaf _GoToRest = new("GoRest", GoToRest);
        BT_Leaf _Recharge = new ("Recharge", Recharge);


        // -- 2. Creo l'albero --

        // 2.1 lo fa partire dalla prima Sequence
        workingRoot.AddChild(workingLoop);

        // 2.2 esegue la Sequence
        workingLoop.AddChild(_GoToStart);
        workingLoop.AddChild(_GetOrder);
        workingLoop.AddChild(_GatherMaterials);
        workingLoop.AddChild(_Send);

        // 2.3 arriva al Selector
        workingLoop.AddChild(HasEnergy);
            
        // 2.4 esegue il Selector, il quale secondo nodo è una Sequence
            HasEnergy.AddChild(_EnergyCheck);
            HasEnergy.AddChild(restingSequence);

        // 2.5 nel caso esegue la Sequence
                restingSequence.AddChild(_GoToRest);
                restingSequence.AddChild(_Recharge);
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
        * invoca l'evento che sceglie randomicamente un'ordine nel GameManager;
        * setta il "crateCheck" a 0;
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
        * nel caso in cui non è richiesto il materiale lo salta;
        * controlla se ha preso il materiale:
            * se non lo ha preso, controlla se c'è ne sono abbastanza:
                * se quel materiale è finito, va a rifornirlo;
                *  se no, va a prendere il materiale richiesto, riduce la quantità presa da quella presente, e passa alla workbench;
            * se no, lo mette sulla workbench e aumenta il Check;
        * tutto viene ciclato grazie al controllo iniziale, se il Check è al massimo passa al nodo successivo.
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
        * se le ha, ricomincia il workingLoop, in quanto Selector se questa non fallisce ignorerà il resto;
        * se non la ha, fallisce il nodo e passa alla restingSequence.
     */
    Status EnergyCheck()
    {
        OnSend?.Invoke();
        if (currentEnergy >= 0) return Status.Success;
        else return Status.Failure;
    }
    //lo mandiamo a riposare.
    Status GoToRest()
    {
        return MoveTo(rechargeStation.position);
    }
    /*una volta raggiunto la rechargeStation:
        * setta a false "isWorking";
        * e finchè l'energia non è al massimo resta nel nodo, fermandolo alla recharge station.
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
    /* lo stato che fa:
        * capire al worker se deve andare a prendere un materiale o meno;
        * se di quel materiale ne ha abbastanza, nel caso non ne ha va a riempire le casse;
        * craftare l'oggetto.
     */
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
                    if (Product_Manager.instance.leftQuantity[crateCheck] == 0)
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
