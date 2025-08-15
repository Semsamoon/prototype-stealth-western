using System;
using Perception;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : CharacterController
{
    [SerializeField] private Waypoint[] _waypoints = Array.Empty<Waypoint>();
    [SerializeField] private PatrolMode _patrolMode = PatrolMode.Loop;
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField, Range(0, 360)] private float _detectionAngleDegrees = 90;
    [SerializeField] private float _speed = 3.5f;

    [SerializeField] private PlayerController _player;

    private NavMeshAgent _navMeshAgent;
    private SphereCollider _detectionZone;

    private State _currentState;

    private int _currentWaypointIndex;
    private bool _isMovingForward = true;

    private bool _isPaused;
    private float _pauseTimer;

    protected override void Awake()
    {
        base.Awake();

        _navMeshAgent = GetComponent<NavMeshAgent>();

        _detectionZone = gameObject.AddComponent<SphereCollider>();
        _detectionZone.radius = _detectionRadius;
        _detectionZone.isTrigger = true;

        _currentState = State.Patrol;
    }

    private void Start()
    {
        InitializeAgent();
    }

    private void InitializeAgent()
    {
        if (_waypoints.Length == 0) return;
        _navMeshAgent.speed = _speed;
        MoveToNextWaypoint();
    }

    private void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        if (_currentState == State.Patrol) HandlePatrolState();
        else HandleAttackState();
    }

    private void HandlePatrolState()
    {
        if (_currentState != State.Patrol) return;
        if (_isPaused) HandlePause();
        else if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.5f) OnReachWaypoint();
    }

    private void HandlePause()
    {
        _pauseTimer -= Time.deltaTime;
        if (_pauseTimer <= 0) MoveToNextWaypoint();
    }

    private void OnReachWaypoint()
    {
        if (_waypoints.Length == 0) return;
        _isPaused = true;
        _pauseTimer = _waypoints[_currentWaypointIndex].Pause;
    }

    private void MoveToNextWaypoint()
    {
        if (_waypoints.Length == 0) return;
        _isPaused = false;
        UpdateWaypointIndex();
        _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].Transform.position);
    }

    private void UpdateWaypointIndex()
    {
        if (_patrolMode == PatrolMode.Loop)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            return;
        }

        if (_isMovingForward)
        {
            if (_currentWaypointIndex < _waypoints.Length - 1)
            {
                _currentWaypointIndex++;
                return;
            }

            _currentWaypointIndex--;
            _isMovingForward = false;
            return;
        }

        if (_currentWaypointIndex > 0)
        {
            _currentWaypointIndex--;
            return;
        }

        _currentWaypointIndex++;
        _isMovingForward = true;
    }

    private void HandleAttackState()
    {
        if (!IsPlayerInRange() || _player.IsInStealth())
        {
            TransitionToPatrol();
            return;
        }

        AttackPlayer();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_currentState == State.Attack) return;
        if (!other.TryGetComponent<PlayerController>(out var player)) return;
        if (player.IsInStealth()) return;
        TransitionToAttack();
    }

    private bool IsPlayerInRange()
    {
        if (!_player) return false;
        var distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance > _detectionRadius) return false;

        var angle = new float2(math.cos(math.radians(_detectionAngleDegrees) / 2), 0);
        var cone = new ComponentSightCone { AnglesCos = angle, ClipSquared = 0, RadiusSquared = _detectionRadius * _detectionRadius };
        var localToWorld = new LocalToWorld { Value = transform.localToWorldMatrix };
        return cone.IsInside(transform.position, _player.transform.position, localToWorld);
    }

    private void AttackPlayer()
    {
        if (!_player.TryGetComponent<PlayerHealth>(out var health)) return;
        health.TakeDamage();
    }

    private void TransitionToPatrol()
    {
        _currentState = State.Patrol;
        _animator.SetTrigger("Attack");
    }

    private void TransitionToAttack()
    {
        _currentState = State.Attack;
        _animator.SetTrigger("Attack");
    }

    private void OnDrawGizmosSelected()
    {
        var angle = new float2(math.radians(_detectionAngleDegrees) / 2, 0);
        SightSenseAuthoring.DrawCone(transform.position, transform.rotation, 0, _detectionRadius, angle, Color.red);
    }

    private enum PatrolMode
    {
        Loop,
        BackAndForth
    }

    private enum State
    {
        Patrol,
        Attack
    }

    [Serializable]
    public struct Waypoint
    {
        public Transform Transform;
        public float Pause;
    }
}