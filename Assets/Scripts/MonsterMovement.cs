using UnityEngine;

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

    public enum RoomZome
    {
        Bedroom1,
        Bedroom2,
        Bathroom1,
        Bathroom2,
        LivingRoom,
        Kitchen,
        DiningRoom
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

    public MonsterState monsterState;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterState = MonsterState.Idle;
    }
   

    void Update()
    {
        switch (ChooseNextState())
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

    void ChooseNextState()
    {
        float roll = Random.Range(0f, 1f);

        if (roll < 0.3f)
        {
            monsterState = MonsterState.Idle;
        }
        else if (roll < 1.0f)
        {
            monsterState = MonsterState.Wandering;
        }
        // Implement suspicious later
        else
        {
            monsterState = MonsterState.Suspicious;
        }
    }

    void HandleIdle()
    {
        agent.SetSpeed();
    }

    void HandleWalk()
    {
        movement.SetSpeed(walkSpeed);
    }

    void HandleSprint()
    {
        movement.SetSpeed(sprintSpeed);
    }

    void HandleSuspicious()
    {
        movement.SetSpeed(suspiciousSpeed);
    }

    void HandleChase()
    {
        movement.SetSpeed(chaseSpeed);
    }
}