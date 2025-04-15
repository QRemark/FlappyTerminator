using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private readonly Enemy enemy;
    private readonly BulletSpawner spawner;
    private readonly float attackRange;
    private readonly float attackRate;
    private readonly SpriteRenderer spriteRenderer;
    private readonly Sprite defaultSprite;
    private readonly Sprite attackSprite;

    private Transform player;
    private AttackAudio audio;

    public EnemyAttack(Enemy enemy, BulletSpawner spawner, float attackRange, float attackRate, SpriteRenderer spriteRenderer, Sprite defaultSprite, Sprite attackSprite)
    {
        this.enemy = enemy;
        this.spawner = spawner;
        this.attackRange = attackRange;
        this.attackRate = attackRate;
        this.spriteRenderer = spriteRenderer;
        this.defaultSprite = defaultSprite;
        this.attackSprite = attackSprite;

        this.audio = enemy.GetComponent<AttackAudio>();
    }

    public void Initialize(Transform player, Transform bulletPoolParent)
    {
        this.player = player;
        spawner?.SetPoolParent(bulletPoolParent);
    }

    public void StartAttacking()
    {
        enemy.InvokeRepeating(nameof(Fire), attackRate, attackRate);
    }

    public void StopAttacking()
    {
        enemy.CancelInvoke(nameof(Fire));
    }

    public void Fire()
    {
        if (player == null || spawner == null)
        {
            Debug.LogWarning("Player или Spawner не назначен!");
            return;
        }

        float distance = Vector2.Distance(enemy.transform.position, player.position);
        Debug.Log($"Distance to player: {distance}");

        if (distance < attackRange)
        {
            Debug.Log("Враг стреляет!");
            spriteRenderer.sprite = attackSprite;
            audio?.AttackSound();
            enemy.Invoke(nameof(ResetSprite), 0.5f);

            Bullet bullet = spawner.Fire(enemy.transform.position, false);
            bullet?.SetOwner(enemy.transform);
        }
    }


    private void ResetSprite()
    {
        spriteRenderer.sprite = defaultSprite;
    }
}
