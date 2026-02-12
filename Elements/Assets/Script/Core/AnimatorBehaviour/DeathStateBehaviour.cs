using UnityEngine;

namespace ElementGame.Core
{
    public class DeathStateBehaviour : StateMachineBehaviour
    {
        private bool _notified;

        public override void OnStateEnter(
            Animator animator,
            AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _notified = false;
        }

        public override void OnStateUpdate(
            Animator animator,
            AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            if (_notified) return;

            if (stateInfo.normalizedTime >= 1f)
            {
                _notified = true;

                var view = animator.GetComponent<ElementGame.Core.ElementView>();
                if (view != null)
                {
                    view.NotifyDeathFinished();
                }
            }
        }
    }
}