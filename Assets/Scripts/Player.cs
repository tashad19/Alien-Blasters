using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    float _jumpEndTime;
    [SerializeField] float _horizontalVelocity = 3f;
    [SerializeField] float _jumpVelocity = 5f;
    [SerializeField] float _jumpDuration = 0.5f;
    [SerializeField] Sprite _jumpSprite;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _footOffset = 0.5f;

    // CapitalCase for public global variables
    public bool IsGrounded;   

    // start with underscore for private global variables
    SpriteRenderer _spriteRenderer;
    float _horizontal;
    Animator _animator;
    int _jumpsRemaining;
    AudioSource _audioSource;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void OnDrawGizmos()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Gizmos.color = Color.red;
        // Debug.Log(spriteRenderer.bounds.extents.y);

        Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        // Draw left foot
        origin = new Vector2(transform.position.x - _footOffset, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        // Draw right foot
        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Updated at " + Time.time);  // get time elapsed since game started

        UpdateGrounding();

        _horizontal = Input.GetAxis("Horizontal");
        // Debug.Log(_horizontal);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        var vertical = rb.linearVelocityY;

        if (Input.GetButtonDown("Jump") && _jumpsRemaining > 0)
        {
            _jumpEndTime = Time.time + _jumpDuration;
            _jumpsRemaining--;

            _audioSource.pitch = _jumpsRemaining > 0 ? 1 : 1.2f;

            _audioSource.Play();
        }

        // Keep going up (jumping) if jump button is kept pressed
        if (Input.GetButton("Jump") && _jumpEndTime > Time.time)
            vertical = _jumpVelocity;

        _horizontal *= _horizontalVelocity;
        rb.linearVelocity = new Vector2(_horizontal, vertical);  // In Unity, this is the way to set velocity to a RB, we can't set horizontal and vertical velocity separately
        UpdateSprite();
    }

    void UpdateGrounding()
    {
        IsGrounded = false;

        // check center
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider)
            IsGrounded = true;

        // check left
        origin = new Vector2(transform.position.x - _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider)
            IsGrounded = true;

        // check right
        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider)
            IsGrounded = true;

        if(IsGrounded && GetComponent<Rigidbody2D>().linearVelocityY <= 0)
            _jumpsRemaining = 2;
    }

    void UpdateSprite()
    {
        _animator.SetBool("IsGrounded", IsGrounded);
        _animator.SetFloat("HorizontalSpeed", Math.Abs(_horizontal)); 

        if(_horizontal > 0)
            _spriteRenderer.flipX = false;
        else if(_horizontal < 0)    // reason for using else if (not just else) is so that if _horizontal=0 then it stays in same flipped state as it was
            _spriteRenderer.flipX = true;
    }
}
