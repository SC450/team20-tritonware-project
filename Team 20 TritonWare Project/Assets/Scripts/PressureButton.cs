using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    public string buttonID; // e.g., "Red", "Blue"
    public List<string> triggerTags = new List<string>{"Player", "Object"};
    private int objectsOnButton = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerTags.Contains(other.tag)) // Add more tags if needed
        {
            objectsOnButton++;
            DoorManager.Instance.SetDoorState(buttonID, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (triggerTags.Contains(other.tag))
        {
            objectsOnButton--;
            if (objectsOnButton <= 0)
            {
                DoorManager.Instance.SetDoorState(buttonID, false);
            }
        }
    }
}
