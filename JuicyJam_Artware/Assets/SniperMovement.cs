using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperMovement : MonoBehaviour
{
    enum EnemyState { RUNNING, GRAPPLED }
    EnemyState currentState = EnemyState.GRAPPLED;

    CharacterController charCtrl;
    private Vector3 grapplePoint;

    RaycastHit hit;

    bool canGrappleAgain = true;

    Coroutine repositionCoroutine;

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (currentState == EnemyState.RUNNING)
        {
            Chase();
        }
        else if (currentState == EnemyState.GRAPPLED)
        {
            Grapple();
        }
    }

    Vector3 NewPosition()
    {
        Debug.Log("YEEHAW");
        currentState = EnemyState.GRAPPLED;
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

    }
    
    void Grapple()
    {
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
        grapplePoint = NewPosition();
        yield return new WaitForSeconds(delay);
        canGrappleAgain = true;
    }
}
