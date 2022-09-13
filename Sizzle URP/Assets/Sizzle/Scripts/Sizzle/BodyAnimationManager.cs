using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnimationManager : MonoBehaviour
{

    private Coroutine currentHeadAnim;
    private string key;

    /// <summary>
    /// Tries to animate the head if it is free to do so
    /// </summary>
    /// <param name="anim">The coroutine animation that is passed</param>
    /// <param name="priority">Will ovveride the current animation playing and immediatly go to the passed animation</param>
    /// <returns></returns>
    public bool TryAnimateHead(IEnumerator anim, string _key, bool priority = false)
    {
        if(currentHeadAnim == null || priority == true)
        {
            key = _key;
            currentHeadAnim = StartCoroutine(anim);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void EndAnimation(string _key)
    {
        //print("Trying to end");
        if(key == _key)
        {
            StopCoroutine(currentHeadAnim);
            currentHeadAnim = null;
        }
    }


    private void Update()
    {
        //print(currentHeadAnim);
    }
}
