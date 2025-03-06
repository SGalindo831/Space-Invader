using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float boundaryLeft = -8f;
    public float boundaryRight = 8f;

    private Rigidbody2D rb;
    private float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position + new Vector2(horizontalInput * moveSpeed * Time.fixedDeltaTime, 0);
        newPosition.x = Mathf.Clamp(newPosition.x, boundaryLeft, boundaryRight);
        rb.MovePosition(newPosition);
    }

    //visualization of boundaries in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 leftBoundary = transform.position;
        leftBoundary.x = boundaryLeft;
        Vector3 rightBoundary = transform.position;
        rightBoundary.x = boundaryRight;
        
        Gizmos.DrawLine(leftBoundary + Vector3.up, leftBoundary + Vector3.down);
        Gizmos.DrawLine(rightBoundary + Vector3.up, rightBoundary + Vector3.down);
    }
}