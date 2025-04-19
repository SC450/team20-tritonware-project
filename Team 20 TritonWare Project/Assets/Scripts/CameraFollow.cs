using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 3.3f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    private float yAnchor;

    // Border offsets
    private float startingBorderOffset = 7f;

    [SerializeField] private Transform target;

    // Update is called once per frame
    void Update()
    {
        if (target.position.x > startingBorderOffset * 0.35) {
            //Vector3 targetPosition = target.position + offset;
            Vector3 targetPosition = new Vector3(target.position.x, yAnchor, target.position.z) + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    // Initialize position for camera
    void Start()
    {
        yAnchor = target.transform.position.y;
        transform.position = new Vector3(target.position.x + startingBorderOffset, yAnchor, target.position.z) + offset;
    }
}
