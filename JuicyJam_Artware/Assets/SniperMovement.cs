using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SniperMovement : MonoBehaviour
{
    enum EnemyState { CHASING, GRAPPLING, NOAGGRO }
    EnemyState currentState = EnemyState.CHASING;

    CharacterController charCtrl;
    NavMeshAgent navAgent;
    GameObject player;

    float aggroRange = 30;
    bool canGrapple = true;
    bool falling;

    private Vector3 grapplePoint;
    RaycastHit hit;

    Coroutine grappleCooldownCoroutine;
    Coroutine aggroCoroutine;

    [SerializeField] GameObject collisionChecker;
    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
            Destroy(gameObject);
    }

    private void Update()
    {
        if (currentState == EnemyState.CHASING)
        {
            Chase();
        }
        else if (currentState == EnemyState.GRAPPLING)
        {
            Grapple();
        }
        else if (currentState == EnemyState.NOAGGRO)
        {
            IsPlayerReturn();
        }

        if (falling)
        {
            Gravity();
        }
    }

    void Chase()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceFromPlayer < aggroRange && canGrapple)
        {
            navAgent.ResetPath();
            navAgent.enabled = false;
            currentState = EnemyState.GRAPPLING;
        }
        else
        {
            navAgent.enabled = true;
            navAgent.destination = player.transform.position;
        }
    }

    void Grapple()
    {
        float distFromGrapplePoint = Vector3.Distance(transform.position, grapplePoint);
        float distFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distFromGrapplePoint > 0.75f)
        {
            var step = 9f * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, grapplePoint, step);
        }

        if (distFromPlayer > aggroRange && distFromGrapplePoint <= 0.75f)
        {
            aggroCoroutine = StartCoroutine(AggroCooldown());
            return;
        }

        if (!canGrapple || currentState != EnemyState.GRAPPLING)
            return;

        if (distFromGrapplePoint <= 1)
            grapplePoint = NewGrapplePoint();
    }

    void IsPlayerReturn()
    {
        float distFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distFromPlayer < aggroRange)
        {
            Debug.Log("player came back before i could go wacko mode :)");
            StopCoroutine(aggroCoroutine);
            currentState = EnemyState.GRAPPLING;
        }
    }

    Vector3 NewGrapplePoint()
    {
        Vector3 newPos = transform.position;
        float distanceFromOrigin = 0;
        LayerMask grappleMask = 1 << 7 | 1 << 9;

        Vector3 characterCenter = transform.position + charCtrl.center;
        while (distanceFromOrigin < 20)
        {
            Vector3 randomDirection = Random.onUnitSphere;
            if (Physics.Raycast(characterCenter, randomDirection * 50, out hit, 30, grappleMask))
            {
                if (hit.transform.gameObject.layer == 7)
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red, 20);
                    newPos = hit.point;
                }
                else
                {
                    Debug.DrawLine(transform.position, hit.point, Color.black, 20);
                    newPos = transform.position;
                }
            }
            distanceFromOrigin = Vector3.Distance(newPos, transform.position);
        }
        Debug.DrawLine(transform.position, hit.point, Color.blue, 20);
        grappleCooldownCoroutine = StartCoroutine(GrappleCooldown());
        return newPos;
    }

    IEnumerator GrappleCooldown()
    {
        canGrapple = false;
        yield return new WaitForSeconds(Random.Range(4, 8));
        canGrapple = true;
    }

    IEnumerator AggroCooldown()
    {
        Debug.Log("player go too far >:(");
        currentState = EnemyState.NOAGGRO;
        yield return new WaitForSeconds(2);
        falling = true;
        yield return new WaitForSeconds(2);
        currentState = EnemyState.CHASING;
    }

    void Gravity()
    {
        RaycastHit groundPos;
        LayerMask grappleMask = 1 << 7 | 1 << 9;
        float distFromGround = 100;

        if (Physics.Raycast(transform.position, Vector3.down * 50, out groundPos, 50, grappleMask))
        {
            Debug.DrawLine(transform.position, groundPos.point, Color.yellow, 3);
            distFromGround = Vector3.Distance(transform.position, groundPos.point);
        }

        if (distFromGround > 1f)
        {
            var step = 3f * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, groundPos.point, step);
        }
        else
        {
            falling = false;
            currentState = EnemyState.CHASING;
        }
    }
}
