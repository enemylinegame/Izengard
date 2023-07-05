using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{

    public float detectionRadius = 10f;
    public Animator animator;
    public string DefenderDead = "DefenderDead";

    private GameObject defender;

    private void Start()
    {
        defender = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, defender.transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            animator.SetBool(DefenderDead, false);
        }
        else
        {
            animator.SetBool(DefenderDead, true);
        }
    }
}
