using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PacmanAI : MonoBehaviour
{
    public PacmanAI pacman { get; private set; }
    public float cooldown = 0.5f;
    public AnimatedSprite deathSequence; //keep
    public SpriteRenderer spriteRenderer { get; private set; } //keep
    public new Collider2D collider { get; private set; } //keep maybe
    public Movement movement { get; private set; } //keep

    private void Awake()//keep
    {
        pacman = GetComponent<PacmanAI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        cooldown -= Time.deltaTime;
        GameObject[] pellets = GameObject.FindGameObjectsWithTag("Pellet");
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        GameObject nearestPellet = null;
        GameObject nearestGhost = null;
        float nearestPelletDistance = Mathf.Infinity;
        float nearestPowerPelletDistance = Mathf.Infinity;
        float nearestGhostDistance = Mathf.Infinity;

        foreach (GameObject pellet in pellets)
        {
            float distance = Vector2.Distance(transform.position, pellet.transform.position);
            if (distance < nearestPelletDistance)
            {
                nearestPelletDistance = distance;
                nearestPellet = pellet;
            }
        }
        foreach (GameObject ghost in ghosts)
        {
            float distance = Vector2.Distance(transform.position, ghost.transform.position);
            if (distance < nearestGhostDistance)
            {
                nearestGhostDistance = distance;
                nearestGhost = ghost;
            }
        }

        if (nearestGhost != null && nearestGhostDistance < nearestPelletDistance && cooldown <= 0)
        {
            Vector2 oppositeDirection = -(nearestGhost.transform.position - transform.position).normalized;
            movement.SetDirection(oppositeDirection);
            cooldown = 0.5f;
            Debug.Log("Runs away from the ghost!");
        }
        else if (nearestPellet != null)
        {
            Vector2 direction = (nearestPellet.transform.position - transform.position).normalized;
            movement.SetDirection(direction);
            // Debug.Log("Goes to the coin!");
        }
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

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Node"))
    //     {
    //         currentNode = other.gameObject.GetComponent<Node>();
    //         currentDirection = movement.direction;
    //     }
    // }
    
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     Node node = other.GetComponent<Node>();

    //     // Do nothing while the pacman is frightened
    //     if (node != null && enabled && node.availableDirections.Count > 0)
    //     {
    //         // Check if Pacman can move in any of the available directions
    //         List<Vector2> availableDirections = new List<Vector2>();
    //         foreach (Vector2 direction in node.availableDirections)
    //         {
    //             if (movement.CanMoveInDirection(direction))
    //             {
    //                 availableDirections.Add(direction);
    //             }
    //         }

    //         // If Pacman can move in any of the available directions, pick a random one
    //         if (availableDirections.Count > 0)
    //         {
    //             int index = Random.Range(0, availableDirections.Count);
    //             movement.SetDirection(availableDirections[index]);
    //         }
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // Do nothing while the ghost is frightened
        if (node != null && enabled && node.availableDirections.Count > 0)
        {
            // Pick a random available direction
            int index = Random.Range(0, node.availableDirections.Count);

            // Prefer not to go back the same direction so increment the index to
            // the next available direction
            if (node.availableDirections.Count > 1 && node.availableDirections[index] == pacman.movement.direction)
            {
                index++;
                //index %= node.availableDirections.Count;
                // Wrap the index back around if overflowed
                if (index >= node.availableDirections.Count) {
                    index = 0;
                }
            }

            pacman.movement.SetDirection(node.availableDirections[index]);
        }
    }


}
