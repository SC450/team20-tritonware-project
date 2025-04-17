using UnityEngine;

public class MainPlatformSetup : MonoBehaviour
{
    // Get the main platform object and number of times to repeat it
    public GameObject mainPlatform;
    public int count;
    public float separationDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 1; i <= count; i++) 
        {
            GameObject clonedPlatform = Instantiate(mainPlatform, transform.position, transform.rotation);
            clonedPlatform.transform.position = new Vector3(transform.position.x + separationDistance * i, transform.position.y, transform.position.z);
            clonedPlatform.transform.parent = transform.parent;
        }
    }
}
