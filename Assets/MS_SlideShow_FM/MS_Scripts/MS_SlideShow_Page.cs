using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.EditorCoroutines.Editor;

public class MS_SlideShow_Page : MonoBehaviour
{
    public Text pageNumberTxt;

    List<MS_SlideShow_Element> elementsList = new List<MS_SlideShow_Element>();

    int reachedAnimation = 0;

    void OnEnable()
    {
       Init_Vars();
       Fill_Elements_List();
       StartCoroutine(Play_Elements_Animations());
    }

    void Init_Vars()
    {
        reachedAnimation = 0;
        elementsList = new List<MS_SlideShow_Element>();
    }

    void Fill_Elements_List()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<MS_SlideShow_Element>() != null)
            {
                elementsList.Add(transform.GetChild(i).GetComponent<MS_SlideShow_Element>());
            }
        }
    }

    IEnumerator Play_Elements_Animations()
    {
        for (int i = reachedAnimation; i < elementsList.Count; i++)
        {
            elementsList[i].Store_Init_Stats();
            elementsList[i].Prepare_Animation();
            yield return null;
        }
        yield return null;

        for (int i = reachedAnimation; i < elementsList.Count; i++)
        {
            if (elementsList[i].animationOrder == MS_SlideShow_Element.AnimationLayering.Auto)
            {
                StartCoroutine(elementsList[i].Preview_Start_Animation());

                while (!elementsList[i].isStartingAnimationEnds)
                {
                    yield return null;
                }
                reachedAnimation++;
            }
            else if (elementsList[i].animationOrder == MS_SlideShow_Element.AnimationLayering.Parallel)
            {
                StartCoroutine(elementsList[i].Preview_Start_Animation());
                reachedAnimation++;
            }
            yield return null;
        }
    }

    public void Set_Me(int myPageIndex)
    {
        if (pageNumberTxt != null)
        {
            pageNumberTxt.text = myPageIndex.ToString();
        }
    }

    public void Set_Active(bool isShowing)
    {
        gameObject.SetActive(isShowing);
    }

}
