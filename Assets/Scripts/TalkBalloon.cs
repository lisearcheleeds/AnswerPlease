using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AnswerPlease
{
    public class TalkBalloon : MonoBehaviour
    {
        [SerializeField] RectTransform container;
        [SerializeField] Text talkText;
        [SerializeField] Image talkBackground;
        [SerializeField] Color robotColor;
        [SerializeField] Color userColor;

        bool isRobot;
        string text;

        public void Apply(bool isRobot, string text)
        {
            this.isRobot = isRobot;
            this.text = text;

            container.localPosition = new Vector3(isRobot ? -10 : 10, 0, 0);
            talkBackground.color = isRobot ? robotColor : userColor;
        }

        public IEnumerator Talk()
        {
            for (var i = 0; i < text.Length + 1; i++)
            {
                talkText.text = text.Substring(0, i);
                yield return new WaitForSeconds(isRobot ? 0.1f : 0.02f);
            }
        }
    }
}