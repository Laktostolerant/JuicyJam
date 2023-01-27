using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperMovement : MonoBehaviour
{
    enum EnemyState { FIGHTING, REPOSITIONING }
    EnemyState currentState = EnemyState.REPOSITIONING;

    CharacterController charCtrl;
    private Vector3 targetPos;

    RaycastHit hit;

    bool repositionDelay;

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
    }



    void Update()
    {
        if (!repositionDelay)
            StartCoroutine(Reposition());


        if (currentState == EnemyState.FIGHTING)
        {
            Debug.Log("lol");
        }
        else if (currentState == EnemyState.REPOSITIONING)
        {
            var offset = targetPos - transform.position;
            charCtrl.Move(offset * Time.fixedDeltaTime);
        }
    }

    Vector3 NewPosition()
    {
        Vector3 newPos = transform.position;

        float distanceFromOrigin = 0;

        LayerMask mask = 1 << 7;

        Vector3 characterCenter = transform.position + charCtrl.center;
        while(distanceFromOrigin < 20)
        {
            Vector3 randomDirection = Random.onUnitSphere;
            if (Physics.Raycast(characterCenter, randomDirection * 50, out hit, 30, mask))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red, 20);
                newPos = hit.point;
            }
            distanceFromOrigin = Vector3.Distance(newPos, transform.position);
            Debug.Log("distance from origin: " + distanceFromOrigin + " and hit point Y: " + hit.point.y);
        }

        Debug.DrawLine(transform.position, hit.point, Color.blue, 20);
        Debug.Log("finally broke free: " + hit.point);
        return newPos;
    }

    IEnumerator Reposition()
    {
        repositionDelay = true;
        yield return new WaitForSeconds(0.5f);
        targetPos = NewPosition();
        currentState = EnemyState.REPOSITIONING;
        repositionDelay = false;
    }
}
