using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element_Script : MonoBehaviour
{

    Vector2 randPos, initPosition;

    float MARGE = 0.5f;

    void Start()
    {
        initPosition = transform.position;
        randPos = transform.position;
        randPos.y += Random.Range(-MARGE, MARGE);
        transform.position = randPos;

        StartCoroutine(Loopable_Float());
    }

    IEnumerator Loopable_Float()
    {
        Vector2 targetPos = transform.position;
        Vector2 currentPos = targetPos;
        targetPos.y += MARGE;

        while (true) {
            while (transform.position.y < targetPos.y)
            {
                currentPos.y += Time.deltaTime * 5f;
                transform.position = currentPos;
                yield return new WaitForEndOfFrame();
            }

            targetPos = initPosition;
            targetPos.y -= MARGE;

            while (transform.position.y < targetPos.y)
            {
                currentPos.y -= Time.deltaTime * 5f;
                transform.position = currentPos;
                yield return  new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
