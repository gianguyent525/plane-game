using UnityEngine;

public class BossMove : MonoBehaviour
{
    public float speed = 2f;
    public float leftLimit = -4f;
    public float rightLimit = 4f;

    private float targetX;

    void Start()
    {
        PickNewTarget();
    }

    void Update()
    {
        Vector3 target = new Vector3(targetX, transform.position.y, 0);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        targetX = Random.Range(leftLimit, rightLimit);
    }
}
