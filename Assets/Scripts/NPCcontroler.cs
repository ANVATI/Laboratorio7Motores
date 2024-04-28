using System.Collections;
using UnityEngine;

public class NPCcontroller : MonoBehaviour
{
    [SerializeField] private float velocity;
    private Animator NPCAnimation;
    [SerializeField] private Transform[] checkpointsPatrol;
    public GameObject Interaction;
    public GameObject Message;
    private Transform currentPatrolTarget;
    private int patrolIndex = 0;
    private Transform lastPatrolTarget;
    private Vector3 _movementDirection;
    private bool canMove = true;

    void Start()
    {
        NPCAnimation = GetComponentInChildren<Animator>();
        currentPatrolTarget = checkpointsPatrol[patrolIndex];
        transform.position = currentPatrolTarget.position;
        lastPatrolTarget = currentPatrolTarget;
    }

    void Update()
    {
        if (canMove)
        {
            MoveTowardsPatrolPoint();
            UpdateAnimation();
            RotateTowardsPatrolPoint();
        }
    }

    private void MoveTowardsPatrolPoint()
    {
        _movementDirection = (currentPatrolTarget.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, currentPatrolTarget.position, velocity * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentPatrolTarget.position) < 0.1f)
        {
            NextPatrolPoint();
        }
    }

    private void UpdateAnimation()
    {
        if (canMove)
        {
            float movementSpeed = velocity / 2f;
            NPCAnimation.SetFloat("Speed", movementSpeed);
        }
        else
        {
            NPCAnimation.SetFloat("Speed", 0f);
        }
    }

    private void RotateTowardsPatrolPoint()
    {
        Vector3 targetDirection = (currentPatrolTarget.position - transform.position).normalized;
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void NextPatrolPoint()
    {
        lastPatrolTarget = currentPatrolTarget;
        patrolIndex = (patrolIndex + 1) % checkpointsPatrol.Length;
        currentPatrolTarget = checkpointsPatrol[patrolIndex];
    }
    public void Interact()
    {
        Interaction.SetActive(true);
        Message.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Point")
        {
            canMove = false;
            NPCAnimation.SetFloat("Speed", 0f);
            NextPatrolPoint();
            StartCoroutine(ResumeMovementAfterDelay(3f));
        }
        else if (other.tag == "Player")
        {
            canMove = false;
            NPCAnimation.SetFloat("Speed", 0f);
            Message.SetActive(true);
            Vector3 directionToPlayer = (other.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
            StartCoroutine(ResumeMovementAfterDelay(4f));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Message.SetActive(false);
        }
    }

    private IEnumerator ResumeMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true;
        Interaction.SetActive(false);
    }
}
