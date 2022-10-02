using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseCopyManager : MonoBehaviour
{
    /*private Animator animator;
    public AnimationClip clipTest;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(AnimationCo(clipTest, null));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            print("Base Layer." + clipTest.name);
            animator.Play(clipTest.name);
            
            
        }
    }

    // Current issue is that we don't know what set of bones are currently being
    // used by the animation so we need to remove them whenever an animation stops 

    /// <summary>
    /// Attaches all the bones necessary to the anim component by getting via
    /// the keys. Then sends data to the animation manager as a coroutine 
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="keys"></param>
    private void TryPoseAnimation(AnimationClip anim, string[] keys)
    {
        
    }

    /// <summary>
    /// The coroutine that is passed to the animation manager. Animated
    /// the pose copy and ends it when the animation is over. clearns up 
    /// the animation from its mix of bones when done as well 
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="poseCopies"></param>
    /// <returns></returns>
    private IEnumerator AnimationCo(AnimationClip anim, PoseCopy[] poseCopies)
    {
        print(anim.name);
       // animator.Play(anim.name);


        yield return null;
    }*/




    // Old
    [SerializeField] List<PoseCopy> body;
    [SerializeField] List<PoseCopy> legs;

    

    private Animator animator;
    public AnimationClip clipTest;

    private Quaternion[] originalValuesBody;


    // hard selections that cannot be changed in the 
    private enum SizzleSections
    {
        neck,
        jaw,
        mid,
        tail,
        frontLegs,
        backLegs
    };

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        originalValuesBody = new Quaternion[body.Count];


        for (int i = 0; i < body.Count; i++)
        {
            originalValuesBody[i] = body[i].TargetValue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Base Layer." + clipTest.name);
            animator.Play(clipTest.name);
        }

        for (int i = 0; i < body.Count; i++)
        {
            body[i].UpdateTarget(originalValuesBody[i]);
        }
    }

}
