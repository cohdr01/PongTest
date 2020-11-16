using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Player identifier enum
    /// </summary>
    public enum PlayerType
    {
        Left,
        Right
    }

    #region Editor exposed members
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    [SerializeField] private PlayerType _playerType;
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] public float _boundY = 2.25f;
    #endregion

    #region Private members
    private Transform _transform;
    private float _halfHeight;
    private Rigidbody2D _rigidBody2D;

    #endregion

    private void Start()
    {
        // Store highly used variables in advance for performance
        _transform = transform;
        //_halfHeight = GetComponent<Collider>().bounds.extents.y;
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // TODO: Get movement input (Make sure left/right player)
        // TODO: Move player
        // TODO: Make sure player doesn't leave screen bounds (ScreenUtil.ScreenPhysicalBounds will help you out)

        var vel = _rigidBody2D.velocity;
        if (Input.GetKey(moveUp))
        {
            vel.y = _movementSpeed;
        }
        else if (Input.GetKey(moveDown))
        {
            vel.y = -_movementSpeed;
        }
        else if (!Input.anyKey)
        {
            vel.y = 0;
        }
        _rigidBody2D.velocity = vel;

        var pos = transform.position;
        if (pos.y > _boundY)
        {
            pos.y = _boundY;
        }
        else if (pos.y < -_boundY)
        {
            pos.y = -_boundY;
        }
        transform.position = pos;


    }
}