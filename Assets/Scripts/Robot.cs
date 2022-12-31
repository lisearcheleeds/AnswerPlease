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
            var direction = (targetPosition - transform.localPosition).normalized;
            transform.localPosition = transform.localPosition + direction * transform.localScale.x * Time.deltaTime * 300.0f;
        }
    }
}
