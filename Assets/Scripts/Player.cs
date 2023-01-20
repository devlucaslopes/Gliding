using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float GlideGravityScale;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private bool _onGround;
    private bool _isJumping;
    private bool _isGliding;

    private float _defaultGravityScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        _defaultGravityScale = rb.gravityScale;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            _isGliding = true;
            _isJumping = false;
            anim.SetInteger("transition", 3);

            rb.gravityScale = GlideGravityScale;
        }

        if (Input.GetButtonUp("Fire3"))
        {
            _isGliding = false;
            rb.gravityScale = _defaultGravityScale;
        }


        if (Input.GetButtonDown("Jump") && _onGround)
        {
            _onGround = false;
            _isJumping = true;

            anim.SetInteger("transition", 2);
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalAxis * Speed, rb.velocity.y);

        if (horizontalAxis == 1)
        {
            sprite.flipX = false;                
        } else if (horizontalAxis == -1)
        {
            sprite.flipX = true;
        }

        if (!_isJumping && !_isGliding)
        {
            if (horizontalAxis != 0)
            {
                anim.SetInteger("transition", 1);
            }
            else
            {
                anim.SetInteger("transition", 0);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            _onGround = true;
            _isJumping = false;
            _isGliding = false;
        }
            
    }
}
