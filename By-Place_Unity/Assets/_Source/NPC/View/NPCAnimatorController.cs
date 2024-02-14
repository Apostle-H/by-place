using UnityEngine;

namespace NPC.View
{
    public class NPCAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator npcAnimator;
        [SerializeField] private Transform lootAtTarget;
        [SerializeField] private float lootAtMinDistance;
        [SerializeField] private Transform lootAtRotationChecker;
        [SerializeField, Range(0f, 1f)] private float maxLookRotation;

        private void OnAnimatorIK(int layerIndex)
        {
            if ((lootAtTarget.position - transform.position).magnitude > lootAtMinDistance) 
                return;
            
            lootAtRotationChecker.LookAt(lootAtTarget.position);
            if (Mathf.Abs(lootAtRotationChecker.rotation.y) > maxLookRotation)
            {
                npcAnimator.SetLookAtWeight(0f);
                return;
            }
            
            npcAnimator.SetLookAtWeight(1f);
            npcAnimator.SetLookAtPosition(lootAtTarget.position);
        }
    }
}