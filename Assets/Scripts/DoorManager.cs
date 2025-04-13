using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance;

    private Dictionary<string, List<DoorController>> doorsByID = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterDoor(DoorController door)
    {
        if (!doorsByID.ContainsKey(door.doorID))
            doorsByID[door.doorID] = new List<DoorController>();

        doorsByID[door.doorID].Add(door);
    }

    public void SetDoorState(string id, bool isOpen)
    {
        if (!doorsByID.ContainsKey(id)) return;

        foreach (var door in doorsByID[id])
        {
            door.SetOpen(isOpen);
        }
    }
}
