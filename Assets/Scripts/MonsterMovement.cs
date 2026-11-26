using System.Collections;
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

    // Suspisious boundaries
    public float suspiciousDistance = 8f;
    public float suspiciousDuration = 5f;

    // Player detection
    public float detectionDistance = 12f;

    // room zone area
    public Transform[] bedroom1;
    public Transform[] bedroom2;
    public Transform[] bathroom1;
    public Transform[] bathroom2;
    public Transform[] kitchen;
    public Transform[] livingroom;
    public Transform[] diningroom;

    RoomZone[] rooms = {RoomZone.Bedroom1, RoomZone.Bedroom2, RoomZone.Bathroom1, RoomZone.Bathroom2, RoomZone.Kitchen, RoomZone.LivingRoom, RoomZone.DiningRoom};

    public MonsterState monsterState;

    private NavMeshAgent agent;
    private Coroutine pauseCoroutine;

    private RoomZone roomState;

    private Animator animator;

    private bool hasDestination = false;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterState = MonsterState.Walk;
        roomState = RoomZone.LivingRoom;
        animator = GetComponent<Animator>();
    }
   

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
        
        if (pauseCoroutine != null)
        {
            return;
        }

        switch (monsterState)
        {
            case MonsterState.Idle:
                HandleIdle();
                break;

            case MonsterState.Walk:

                if (!hasDestination)
                {
                    HandleWalk();
                }
                else if (!agent.pathPending &&
                        agent.remainingDistance <= agent.stoppingDistance)
                {
                    hasDestination = false;
                    monsterState = ChooseNextState();
                }

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
        float roll = Random.Range(0f, 1f);

        if (roll < 0.3f)
        {
            return MonsterState.Idle;
        }

        return MonsterState.Walk;
    }

    void HandleIdle()
    {
        pauseCoroutine = StartCoroutine(PauseMovement());
    }

    void HandleWalk()
    {
        agent.isStopped = false;
        agent.speed = walkSpeed;
        int roll = Random.Range(0, rooms.Length);
        roomState = rooms[roll];
        hasDestination = agent.SetDestination(GetRandomPositionForRoom(roomState));
    }

    Vector3 GetRandomPositionForRoom(RoomZone room)
    {
        switch (room)
        {
            case RoomZone.Bathroom1:
                return choosePos(bathroom1);

            case RoomZone.Bathroom2:
                return choosePos(bathroom2);

            case RoomZone.Bedroom1:
                return choosePos(bedroom1);

            case RoomZone.Bedroom2:
                return choosePos(bedroom2);

            case RoomZone.DiningRoom:
                return choosePos(diningroom);

            case RoomZone.LivingRoom:
                return choosePos(livingroom);

            case RoomZone.Kitchen:
                return choosePos(kitchen);
        }

        return transform.position;
    }

    Vector3 choosePos(Transform[] zones)
    {
        int index = Random.Range(0, zones.Length);

        Vector3 center = zones[index].position;
        Vector3 size = zones[index].localScale;

        float randomX = Random.Range(-size.x / 2f, size.x / 2f);
        float randomZ = Random.Range(-size.z / 2f, size.z / 2f);

        return new Vector3(
            center.x + randomX,
            center.y,
            center.z + randomZ
        );
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

    public IEnumerator PauseMovement()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        float duration = Random.Range(minIdleTime, maxIdleTime);

        yield return new WaitForSeconds(duration);

        pauseCoroutine = null;
        monsterState = ChooseNextState();

    }

    public Vector3 getMosterLoc()
    {
        return transform.position;
    }

}