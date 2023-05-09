// using UnityEngine;

// [RequireComponent(typeof(PacmanAI))]
// public abstract class PacmanAIBehavior : MonoBehaviour
// {
//     public PacmanAI pacman { get; private set; }
//     //public float duration;

//     private void Awake()
//     {
//         this.pacman = GetComponent<PacmanAI>(); // getting the pacman component from the game object 
// 		this.enabled = false; // disabling this behaviour
//     }

//     public void Enable()
//     {
// 		CancelInvoke(); // cancel any previous invokes
//         Invoke(nameof(Disable)); // disable this behaviour after duration
//     }


//     public virtual void Disable()
//     {
//         this.enabled = false; // disabling this behaviour
//         CancelInvoke(); // cancel any previous invokes
//     }
// }