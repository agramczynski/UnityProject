using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehav : MonoBehaviour
{
    [System.Serializable()]
    public class ClipNotFoundException : System.Exception
    {
        public ClipNotFoundException() : base() { }
        public ClipNotFoundException(string message) : base(message) { }
        public ClipNotFoundException(string message, System.Exception inner) : base(message, inner) { }

        protected ClipNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    // Start is called before the first frame update
    void Start()
    {
    }


    public void PlayAndDestroy(string animation, float time)
    {
        Animator animator = GetComponent<Animator>();
        animator.speed = 1/time;
        time *= GetAnimationLength(animator, animation);
        Destroy(gameObject, time);
        animator.Play(animation, 0, time);
    }
    float GetAnimationLength(Animator anim, string animName)
    {
        RuntimeAnimatorController ac = anim.runtimeAnimatorController;
        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name == animName)
            {
                Debug.Log(clip.length);
                return clip.length;
            }
        }
        throw new ClipNotFoundException("No clip with name: \"" + animName + "\"");
    }
}
