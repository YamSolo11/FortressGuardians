using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour

{

    [SerializeField]
    private GameObject PlaceableObjectPrefab;
    private BoxCollider _actionObject;
    public int cost = 0;

    [SerializeField]
    public GameMaster MainHub;

    private GameObject CurrentPlaceableObject;

    public void PurchaseStandardTurret()
    {
        if((MainHub.currency - cost) >= 0)
        {
            if (CurrentPlaceableObject == null)
            {
                CurrentPlaceableObject = Instantiate(PlaceableObjectPrefab);
                _actionObject = CurrentPlaceableObject.GetComponent<BoxCollider>();
            }
        }
        else
        {
            Debug.Log("Not Enough Money");
        }
       
    }

    public void PurchaseAnotherTurret()
    {
        //if there is enough money, let them buy the turret
        if ((MainHub.currency - cost) >= 0) 
        {
            if (CurrentPlaceableObject == null)
            {
                CurrentPlaceableObject = Instantiate(PlaceableObjectPrefab); //Create the new turret in scene
                _actionObject = CurrentPlaceableObject.GetComponent<BoxCollider>(); //get the hitbox of the turret to turn on later
            }
        }
        else
        {
            Debug.Log("Not Enough Money");
        }
    }

    public void UpdateTurret()
    {
        //Upgrade the turret. Basic. Might need to give them options later
        if ((MainHub.currency - cost) >= 0)
        {
            MainHub.upgrade();
            MainHub.currency = MainHub.currency - cost;
        }
        else
        {
            Debug.Log("Not Enough Money");
        }
    }

    //Sell the turret
    public void SellTurret()
    {
        MainHub.currency = MainHub.currency + cost;
        MainHub.sell();
    }

    private void Update()
    {
        //If there is a placible turret created in the scene by the button, do this
        if (CurrentPlaceableObject != null)
        {
            MoveCurrentPlaceableObjectToMouse(); //Move the tower to the mouse
            ReleaseIfClicked(); //release tower if clicked

            //If they press Q, then the tower gets deleted and you don't buy it
            if(Input.GetKeyUp(KeyCode.Q))
            {
                Destroy(CurrentPlaceableObject);
                CurrentPlaceableObject = null;
            }
        }
    }

    private void MoveCurrentPlaceableObjectToMouse()
    {
        //Get the mouse location for main camera
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //move the tower with the mouse location
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            CurrentPlaceableObject.transform.position = hitInfo.point;
            CurrentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void ReleaseIfClicked()
    {
        //Once you click the mouse on the ground, places the object.
        if (Input.GetMouseButtonDown(0))
        {
            _actionObject.enabled = true; //turn on box collider
            MainHub.currency = MainHub.currency - cost; //subtracts the cost
            CurrentPlaceableObject = null; //makes the current item null.
        }
    }
}
