using UnityEngine;

public class PressureButton : MonoBehaviour
{
    public string buttonID; // e.g., "Red", "Blue"
    private int objectsOnButton = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Add more tags if needed
        {
            objectsOnButton++;
            DoorManager.Instance.SetDoorState(buttonID, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            objectsOnButton--;
            if (objectsOnButton <= 0)
            {
                DoorManager.Instance.SetDoorState(buttonID, false);
            }
        }
    }
}
