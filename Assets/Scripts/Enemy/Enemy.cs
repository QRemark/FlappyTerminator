using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour, IDisappearable
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float moveRange = 1.5f;
    [SerializeField] private float lerpSpeed = 1f;
    [SerializeField] private float approachDuration = 1f;
    [SerializeField] private float followMinDistance = 15f;
    [SerializeField] private float followMaxOffsetY = 7f;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 15f;
    [SerializeField] private float attackRate = 3f;

    [Header("Visuals")]
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite attackSprite;

    [Header("Dependencies")]
    [SerializeField] private BulletSpawner bulletSpawner;

    private SpriteRenderer spriteRenderer;
    private EnemyMovement movement;
    private EnemyAttack attack;

    public event System.Action<IDisappearable> Disappeared;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = new EnemyMovement(this, speed, moveRange, lerpSpeed, approachDuration, followMinDistance, followMaxOffsetY);
        attack = new EnemyAttack(this, bulletSpawner, attackRange, attackRate, spriteRenderer, defaultSprite, attackSprite);
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = defaultSprite;
        transform.localScale = Vector3.one * 0.35f;
        attack.StartAttacking();
    }

    private void OnDisable()
    {
        attack.StopAttacking();
    }

    private void Start()
    {
        if (attack != null)
            InvokeRepeating(nameof(FireThroughAttack), attackRate, attackRate);
    }

    private void FireThroughAttack()
    {
        attack.Fire();
    }



    private void Update()
    {
        movement.Tick();
    }

    public void Initialize(Transform player, Transform bulletPoolParent)
    {
        movement.Initialize(player);
        attack.Initialize(player, bulletPoolParent);
    }

    public void StartMovementDelay(float delay) => movement.StartDelayedMovement(delay);

    public void Disappear()
    {
        Disappeared?.Invoke(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out _))
        {
            Disappear();
        }
    }
}
