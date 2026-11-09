using System.Collections;
using System.Threading.Tasks.Dataflow;
using UnityEngine;
using UnityEngine.AI;


public enum RoomZone
    {
        Bedroom1,
        Bedroom2,
        Bathroom1,
        Bathroom2,
        LivingRoom,
        Kitchen,
        DiningRoom
    }

[System.Serializable]
public class RoomWalkPoints
{
    public RoomZone room;
    public Transform[] points;
}
public class MonsterMovement : MonoBehaviour
{
    // Define new object types MonsterState and RoomZone
    public enum MonsterState
    {
        Idle,
        Walk,
        Sprint,
        Suspicious,
        Chase
    }

    // Define Speeds
    public float walkSpeed = 2f;
    public float sprintSpeed = 5f;
    public float suspiciousSpeed = 1.5f;
    public float chaseSpeed = 6f;

    // Idle wait time
    public float minIdleTime = 2f;
    public float maxIdleTime = 6f;

    // How long the monster pauses before choosing its next movement.
    public float pauseDuration = 2f;

    // Suspisious boundaries
    public float suspiciousDistance = 8f;
    public float suspiciousDuration = 5f;

    // Player detection
    public float detectionDistance = 12f;

    public Transform bedroom1;
    public Transform bedroom2;
    public Transform bathroom1;
    public Transform bathroom2;
    public Transform kitchen;
    public Transform livingroom;
    public Transform diningroom;

    private Transform target;


    // Room zone vectors
    Array rooms = [bedroom1, bedroom2, bathroom1, bathroom2, kitchen, livingroom, diningroom];

    public MonsterState monsterState;

    private NavMeshAgent agent;
    private Coroutine pauseCoroutine;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterState = MonsterState.Idle;
    }
   

    void Update()
    {
        // Do not select another state while the monster is paused.
        if (pauseCoroutine != null)
        {
            return;
        }
        monsterState = ChooseNextState();

        switch (monsterState)
        {
            case MonsterState.Idle:
                HandleIdle();
                break;

            case MonsterState.Walk:
                HandleWalk();
                break;

            case MonsterState.Sprint:
                HandleSprint();
                break;

            case MonsterState.Suspicious:
                HandleSuspicious();
                break;

            case MonsterState.Chase:
                HandleChase();
                break;
        }
    }

    private MonsterState ChooseNextState()
    {
        float roll = Random.value;

        if (roll < 0.3f)
        {
            return MonsterState.Idle;
        }

        return MonsterState.Walk;
    }

    void HandleIdle()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        pauseCoroutine = StartCoroutine(PauseMovement(pauseDuration));
    }

    void HandleWalk()
    {
        agent.isStopped = false;
        agent.speed = walkSpeed;
        target = rooms[Random.Range(0, rooms.Length)];
        agent.SetDestination(target.position);
    }

    void HandleSprint()
    {
        agent.isStopped = false;
        agent.speed = sprintSpeed;
    }

    void HandleSuspicious()
    {
        agent.isStopped = false;
        agent.speed = suspiciousSpeed;
    }

    void HandleChase()
    {
        agent.isStopped = false;
        agent.speed = chaseSpeed;
    }

    public IEnumerator PauseMovement(float duration = 2f)
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        yield return new WaitForSeconds(duration);

        pauseCoroutine = null;

    }
}
