using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlacementController : MonoBehaviour
{
    //Add the options for the object placed and the Key used.
    [SerializeField]
    private GameObject PlaceableObjectPrefab;

    [SerializeField]
    private KeyCode NewObjectHotKey = KeyCode.A;

    private GameObject CurrentPlaceableObject;

    // Update is called once per frame
    private void Update()
    {
        HandleNewObjectHotKey();
        if (CurrentPlaceableObject != null)
        {
            MoveCurrentPlaceableObjectToMouse();
            ReleaseIfClicked();
        }
    }

    private void MoveCurrentPlaceableObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo)) 
        { 
            CurrentPlaceableObject.transform.position = hitInfo.point;
            CurrentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void HandleNewObjectHotKey()
    {
        if (Input.GetKeyDown(NewObjectHotKey))
        {
            if (CurrentPlaceableObject == null)
            {
                CurrentPlaceableObject = Instantiate(PlaceableObjectPrefab);
            }
            else
            {
                Destroy(CurrentPlaceableObject);
            }
        }
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CurrentPlaceableObject = null;
        }
    }
}
