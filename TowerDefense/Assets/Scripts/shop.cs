using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shop : MonoBehaviour

{

    [SerializeField]
    private GameObject PlaceableObjectPrefab;

    private GameObject CurrentPlaceableObject;

    public void PurchaseStandardTurret()
    {
        Debug.Log("Standard Turret Purchased");

        if (CurrentPlaceableObject == null)
        {
            CurrentPlaceableObject = Instantiate(PlaceableObjectPrefab);
        }
        else
        {
            Destroy(CurrentPlaceableObject);
        }
    }

    public void PurchaseAnotherTurret()
    {
        Debug.Log("Missle Turret Purchased");
    }

    private void Update()
    {
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
            CurrentPlaceableObject = null;
        }
    }
}
