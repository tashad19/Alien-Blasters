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
    public bool IsGrounded;   // CapitalCase for global variables
    Sprite _defaultSprite;
    SpriteRenderer _spriteRenderer;
    float _horizontal;
    Animator _animator;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite;
        _animator = GetComponent<Animator>();
    }

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

        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
        var hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);

        if (hit.collider)
            IsGrounded = true;
        else
            IsGrounded = false;

        _horizontal = Input.GetAxis("Horizontal");
        // Debug.Log(_horizontal);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        var vertical = rb.linearVelocityY;

        if (Input.GetButtonDown("Jump") && IsGrounded)
            _jumpEndTime = Time.time + _jumpDuration;

        if (Input.GetButton("Jump") && _jumpEndTime > Time.time)
            vertical = _jumpVelocity;

        _horizontal *= _horizontalVelocity;
        rb.linearVelocity = new Vector2(_horizontal, vertical);  // In Unity, this is the way to set velocity to a RB, we can't set horizontal and vertical velocity separately
        UpdateSprite();
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
