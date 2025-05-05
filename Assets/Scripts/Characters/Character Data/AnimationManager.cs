using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator animator;
    public AnimStateStrings animStates;
    public RuntimeAnimatorController controller;

    public AnimatorStateInfo currentAnimStateInfo;
    public AnimatorClipInfo currentAnimatorClipInfo;

    public float normAnimTime { get; private set; }

    private bool playingAnimation;

    private void Start()
    {
        animStates = new AnimStateStrings();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        currentAnimStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        currentAnimatorClipInfo = GetAnimationClipInfo();

        normAnimTime = currentAnimStateInfo.normalizedTime;
    }

    private AnimatorClipInfo GetAnimationClipInfo()
    {
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);

        if (clipInfos.Length > 0)
        {
            AnimatorClipInfo animClipInfo = clipInfos[0];
            return animClipInfo;
        }
        else
        {
            //Debug.LogWarning("no current anim clip info in this frame");
            return new AnimatorClipInfo();
        }
    }

    public void PlayAnimation(string animState, bool oneShot = false)
    {
        if (!playingAnimation || oneShot)
        {
            StartCoroutine(PlayingAnimation(animState));
            playingAnimation = true;
        }
    }
    private IEnumerator PlayingAnimation(string animState)
    {
        animator.Play(animState, 0, 0);
        yield return new WaitUntil(() => currentAnimStateInfo.IsName(animState));
        while (currentAnimStateInfo.normalizedTime < 1)
        {
            yield return null;
        }
        playingAnimation = false;
    }

    public AnimationClip GetAnimationClipByName(string clipName)
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;
        foreach (AnimationClip clip in controller.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip;
            }
        }
        return null;
    }

}
