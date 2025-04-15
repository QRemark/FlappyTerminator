using UnityEngine;

public class EnemyMovement
{
    private readonly Enemy enemy;
    private readonly float speed, moveRange, lerpSpeed, approachDuration, followMinDistance, followMaxOffsetY;

    private Transform player;
    private float timeOffset;
    private float followDistance;
    private float offsetY;
    private Vector3 smoothPosition;
    private float moveTimer;

    private float minY, maxY;
    private bool canFollow;
    private bool isApproaching;
    private Vector3 approachStart;
    private float approachTime;

    public EnemyMovement(Enemy enemy, float speed, float moveRange, float lerpSpeed, float approachDuration, float followMinDistance, float followMaxOffsetY)
    {
        this.enemy = enemy;
        this.speed = speed;
        this.moveRange = moveRange;
        this.lerpSpeed = lerpSpeed;
        this.approachDuration = approachDuration;
        this.followMinDistance = followMinDistance;
        this.followMaxOffsetY = followMaxOffsetY;

        timeOffset = Random.Range(0f, 10f);
    }

    public void Initialize(Transform player)
    {
        this.player = player;

        var cam = Camera.main;
        float camY = cam.transform.position.y;
        float camHeight = cam.orthographicSize;

        minY = camY - camHeight;
        maxY = camY + camHeight - 1f;

        smoothPosition = enemy.transform.position;
    }

    public void StartDelayedMovement(float delay)
    {
        followDistance = Random.Range(6f, 13f);
        offsetY = Random.Range(-4f, 4f);
        approachStart = enemy.transform.position;
        approachTime = 0;
        isApproaching = true;
    }

    public void Tick()
    {
        if (player == null) return;

        if (isApproaching)
        {
            approachTime += Time.deltaTime;
            float t = Mathf.Clamp01(approachTime / approachDuration);

            Vector3 target = player.position + new Vector3(followDistance, offsetY, 0);
            enemy.transform.position = Vector3.Lerp(approachStart, target, t);

            if (t >= 1f)
            {
                isApproaching = false;
                canFollow = true;
                smoothPosition = enemy.transform.position;
            }

            return;
        }

        if (canFollow)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer >= 3f)
            {
                followDistance = Random.Range(followMinDistance, followMinDistance + 10f);
                offsetY = Random.Range(-5f, followMaxOffsetY);
                moveTimer = 0;
            }

            float waveY = Mathf.Sin((Time.time + timeOffset) * speed) * moveRange;
            float targetY = Mathf.Clamp(player.position.y + waveY + offsetY, minY, maxY);
            Vector3 targetPos = new Vector3(player.position.x + followDistance, targetY, 0);

            smoothPosition = Vector3.Lerp(smoothPosition, targetPos, Time.deltaTime * lerpSpeed);
            enemy.transform.position = smoothPosition;
        }
    }
}
