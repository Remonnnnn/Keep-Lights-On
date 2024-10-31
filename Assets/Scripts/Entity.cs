
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Compnents
    public Animator anim { get; set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }

    public CapsuleCollider2D cd { get; private set; }

    #endregion

    public int facingDir = 1;
    public bool facingRight = true;

    public System.Action onFlipped;
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();

    }

    protected virtual void LateUpdate()
    {

    }
    protected virtual void Update()
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }


    #region Velocity
    public void SetZeroVelocity()
    {
        rb.velocity = new Vector2(0, 0);
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null)
        {
            onFlipped();
        }
    }
    
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }

    public virtual void SetUpfaultFacingDir(int _direction)
    {
        facingDir = _direction;

        if (facingDir == -1)
        {
            facingRight = false;
        }
    }

    #endregion

    public virtual void Die()
    {

    }
}
