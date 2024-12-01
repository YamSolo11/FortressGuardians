using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 30f;
    public float scrollSpeed = 2f;

    public float minY = 10f;
    public float maxY = 150f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            new Vector3(0f, 0f, panSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s"))
        {
            new Vector3(0f, 0f, panSpeed * Time.deltaTime);
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a"))
        {
            new Vector3(0f, 0f, panSpeed * Time.deltaTime);
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d"))
        {
            new Vector3(0f, 0f, panSpeed * Time.deltaTime);
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;
        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
      
        transform.position = pos;
    }
}
