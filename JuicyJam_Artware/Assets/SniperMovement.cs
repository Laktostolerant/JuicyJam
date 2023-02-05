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

        if(Physics.Raycast(transform.position, Vector3.down, out hit, 10, 1 << 8))
        {
            navAgent.Warp(hit.point);
            Debug.DrawLine(transform.position, new Vector3(hit.point.x, hit.point.y - 100, hit.point.z), Color.red, 10);
        }

        grapplePoint = transform.position;
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
            Gravity();
    }

    //If close enough to the player, go grapple. 
    //Otherwise, chase the player wherever they are on the map.
    void Chase()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceFromPlayer < aggroRange && canGrapple)
        {
            navAgent.ResetPath();
            navAgent.enabled = false;
            grapplePoint = transform.position;
            currentState = EnemyState.GRAPPLING;
        }
        else
        {
            navAgent.enabled = true;
            navAgent.destination = player.transform.position;
        }
    }

    //Moves toward grapple point if there is any.
    void Grapple()
    {
        float distFromGrapplePoint = Vector3.Distance(transform.position, grapplePoint);
        float distFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        //Move toward grapple point if not there yet.
        if (distFromGrapplePoint > 1f)
        {
            var step = 9f * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, grapplePoint, step);
        }

        //If player exists range, starts an aggro cooldown where it eventually chases after.
        if (distFromPlayer > aggroRange && distFromGrapplePoint <= 1f)
        {
            aggroCoroutine = StartCoroutine(AggroCooldown());
            return;
        }

        if (!canGrapple || currentState != EnemyState.GRAPPLING)
            return;

        //Find new grapple point if it can grapple again.
        if (distFromGrapplePoint <= 1)
            grapplePoint = NewGrapplePoint();
    }

    //Stops the aggro cooldown if player returns.
    void IsPlayerReturn()
    {
        float distFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distFromPlayer < aggroRange)
        {
            Debug.Log("player came back before i could go wacko mode :)");
            StopCoroutine(aggroCoroutine);
            falling = false;
            currentState = EnemyState.GRAPPLING;
        }
    }

    //Finds a new point to grapple onto.
    Vector3 NewGrapplePoint()
    {
        Vector3 newPos = transform.position;
        float distanceFromOrigin = 0;
        LayerMask grappleMask = 1 << 7 | 1 << 8 | 1 << 9;

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

    //Cooldown between grapples.
    IEnumerator GrappleCooldown()
    {
        canGrapple = false;
        yield return new WaitForSeconds(Random.Range(4, 8));
        canGrapple = true;
    }

    //Drops enemy onto ground while not aggro & too far, then chases player.
    IEnumerator AggroCooldown()
    {
        currentState = EnemyState.NOAGGRO;
        yield return new WaitForSeconds(2);
        falling = true;
        yield return new WaitForSeconds(2);
        currentState = EnemyState.CHASING;
    }

    //Raycasts and sends to ground slowly...
    //Didnt want to add a rigidbody lol.
    //Works well enough.
    void Gravity()
    {
        RaycastHit groundPos;
        LayerMask grappleMask = 1 << 7 | 1 << 8 | 1 << 9;
        float distFromGround = 100;

        if (Physics.Raycast(transform.position, Vector3.down * 50, out groundPos, 50, grappleMask))
        {
            Debug.DrawLine(transform.position, groundPos.point, Color.yellow, 3);
            distFromGround = Vector3.Distance(transform.position, groundPos.point);
        }

        if (distFromGround > 1f)
        {
            var step = 5f * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, groundPos.point, step);
        }
        else
        {
            falling = false;
            currentState = EnemyState.CHASING;
        }
    }
}
