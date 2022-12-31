using UnityEngine;

namespace AnswerPlease
{
    public class Robot : MonoBehaviour
    {
        Vector3 targetPosition;
        
        public RobotState State { get; set; }
        public int BeltConveyorQueueIndex { get; set; }
        public bool IsBadAI { get; set; }

        public bool IsNeedMove => (targetPosition - transform.localPosition).sqrMagnitude > 10.0f;

        public enum RobotState
        {
            Cache,
            InBeltConveyor,
            InBox,
            OutBox,
            DustChute,
            OutBeltConveyor,
        }

        public void SetTargetPosition(Vector3 position)
        {
            targetPosition = position;
        }

        void Update()
        {
            var prevPosition = transform.localPosition;
            var direction = (targetPosition - prevPosition).normalized;
            var move = direction * transform.localScale.x * Time.deltaTime * 300.0f;

            if ((targetPosition - prevPosition).sqrMagnitude < move.sqrMagnitude)
            {
                transform.localPosition = targetPosition;
                return;
            }

            transform.localPosition = prevPosition + direction * transform.localScale.x * Time.deltaTime * 300.0f;
        }
    }
}
