using UnityEngine;
using System.Collections.Generic;

public class LaserEmitter : MonoBehaviour 
{
    public float maxDistance = 100f;
    public LayerMask hitLayers;
    public int maxReflections = 5;
    
    private LineRenderer lineRenderer;
    
    void Start() 
    {
        // Add a LineRenderer component if one doesn't exist
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            
        // Basic line renderer setup
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.positionCount = 2;
    }
    
    void Update() 
    {
        CastLaser();
    }
    
    void CastLaser() 
    {
        // List to store all positions in our laser path
        List<Vector2> laserPositions = new List<Vector2>();
        
        // Starting position and direction
        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y + 0.26f);
        Vector2 direction = transform.up;
        
        // Add starting position
        laserPositions.Add(startPosition);
        
        int reflectionCount = 0;
        
        // Continue casting rays for each reflection
        while (reflectionCount <= maxReflections)
        {
            RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, maxDistance, hitLayers);
            
            if (hit.collider != null)
            {
                // Add the hit point to our laser positions
                laserPositions.Add(hit.point);
                
                // Check what we hit
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Player detected!");
                    break; // Stop reflecting if we hit the player
                }
                else if (hit.collider.CompareTag("Mirror"))
                {
                    Debug.Log("Mirror detected at reflection #" + reflectionCount);
                    
                    // Calculate the reflection direction correctly
                    direction = Vector2.Reflect(direction, hit.normal);
                    
                    // Update our start position to continue from the hit point
                    // Add a tiny offset in the reflection direction to avoid hitting the same point
                    startPosition = hit.point + direction * 0.01f;
                    
                    reflectionCount++;
                }
                else
                {
                    // Hit something that doesn't reflect
                    break;
                }
            }
            else
            {
                // Nothing was hit, add the end point at max distance
                laserPositions.Add(startPosition + direction * maxDistance);
                break;
            }
        }
        
        // Update the line renderer with all the points
        lineRenderer.positionCount = laserPositions.Count;
        for (int i = 0; i < laserPositions.Count; i++)
        {
            lineRenderer.SetPosition(i, laserPositions[i]);
        }
    }
}
