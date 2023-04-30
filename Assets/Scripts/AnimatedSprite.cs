using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite[] sprites;
    public float animationTime = 0.125f;
    public int AnimationFrame { get; private set; }
    public bool loop = true;

    public void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        InvokeRepeating(nameof(Advance), this.animationTime, this.animationTime);
    }

    private void Advance()
    { 
        if (!this.spriteRenderer.enabled)
        {
            return;
        }
        this.AnimationFrame++;
        if (this.AnimationFrame >= this.sprites.Length && this.loop) {
            this.AnimationFrame = 0;
        }
        if (this.AnimationFrame >= 0 && (this.AnimationFrame < sprites.Length)) {
            this.spriteRenderer.sprite = this.sprites[this.AnimationFrame];
        }
    }
    public void Restart()
    {
        this.AnimationFrame = -1;
        Advance();
    }
}
