using UnityEngine;
using UnityEngine.InputSystem;

public class GrabObjects : MonoBehaviour
{
    // Serializables
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float rayDistance;

    // Variables for grabbing objects
    private GameObject grabbedObject;
    private int layerIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize layerIndex
        layerIndex = LayerMask.NameToLayer("Objects");
    }

    // Update is called once per frame
    void Update()
    {
        // Send raycast to check if object can be picked up
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance);

        // Check for hit
        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex)
        {
            Debug.Log("Wow1");
            // Grab object
            if (Keyboard.current.eKey.wasPressedThisFrame && grabbedObject == null)
            {
                Debug.Log("bruhhh");
                grabbedObject = hitInfo.collider.gameObject;
                grabbedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                grabbedObject.transform.position = grabPoint.position;
                grabbedObject.transform.SetParent(transform);
            }
            // Drop object
            else if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                grabbedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                grabbedObject.transform.SetParent(null);
                grabbedObject = null;
            }
        }

        // For debugging purposes
        Debug.DrawRay(rayPoint.position, transform.right * rayDistance, Color.white);
    }
}
