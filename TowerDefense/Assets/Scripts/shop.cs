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
        if ((MainHub.currency - cost) >= 0)
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

    public void UpdateTurret()
    {
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

    public void SellTurret()
    {
        MainHub.currency = MainHub.currency + cost;
        MainHub.sell();
    }

    private void Update()
    {
        if (CurrentPlaceableObject != null)
        {
            MoveCurrentPlaceableObjectToMouse();
            ReleaseIfClicked();
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                Destroy(CurrentPlaceableObject);
                CurrentPlaceableObject = null;
            }
        }
    }

    private void MoveCurrentPlaceableObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            CurrentPlaceableObject.transform.position = hitInfo.point;
            CurrentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _actionObject.enabled = true;
            MainHub.currency = MainHub.currency - cost;
            CurrentPlaceableObject = null;
        }
    }
}
