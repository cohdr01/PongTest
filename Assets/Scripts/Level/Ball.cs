using System;
using System.Reflection.Emit;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    #region Editor exposed members
    [SerializeField] private float _minVelocity = 5;
    #endregion

    private Rigidbody2D _rigidBody2D;

    #region Events
    public event Action<EndZone.EndZoneType> EnteredEndZone;
    #endregion

    /// <summary>
    /// Gives the ball a completely random velocity (50%/50% left/right + 50%/50% up/down) with the minimum velocity
    /// </summary>
    public void GiveRandomVelocity()
    {
        // TODO: Give our rigidbody a random velocity, 50%/50% left/right + 50%/50% up/down - must be at least at _minVelocity
        float rand = Random.Range(0, 2);
        float rndVelocity = Random.Range(_minVelocity, 30);
        if (rand < 1)
        {
            _rigidBody2D.AddForce(new Vector2(rndVelocity, -15));
        }
        else
        {
            _rigidBody2D.AddForce(new Vector2(-rndVelocity, -15));
        }
    }

    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        if (_rigidBody2D == null)
        {
            Debug.LogError("_rigidBody2D not found!");
            Application.Quit();
            return;
        }

        Invoke("GiveRandomVelocity", 2); 
    }

    /// <summary>
    /// 
    /// </summary>
    void ResetBall()
    {
        _rigidBody2D.velocity = new Vector2(0, 0);
        transform.position = Vector2.zero;
    }


    /// <summary>
    /// Resets the ball (position and velocity)
    /// </summary>
    public void Reset()  //RestartGame
    {
        // TODO: Reset our ball's position and velocity
        ResetBall();
        Invoke("GiveRandomVelocity", 1);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Player"))
        {
            UnityEngine.Vector2 vel;
            vel.x = _rigidBody2D.velocity.x;
            vel.y = (_rigidBody2D.velocity.y / 2.0f) + (coll.collider.attachedRigidbody.velocity.y / 3.0f);
            _rigidBody2D.velocity = vel;
        }

        if (_rigidBody2D.velocity.magnitude < _minVelocity)
        {
            float ratio = _minVelocity / _rigidBody2D.velocity.magnitude;
            _rigidBody2D.velocity = _rigidBody2D.velocity * ratio;
        }

    }



    private void OnCollisionEnter(Collision collision)
    {
        // Make sure if the ball lost velocity that we're never below the minimum
        //if (GetComponent<Rigidbody2D>().velocity.magnitude < _minVelocity)
        //{
        //    float ratio = _minVelocity / GetComponent<Rigidbody2D>().velocity.magnitude;
        //    GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * ratio;
        //}
        

        // DROR: All colisions are 2D
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        // TODO: Handle trigger collisions for endzones
        // TODO: Make sure we collided with an endzone
        // TODO: Raise the EnteredEndZone event if we did

        //GetComponent<Rigidbody2D>().velocity = Vector2.zero; //Vector3.zero;
        //_rigidBody2D.velocity = Vector2.zero;
    }
}