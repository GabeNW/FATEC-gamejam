using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextClickEffect : MonoBehaviour
{
    public Text text;
    public Button button;
    public bool clicked;

    private bool switchClicked;

    void Update()
    {
        if(clicked != switchClicked)
        {
            if (clicked)
            {
                StartCoroutine(DelayedClick(true));
            } else
            {
                //StartCoroutine(DelayedClick(false));
                Unclick();
            }
            switchClicked = clicked;
        }
    }
    public void Click(){
        text.alignment = TextAnchor.MiddleCenter;
    }

    public void Unclick()
    {
        text.alignment = TextAnchor.UpperCenter;
    }

    IEnumerator DelayedClick(bool c)
    {
        yield return new WaitForSeconds(0.1f);
        Click();

        //if (c)
        //{
        //    yield return new WaitForSeconds(0.1f);
        //    Click();
        //} else
        //{
        //    yield return new WaitForSeconds(0.2f);
        //    Unclick();
        //}
    }
}
