using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


[RequireComponent(typeof(Movement))]
public class PacmanAI : MonoBehaviour
{
    public PacmanAI pacman { get; private set; }
    public float cooldown = 0.5f;
    public AnimatedSprite deathSequence; //keep
    public SpriteRenderer spriteRenderer { get; private set; } //keep
    public new Rigidbody2D rigidbody2D {get; private set;}
    public new Collider2D collider { get; private set; } //keep maybe
    public Movement movement { get; private set; } //keep
    public Transform target; // target property (target is an object with wich the behaviour is associated)

    private Vector3 previousPosition;
    private float timeSinceLastMove;

    public void Start()
    {
        // store the initial position of the object
        previousPosition = transform.position;
    }

    private void Awake()//keep
    {
        pacman = GetComponent<PacmanAI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        GameObject[] pellets = GameObject.FindGameObjectsWithTag("Pellet");
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        //List<float> distances = new List<float>();
        GameObject nearestPellet = null;
        GameObject nearestGhost = null;
        float nearestPelletDistance = Mathf.Infinity;
        float nearestGhostDistance = Mathf.Infinity;

        foreach (GameObject pellet in pellets)
        {
            float distance = Vector2.Distance(transform.position, pellet.transform.position);
            //distances.Add(distance);
            if (distance < nearestPelletDistance)
            {
                nearestPelletDistance = distance;
                nearestPellet = pellet;
            }
        }

        //distances.Sort();

        foreach (GameObject ghost in ghosts)
        {
            float distance = Vector2.Distance(transform.position, ghost.transform.position);
            if (distance < nearestGhostDistance)
            {
                nearestGhostDistance = distance;
                nearestGhost = ghost;
            }
        }

        if (nearestGhost != null && nearestGhostDistance <= nearestPelletDistance)
        {
            Vector2 oppositeDirection = -(nearestGhost.transform.position - transform.position).normalized;
            oppositeDirection.x = Mathf.RoundToInt(oppositeDirection.x);
            oppositeDirection.y = Mathf.RoundToInt(oppositeDirection.y);
            movement.SetDirection(oppositeDirection);
            Debug.Log("Current direction: " + oppositeDirection + " Distance to coin: " + nearestPelletDistance + " Distance to ghost: " + nearestGhostDistance);    
            ChangeDirectionIfNeeded();
        }
        else if (nearestPellet != null)
        {
            Vector2 direction = (nearestPellet.transform.position - transform.position).normalized;
            direction.x = Mathf.RoundToInt(direction.x);
            direction.y = Mathf.RoundToInt(direction.y);
            movement.SetDirection(direction);
            Debug.Log("Current direction: " + direction + " Distance to coin: " + nearestPelletDistance + " Distance to ghost: " + nearestGhostDistance);
            ChangeDirectionIfNeeded();
        }
    }

    private void ChangeDirectionIfNeeded()
    {
        if (CheckIfObjectDontMove())  movement.ReverseDirection();
    }
    
    public void ResetState()
     {
        gameObject.SetActive(true);
        movement.ResetState();
        
        enabled = true;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        deathSequence.enabled = false;
        deathSequence.spriteRenderer.enabled = false;
        movement.ResetState();
        gameObject.SetActive(true);
    }


   public void DeathSequence() //keep
    {
        enabled = false;
        spriteRenderer.enabled = false;
        collider.enabled = false;
        movement.enabled = false;
        deathSequence.enabled = true;
        deathSequence.spriteRenderer.enabled = true;
        deathSequence.Restart();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();
        if (node != null && enabled /*&& node.availableDirections.Count > 0*/)
        {
            Debug.Log("Node is not null and pacman is enabled!");
            int index = Random.Range(0, node.availableDirections.Count);
            pacman.movement.SetDirection(node.availableDirections[index]);
        }
    }
    
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     Node node = other.GetComponent<Node>();

    //     // Do nothing while the ghost is frightened
    //     if (node != null && enabled && node.availableDirections.Count > 0)
    //     {
    //         // Pick a random available direction
    //         int index = Random.Range(0, node.availableDirections.Count);

    //         // Prefer not to go back the same direction so increment the index to
    //         // the next available direction
    //         if (node.availableDirections.Count > 1 && node.availableDirections[index] == pacman.movement.direction)
    //         {
    //             index++;
    //             //index %= node.availableDirections.Count;
    //             // Wrap the index back around if overflowed
    //             if (index >= node.availableDirections.Count) {
    //                 index = 0;
    //             }
    //         }

    //         pacman.movement.SetDirection(node.availableDirections[index]);
    //     }
    // }


    private bool CheckIfObjectDontMove()
    {
        bool dontMove = false;
        // check if the object has moved
        if (transform.position != previousPosition)
        {
            // reset the timer if the object has moved
            timeSinceLastMove = 0f;
            previousPosition = transform.position;
        }
        else
        {
            // increment the timer if the object has not moved
            timeSinceLastMove += Time.deltaTime;
        }

        // check if the object has not moved for 1 second
        if (timeSinceLastMove >= 0.1f)
        {
            // return true if the object has not moved for 1 second
            dontMove = true;
        }

        return dontMove;
    }
}
