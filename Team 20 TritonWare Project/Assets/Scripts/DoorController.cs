using UnityEngine;

public class DoorController : MonoBehaviour
{
    public string doorID;
    private Collider2D doorCollider;
    private SpriteRenderer sr;
    private Color doorColor;

    private void Start()
    {
        doorCollider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        doorColor = sr.color; // store the original color
        DoorManager.Instance.RegisterDoor(this);
    }

    public void SetOpen(bool isOpen)
    {
        doorCollider.enabled = !isOpen;
        sr.color = isOpen ? new Color(doorColor.r, doorColor.g, doorColor.b, 0.3f) : doorColor;
    } 
}