using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

[RequireComponent(typeof(Movement))]
public class PacmanAI : MonoBehaviour
{
    public PacmanAI pacman { get; private set; } // pacman instance
    public AnimatedSprite deathSequence; // animation
    public SpriteRenderer spriteRenderer { get; private set; } // shape
    public new Rigidbody2D rigidbody2D {get; private set;} // physics
    public new Collider2D collider { get; private set; } // collision
    public Movement movement { get; private set; } 
    // public PacmanIsRunning running { get; private set; } // pacman scatter behaviour property
    // public PacmanIsEating eating { get; private set; } // pacman chase behaviour property
    // public PowerPacman powerPacman { get; private set;}
    // public GhostBehavior initialBehavior; // initial behaviour property (the one with wich the ghost starts the game)

    private Vector3 previousPosition; // position in previous frame
    private float timeSinceLastMove; // time idling

    private const int DISTANCE_OF_FEAR = 10; // distance in which ghosts are a threat

    public void Start()
    {
        previousPosition = transform.position;
    }

    private void Awake() 
    {
        pacman = GetComponent<PacmanAI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        movement = GetComponent<Movement>();
        // this.running = GetComponent<PacmanIsRunning>(); // getting the pacman scatter behaviour from the game object
        // this.eating = GetComponent<PacmanIsEating>(); // getting the pacman chase behaviour from the game object
        // this.powerPacman = GetComponent<PowerPacman>();
    }

    private void Update()
    {
        GameObject[] pellets = GameObject.FindGameObjectsWithTag("Pellet");
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        GameObject nearestPellet = null;
        GameObject nearestGhost = null;
        GhostChase ghostChase = null;
        GhostFrightened ghostFrightened = null;
        GhostHome ghostHome = null;
        // for brave behaviour (partially implemented)

        float nearestPelletDistance = Mathf.Infinity;
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
                ghostChase = GetComponent<GhostChase>();
                ghostFrightened = GetComponent<GhostFrightened>();
                // ghostChase = ghost.GetComponent<GhostChase>();
                // ghostFrightened = ghost.GetComponent<GhostFrightened>();
                // ghostHome = ghost.GetComponent<GhostHome>();
            }
        }
        BehaviourStable(nearestGhost, nearestPellet, ghostChase, 
                        ghostFrightened, nearestPelletDistance, nearestGhostDistance);
        
        // BehaviourBrave(nearestGhost, nearestPellet, ghostChase, 
        // ghostFrightened, ghostHome, nearestPelletDistance, nearestGhostDistance);

        
    }

    private void BehaviourStable(GameObject nearestGhost,GameObject nearestPellet, GhostChase ghostChase, GhostFrightened ghostFrightened,
                                float nearestPelletDistance, float nearestGhostDistance) 
    {
        // run away from ghosts
        if (nearestGhost != null && nearestGhostDistance <= nearestPelletDistance && ghostChase != null && ghostChase.enabled && !ghostFrightened.enabled)
        {
            Vector2 oppositeDirection = -(nearestGhost.transform.position - transform.position).normalized;
            oppositeDirection.x = Mathf.RoundToInt(oppositeDirection.x);
            oppositeDirection.y = Mathf.RoundToInt(oppositeDirection.y);
            movement.SetDirection(oppositeDirection);
            Debug.Log("Current direction: " + oppositeDirection + " Distance to coin: " + nearestPelletDistance + " Distance to ghost: " + nearestGhostDistance);    
            ChangeDirectionIfNeeded();
        }
        // eat ghost if ghost closer then pellet
        else if (ghostFrightened != null && ghostFrightened.enabled && nearestGhostDistance <= nearestPelletDistance){
            movement.SetDirection(nearestGhost.transform.position);
        }
        // eat pellet if pellet closer then ghost
        else if (ghostFrightened != null && ghostFrightened.enabled && nearestGhostDistance > nearestPelletDistance){
            Vector2 direction = (nearestPellet.transform.position - transform.position).normalized;
            movement.SetDirection(direction);
        }
        // eat closest pellet
        else if (nearestPellet != null)
        {
            Vector2 direction = (nearestPellet.transform.position - transform.position).normalized;
            direction.x = Mathf.RoundToInt(direction.x);
            direction.y = Mathf.RoundToInt(direction.y);
            movement.SetDirection(direction);
            Debug.Log("Current direction: " + direction + " Distance to coin: " + nearestPelletDistance + " Distance to ghost: " + nearestGhostDistance);
            ChangeDirectionIfNeeded();
        }
        // GIVEN pacman ai WHEN distance_to_nearest_ghost < cosntant_of_danger THEN run from nearest_ghost
    }

    private void BehaviourBrave(GameObject nearestGhost,GameObject nearestPellet, GhostChase ghostChase, GhostFrightened ghostFrightened, GhostHome ghostHome,
                                float nearestPelletDistance, float nearestGhostDistance) 
    {
        // run away from ghosts
        if (nearestGhost != null && nearestGhostDistance <= nearestPelletDistance 
        && !ghostFrightened.enabled && nearestGhostDistance <= DISTANCE_OF_FEAR)
        {
            Vector2 oppositeDirection = -(nearestGhost.transform.position - transform.position).normalized;
            oppositeDirection.x = Mathf.RoundToInt(oppositeDirection.x);
            oppositeDirection.y = Mathf.RoundToInt(oppositeDirection.y);
            movement.SetDirection(oppositeDirection);
            Debug.Log("run away from ghosts");
            //Debug.Log("Current direction: " + oppositeDirection + " Distance to coin: " + nearestPelletDistance + " Distance to ghost: " + nearestGhostDistance);    
            ChangeDirectionIfNeeded();
        }
        // eat ghost if ghost closer then pellet
        else if (ghostFrightened != null && ghostFrightened.enabled && nearestGhostDistance <= nearestPelletDistance && !ghostHome.enabled){
            Debug.Log("F eat ghost");
            movement.SetDirection(nearestGhost.transform.position);
            ChangeDirectionIfNeeded();

        }
        // eat pellet if pellet closer then ghost
        else if (ghostFrightened != null && ghostFrightened.enabled && nearestGhostDistance > nearestPelletDistance){
            Debug.Log("F eat pellets");
            Vector2 direction = (nearestPellet.transform.position - transform.position).normalized;
            movement.SetDirection(direction);
            ChangeDirectionIfNeeded();

        }
        // eat closest pellet
        else if (nearestPellet != null && ghostFrightened != null && nearestGhostDistance > DISTANCE_OF_FEAR  && !ghostFrightened.enabled)
        {
            Vector2 direction = (nearestPellet.transform.position - transform.position).normalized;
            direction.x = Mathf.RoundToInt(direction.x);
            direction.y = Mathf.RoundToInt(direction.y);
            movement.SetDirection(direction);
            Debug.Log("just eat pellets");
            //Debug.Log("Current direction: " + direction + " Distance to coin: " + nearestPelletDistance + " Distance to ghost: " + nearestGhostDistance);
            ChangeDirectionIfNeeded();
        }
        // GIVEN pacman ai WHEN distance_to_nearest_ghost < cosntant_of_danger THEN run from nearest_ghost
    }


    private void ChangeDirectionIfNeeded()
    {
        if (CheckIfObjectDontMove()) movement.RotateDirection();
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


   public void DeathSequence()
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

    //     private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     // Reverse direction everytime the ghost hits a wall to create the
    //     // effect of the ghost bouncing around the home
    //     if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
    //         pacman.movement.SetDirection(-pacman.movement.direction);
    //     }
    // }

}
