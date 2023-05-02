using UnityEngine;

public class GhostChase : GhostBehavior
{
    private void OnDisable()
    {
        ghost.scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // Do nothing while the ghost is frightened
        if (node != null && enabled && !ghost.frightened.enabled && node.availableDirections.Count > 0)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            foreach (Vector2 availableDirections in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(availableDirections.x, availableDirections.y, 0.0f);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance < minDistance) {
                    direction = availableDirections;
                    minDistance = distance;
                }
            }
            ghost.movement.SetDirection(direction);
        }
    }
}
