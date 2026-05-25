using UnityEngine;
using UnityEngine.AI;

public enum ActionState
{ 
    Idle,
    Walking
}
public class Worker_Controller : MonoBehaviour
{
    ActionState state = ActionState.Idle;

    [SerializeField] Transform[] Waypoitns;
    NavMeshAgent agent;
    BT_Root workingRoot;

    Status treeState = Status.Running;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        workingRoot = new BT_Root("Root");

        //da qui in poi si crea l'albero
        //BT_Sequence workingLoop = new("Work");

        //BT_Selector HasMat = new("Material?");
        //BT_Selector HasEnergy = new ("Energy?");

        //BT_Leaf _GoToStart = new("Start", GoToStart);
        //BT_Leaf _GetOrder = new("Order", GetOrder);

        //BT_Leaf _MatCheck = new("MCheck", MatCheck);
        //BT_Leaf _Restock = new("Restock", Restock);

        //BT_Leaf _GetMaterial = new("Material", GetMaterial);
        //BT_Leaf _Place = new("Place", Place);
        //BT_Leaf _Craft = new("Craft", Craft);
        //BT_Leaf _Send = new("Send", Send);

        //BT_Leaf _EnergyCheck ("ECheck", EnergyCheck);
        //BT_Leaf _Rest = new("Rest", Rest);

        //workingRoot.AddChild(workingLoop);

            //workingLoop.AddChild(_GoToStart);
            //workingLoop.AddChild(_GetOrder);
            //workingLoop.AddChild(HasMat);

                //HasMat.AddChild(_MatCheck);
                //HasMat.AddChild(_Restock);

            //workingLoop.AddChild(_GetMaterial);
            //workingLoop.AddChild(_Place);
            //workingLoop.AddChild(_Craft);
            //workingLoop.AddChild(_Send);
            //workingLoop.AddChild(HasEnergy);

                //hasEnergy.AddChild(_EnergyCheck);
                //HasEnergy.AddChild(_Rest);


        //workingRoot.PrintTree();
    }
    private void Update()
    {
        treeState = workingRoot.Process();
    }

    //public Status GoToStart()
    //{
    //    
    //}

    //public Status GetOrder()
    //{
    //    
    //}

    //public Status MatCheck()
    //{
    //    
    //}

    //public Status Restock()
    //{
    //    
    //}

    //public Status GetMaterial()
    //{
    //    
    //}

    //public Status Place()
    //{
    //    
    //}

    //public Status Craft()
    //{
    //    
    //}

    //public Status Send()
    //{
    //    
    //}

    //public Status EnergyCheck()
    //{
    //    
    //}

    //public Status Rest()
    //{
    //    
    //}
}
