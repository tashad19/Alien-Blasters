using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _jumpEndTime;
    [SerializeField] private float _horizontalVelocity = 3f;
    [SerializeField] private float _jumpVelocity = 5f;
    [SerializeField] private float _jumpDuration = 0.5f;
    public bool isGrounded;

    void OnDrawGizmos()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        // Debug.Log(spriteRenderer.bounds.extents.y);
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Updated at " + Time.time);  // get time elapsed since game started

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);
        var hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);

        if (hit.collider)
            isGrounded = true;
        else 
            isGrounded = false;
        
        var horizontal = Input.GetAxis("Horizontal");
        // Debug.Log(horizontal);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        var vertical = rb.linearVelocityY;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            _jumpEndTime = Time.time + _jumpDuration;
        }

        if(Input.GetButton("Jump") && _jumpEndTime > Time.time)
        {
            vertical = _jumpVelocity;
        }

        horizontal *= _horizontalVelocity;
        rb.linearVelocity = new Vector2(horizontal, vertical);  // In Unity, this is the way to set velocity to a RB, we can't set horizontal and vertical velocity separately
    }
}
