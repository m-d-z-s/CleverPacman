using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    public AnimatedSprite deathSequence;
    public SpriteRenderer spriteRenderer { get; private set; }
    public new Collider2D collider { get; private set; }
    public Movement movement { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        // Set the new direction based on the current input
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            movement.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            movement.SetDirection(Vector2.right);
        }

        // Rotate pacman to face the movement direction
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
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
    
    
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     Node node = other.GetComponent<Node>();

    //     // Do nothing while the ghost is frightened
    //     if (node != null && enabled && !ghost.frightened.enabled && node.availableDirections.Count > 0)
    //     {
    //         // Pick a random available direction
    //         int index =  Random.Range(0, node.availableDirections.Count);

    //         // Prefer not to go back the same direction so increment the index to
    //         // the next available direction
    //         if (node.availableDirections.Count > 1 && node.availableDirections[index] == ghost.movement.direction)
    //         {
    //             index++;
    //             //index %= node.availableDirections.Count;
    //             // Wrap the index back around if overflowed
    //             if (index >= node.availableDirections.Count) {
    //                 index = 0;
    //             }
    //         }

    //         ghost.movement.SetDirection(node.availableDirections[index]);
    //     }
    // }


}