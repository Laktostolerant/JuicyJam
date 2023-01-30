using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SniperMovement : MonoBehaviour
{
    enum EnemyState { CHASING, GRAPPLING, NOAGGRO }
    EnemyState currentState = EnemyState.GRAPPLING;

    GameObject player;
    CharacterController charCtrl;
    NavMeshAgent navAgent;
    private Vector3 grapplePoint;

    RaycastHit hit;

    bool canGrappleAgain = true;
    bool falling = false;
    int aggroRange = 15;

    Coroutine repositionCoroutine;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
        {
            Debug.Log("No player found...");
            GameObject.Destroy(gameObject);
        }
        charCtrl = GetComponent<CharacterController>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (currentState == EnemyState.CHASING)
            Chase();
        else if (currentState == EnemyState.GRAPPLING)
            Grapple();
    }

    void FixedUpdate()
    {
        Debug.Log("current state is " + currentState);
        if(falling)
            Gravity();
    }

    Vector3 NewPosition()
    {
        Debug.Log("YEEHAW");
        currentState = EnemyState.GRAPPLING;
        Vector3 newPos = transform.position;
        float distanceFromOrigin = 0;
        LayerMask grappleMask = 1 << 7 | 1 << 9;

        Vector3 characterCenter = transform.position + charCtrl.center;
        while(distanceFromOrigin < 20)
        {
            Vector3 randomDirection = Random.onUnitSphere;
            if (Physics.Raycast(characterCenter, randomDirection * 50, out hit, 30, grappleMask))
            {
                if(hit.transform.gameObject.layer == 7)
                {
                    newPos = hit.point;
                    Debug.DrawLine(transform.position, hit.point, Color.red, 20);
                }
                else
                {
                    newPos = transform.position;
                    Debug.DrawLine(transform.position, hit.point, Color.black, 20);
                }

            }
            distanceFromOrigin = Vector3.Distance(newPos, transform.position);
        }
        Debug.DrawLine(transform.position, hit.point, Color.blue, 20);
        return newPos;
    }

    void Chase()
    {
        if (currentState == EnemyState.NOAGGRO)
            return;

        Debug.Log("i be chasin");

        if(Vector3.Distance(player.transform.position, transform.position) > aggroRange)
        {
            navAgent.destination = player.transform.position;
        }
        else
        {
            StartCoroutine(Reposition(Random.Range(4, 10)));
        }
    }
    
    void Grapple()
    {
        if (currentState == EnemyState.NOAGGRO)
            return;

        if(Vector3.Distance(player.transform.position, transform.position) > aggroRange && repositionCoroutine == null)
        {
            StartCoroutine(AggroCooldown());
        }

        var moveOffset = grapplePoint - transform.position;
        charCtrl.Move(moveOffset * Time.fixedDeltaTime);

        if (canGrappleAgain && charCtrl.velocity.magnitude < 1)
        {
            repositionCoroutine = StartCoroutine(Reposition(Random.Range(4, 10)));
        }
    }

    IEnumerator Reposition(float delay)
    {
        canGrappleAgain = false;
        if(currentState == EnemyState.GRAPPLING || currentState == EnemyState.CHASING)
            grapplePoint = NewPosition();
        yield return new WaitForSeconds(delay);
        canGrappleAgain = true;
    }

    IEnumerator AggroCooldown()
    {
        currentState = EnemyState.NOAGGRO;
        falling = true;
        yield return new WaitUntil(() => !falling);
        //currentState = EnemyState.CHASING;
    }

    void Gravity()
    {
        //charCtrl.Move(new Vector3(transform.position.x, -0.1f, transform.position.z) * Time.fixedDeltaTime * 0.00001f);
        if (charCtrl.velocity.magnitude < 1)
        {
            Debug.Log("hi");
            //falling = false;
        }
    }

    Vector3 FloorPos()
    {
        Vector3 floor = transform.position;
        LayerMask grappleMask = 1 << 7 | 1 << 9;
        Vector3 characterCenter = transform.position + charCtrl.center;

        if (Physics.Raycast(characterCenter, Vector3.down * 50, out hit, 30, grappleMask))
        {
            floor = hit.point;
        }

        return floor;
    }
}
