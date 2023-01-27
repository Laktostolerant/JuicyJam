using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperMovement : MonoBehaviour
{
    enum EnemyState { FIGHTING, REPOSITIONING}
    EnemyState currentState = EnemyState.REPOSITIONING;

    CharacterController charCtrl;
    Vector3 targetPos;

    RaycastHit hit;

    bool repositionDelay;

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
    }



    void Update()
    {
        if(!repositionDelay)
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
        Vector3 randomDirection = Random.insideUnitSphere;
        Debug.Log("random sphere: " + randomDirection);

        LayerMask mask = 1 << 7;

        Vector3 characterCenter = transform.position + charCtrl.center;

        if (Physics.SphereCast(transform.position, 3, randomDirection, out hit, 500, mask))
        {
            if(hit.transform.position.y > 10)
            {
                newPos = hit.transform.position;
                Debug.Log("new position: " + newPos);
            }
        }
        return newPos;
    }

    IEnumerator Reposition()
    {
        repositionDelay = true;
        yield return new WaitForSeconds(1f);
        targetPos = NewPosition();
        Debug.DrawLine(transform.position, targetPos, Color.red, 30);
        currentState = EnemyState.REPOSITIONING;
        repositionDelay = false;
    }
}
