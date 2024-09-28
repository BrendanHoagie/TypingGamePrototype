using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum STATE { WANDER, CHASE, ATTACK, DIE };
    public float wanderSpeed;
    public float chaseSpeed;
    public float minWanderTime;
    public float maxWanderTime;
    public float sightDistance;
    public float strikeDistance;
    public float rotationSpeed;
    public LayerMask willAttackLayer;

    private STATE _currentState;
    private float _currentWanderTime;
    private Vector2 _currentDirection;
    private GameObject _currentTarget;
    private Rigidbody2D _myRigidbody2D;

    void Start()
    {
        _myRigidbody2D = GetComponent<Rigidbody2D>();
        EnterStateWander();
    }
    void Update()
    {
        switch (_currentState)
        {
            case STATE.WANDER:
                UpdateWander();
                break;
            case STATE.CHASE:
                UpdateChase();
                break;
            case STATE.ATTACK:
                UpdateAttack();
                break;
            case STATE.DIE:
                UpdateDie();
                break;
        }
    }
    private void EnterStateWander()
    {
        _currentState = STATE.WANDER;
        _currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        _currentWanderTime = Random.Range(minWanderTime, maxWanderTime);
    }
    private void UpdateWander()
    {
        _myRigidbody2D.velocity = _currentDirection * wanderSpeed;
        _currentWanderTime -= Time.deltaTime;
        if (_currentWanderTime <= 0f) EnterStateWander();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _currentDirection, sightDistance, willAttackLayer.value);
        if (hit.collider != null)
        {
            Debug.Log("Attack");
            EnterStateChase(hit.collider.gameObject);
        }
    }
    private void EnterStateChase(GameObject target)
    {
        _currentState = STATE.CHASE;
        _currentTarget = target;
        _currentDirection = (target.transform.position - transform.position).normalized;
    }
    private void UpdateChase()
    {
        Vector2 targetDirection = (_currentTarget.transform.position - transform.position).normalized;
        _currentDirection = Vector3.RotateTowards(_currentDirection.normalized,
        targetDirection, rotationSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f).normalized;
        _myRigidbody2D.velocity = _currentDirection * chaseSpeed;

        float targetDistance = Vector3.Distance(_currentTarget.transform.position, transform.position);
        if (targetDistance <= strikeDistance)
        {
            EnterStateAttack();
        }
        else if (targetDistance > sightDistance || Vector2.Angle(_currentDirection, targetDirection) > 30f)
        {
            _currentState = STATE.WANDER;
        }
    }
    private void EnterStateAttack()
    {
        _currentState = STATE.ATTACK;
        _myRigidbody2D.velocity = Vector2.zero;
    }
    private void UpdateAttack()
    {
        //Nothing to do here until the attack ends!
    }
    public void DoAttack()
    {
        //Nothing for now
    }
    public void AttackOver()
    {
        EnterStateWander();
    }
    public void Die()
    {
        EnterStateDie();
    }
    private void EnterStateDie()
    {
        _currentState = STATE.DIE;
        _myRigidbody2D.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
    }
    private void UpdateDie()
    {
        Destroy(gameObject);
    }
}
