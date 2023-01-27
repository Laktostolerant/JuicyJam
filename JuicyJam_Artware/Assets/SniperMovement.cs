using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperMovement : MonoBehaviour
{
    enum EnemyState { CHASING, REPOSITIONING, MOVINGTONEWPOS, SHOOTING }
    EnemyState currentState = EnemyState.REPOSITIONING;

    CharacterController charCtrl;
    Vector3 targetPos;

    [SerializeField] LayerMask testMask;
    RaycastHit hit;

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("CURRENT STATE:" + currentState);

        if (currentState == EnemyState.SHOOTING)
        {
            Debug.Log("lol");
        }
        else if (currentState == EnemyState.CHASING)
        {

        }
        else if(currentState == EnemyState.MOVINGTONEWPOS)
        {
            var offset = targetPos - transform.position;
            charCtrl.Move(offset * Time.fixedDeltaTime);
            if(Vector3.Distance(transform.position, targetPos) < 1.5f)
            {
                Debug.Log("IM CLOSE ENOUGH, TIME TO GO TO NEW POS LOL");
                StartCoroutine(Snoper());
            }
            else
            {
                Debug.Log("DISTANCE IS: " + Vector3.Distance(transform.position, targetPos));
            }
        }
        else if (currentState == EnemyState.REPOSITIONING)
        {
            targetPos = NewPosition(transform.position);
            if(targetPos != transform.position)
                currentState = EnemyState.MOVINGTONEWPOS;

            Debug.DrawRay(transform.position, targetPos, Color.green, 3);
        }
        else
        {
            currentState = EnemyState.CHASING;
        }
    }

    Vector3 NewPosition(Vector3 originPosition)
    {
        Vector3 newPos = new Vector3(0, 0, 0);

        Vector3 randomDirection = Random.insideUnitSphere * 100;
        LayerMask mask = LayerMask.NameToLayer("Wall");
        mask = 1 << 7;

        Vector3 p1 = transform.position + charCtrl.center;

        if (Physics.SphereCast(p1, 2, randomDirection, out hit, 100, mask))
        {
            newPos = hit.transform.position;
            Debug.Log("HIT!");
        }
        else
        {
            newPos = transform.position;
            Debug.Log("NO HIT :(");
        }
        return newPos;
    }

    IEnumerator Snoper()
    {
        yield return new WaitForSeconds(1f);
        currentState = EnemyState.REPOSITIONING;
    }
}
