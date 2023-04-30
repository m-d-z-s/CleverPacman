using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Movement))]
public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; } // movevent property
    public GhostHome home { get; private set; } // ghost home behaviour property
    public GhostScatter scatter { get; private set; } // ghost scatter behaviour property
    public GhostChase chase { get; private set; } // ghost chase behaviour property
    public GhostFrightened frightened { get; private set; } // ghost frightened behaviour property
    public GhostBehavior initialBehavior; // initial behaviour property (the one with wich the ghost starts the game)
    public Transform target; // target property (target is an object with wich the behaviour is associated)
    public int points = 200; // points property (points that the player gets when he eats the ghost)

    private void Awake()
    {
        this.movement = GetComponent<Movement>(); // getting the movement component from the game object
        this.home = GetComponent<GhostHome>(); // getting the ghost home behaviour from the game object
        this.scatter = GetComponent<GhostScatter>(); // getting the ghost scatter behaviour from the game object
        this.chase = GetComponent<GhostChase>(); // getting the ghost chase behaviour from the game object
        this.frightened = GetComponent<GhostFrightened>(); // getting the ghost frightened behaviour from the game object
    }

    private void Start()
    {
        movement = GetComponent<Movement>();
        ResetState();
    }

    public void ResetState()
    {
        gameObject.SetActive(true); // activating the game object
        movement.ResetState(); // resetting the movement state

        frightened.Disable(); // disabling the frightened behaviour because ghost never starts with frightened behaviour
        chase.Disable(); // disabling the chase behaviour because ghost never starts with chase behaviour
        scatter.Enable(); // enabling the scatter behaviour because ghost always starts with scatter behaviour

        if (home != initialBehavior) this.home.Disable(); // if ghost is not home then disable the home behaviour
        if (initialBehavior != null) this.initialBehavior.Enable(); // if ghost has an initial behaviour then enable it
        
    }

    public void SetPosition(Vector3 position)
    {
        // Keep the z-position the same since it determines draw depth
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman")) // if the collision is with pacman
        {
            if (frightened.enabled) FindObjectOfType<GameManager>().GhostEaten(this); // if ghost is frightened then pacman eats the ghost
			else FindObjectOfType<GameManager>().PacmanEaten(); // if ghost is not frightened then pacman is eaten
        }
    }
}
