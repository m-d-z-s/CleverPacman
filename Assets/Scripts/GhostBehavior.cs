using UnityEngine;

[RequireComponent(typeof(Ghost))]
public abstract class GhostBehavior : MonoBehaviour
{
    public Ghost ghost { get; private set; }
    public float duration;

    private void Awake()
    {
        this.ghost = GetComponent<Ghost>(); // getting the ghost component from the game object 
		this.enabled = false; // disabling this behaviour
    }

    public void Enable()
    {
        Enable(this.duration); // enabling this behaviour with the duration
    }

    public virtual void Enable(float duration)
    {
        this.enabled = true; // enabling this behaviour
        
		CancelInvoke(); // cancel any previous invokes
        Invoke(nameof(Disable), duration); // disable this behaviour after duration
    }

    public virtual void Disable()
    {
        this.enabled = false; // disabling this behaviour
        CancelInvoke(); // cancel any previous invokes
    }
}