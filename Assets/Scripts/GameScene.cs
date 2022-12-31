using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace AnswerPlease
{
    public class GameScene : MonoBehaviour
    {
        [SerializeField] RobotTextBuilder robotTextBuilder;
        
        [SerializeField] Robot robotPrefab;
        [SerializeField] TalkBalloon talkBalloonPrefab;
        
        [SerializeField] RectTransform faceWindowParent;
        [SerializeField] RectTransform laneParent;
        [SerializeField] RectTransform talkParent;
        
        [SerializeField] Button okButton;
        [SerializeField] Button ngButton;
        [SerializeField] Button[] commandButtons;
        [SerializeField] Text[] commandButtonTexts;

        bool IsShowQuestionText => currentTexts != null;

        List<Robot> robots = new List<Robot>();
        
        RobotTextBuilder.RobotTextPattern[] currentTexts;
        Coroutine talkCoroutine;
        List<(bool IsRobot, string Text)> talkTexts = new List<(bool IsRobot, string Text)>();
        
        Action endCallback;

        public void StartGame(Action endCallback)
        {
            this.endCallback = endCallback;
            
            gameObject.SetActive(true);

            HideQuestionText();

            StartCoroutine(BeltConveyor());
        }
        
        void Awake()
        {
            okButton.onClick.AddListener(OnClickOKButton);
            ngButton.onClick.AddListener(OnClickNGButton);

            for (var i = 0; i <commandButtons.Length; i++)
            {
                var index = i;
                commandButtons[index].onClick.AddListener(() => OnClickCommandButton(index));
            }
        }

        IEnumerator BeltConveyor()
        {
            while (true)
            {
                // ベルトコンベア更新
                foreach (var robot in robots)
                {
                    switch (robot.State)
                    {
                        case Robot.RobotState.InBeltConveyor:
                            if (!robot.IsNeedMove && robot.BeltConveyorQueueIndex == 0)
                            {
                                robot.State = Robot.RobotState.InBox;
                                robot.SetTargetPosition(Vector3.zero);
                    
                                var robotsTransform = robot.transform;
                                robotsTransform.SetParent(faceWindowParent, true);
                                robotsTransform.localScale = Vector3.one;
                                robotsTransform.localPosition = Vector3.left * 500;
                            }

                            break;
                        case Robot.RobotState.InBox:
                            if (!robot.IsNeedMove && !IsShowQuestionText && talkCoroutine == null)
                            {
                                ShowQuestionText();
                            }

                            break;
                        case Robot.RobotState.OutBox:
                            if (!robot.IsNeedMove)
                            {
                                robot.State = Robot.RobotState.OutBeltConveyor;
                                robot.SetTargetPosition(Vector3.right * 1200);
                                
                                var robotsTransform = robot.transform;
                                robotsTransform.SetParent(laneParent, true);
                                robotsTransform.localScale = Vector3.one * 0.5f;
                                robotsTransform.localPosition = Vector3.zero;
                            }

                            break;
                        
                        case Robot.RobotState.DustChute:
                        case Robot.RobotState.OutBeltConveyor:
                            if (!robot.IsNeedMove)
                            {
                                ReleaseRobot(robot);
                            }

                            break;
                    }
                }

                // ロボットが足りなければ追加
                var lastBeltConveyorQueueIndex = robots.Any() ? robots.Max(x => x.BeltConveyorQueueIndex) : -1;
                if (lastBeltConveyorQueueIndex < 4)
                {
                    var robot = GetRobot();
                    robot.BeltConveyorQueueIndex = lastBeltConveyorQueueIndex + 1;

                    robot.IsBadAI = Random.Range(0.0f, 100.0f) < 20.0f;
                    
                    robot.State = Robot.RobotState.InBeltConveyor;
                    robot.SetTargetPosition(Vector3.left * robot.BeltConveyorQueueIndex * 300);

                    var robotTransform = robot.transform;
                    robotTransform.SetParent(laneParent, true);
                    robotTransform.localScale = Vector3.one * 0.5f;
                    robotTransform.localPosition = Vector3.left * 1200;
                }

                yield return null;
            }
        }

        IEnumerator Talk()
        {
            var index = 0;
            while (true)
            {
                yield return null;
                
                if (talkTexts.Count <= index)
                {
                    continue;
                }

                var talkBalloon = Instantiate(talkBalloonPrefab, talkParent).GetComponent<TalkBalloon>();
                talkBalloon.Apply(talkTexts[index].IsRobot, talkTexts[index].Text);

                // ロボットの回答を表示
                yield return talkBalloon.Talk();

                index++;

                if (talkTexts.Count == index)
                {
                    // 次の質問を表示
                    ShowQuestionText();
                }
            }
        }

        void ShowQuestionText()
        {
            currentTexts = robotTextBuilder.GetRandomTexts(3);
            for (var i = 0; i < commandButtonTexts.Length; i++)
            {
                commandButtons[i].gameObject.SetActive(true);
                commandButtonTexts[i].text = currentTexts[i].QuestionText;
            }
        }

        void HideQuestionText()
        {
            for (var i = 0; i < commandButtonTexts.Length; i++)
            {
                commandButtons[i].gameObject.SetActive(false);
            }

            currentTexts = null;
        }

        void OnClickOKButton()
        {
            var currentRobot = robots.FirstOrDefault(robot => 
                robot.State == Robot.RobotState.InBox
                && robot.BeltConveyorQueueIndex == 0
                && !robot.IsNeedMove);
            
            if (currentRobot != null)
            {
                currentRobot.State = Robot.RobotState.OutBox;
                currentRobot.SetTargetPosition(Vector3.right * 500);
                
                DequeueBeltConveyor();

                HideQuestionText();
                
                talkTexts.Clear();
                
                if (talkCoroutine != null)
                {
                    StopCoroutine(talkCoroutine);
                    talkCoroutine = null;
                }

                var children = Enumerable.Range(0, talkParent.childCount).Select(i => talkParent.GetChild(i).gameObject);
                foreach (var child in children)
                {
                    Destroy(child);                    
                }
            }
        }
        
        void OnClickNGButton()
        {
            var currentRobot = robots.FirstOrDefault(robot => 
                robot.State == Robot.RobotState.InBox
                && robot.BeltConveyorQueueIndex == 0
                && !robot.IsNeedMove);

            if (currentRobot != null)
            {
                currentRobot.State = Robot.RobotState.DustChute;
                currentRobot.SetTargetPosition(Vector3.down * 500);

                DequeueBeltConveyor();
                
                HideQuestionText();

                talkTexts.Clear();
                
                if (talkCoroutine != null)
                {
                    StopCoroutine(talkCoroutine);
                    talkCoroutine = null;
                }

                var children = Enumerable.Range(0, talkParent.childCount).Select(i => talkParent.GetChild(i).gameObject);
                foreach (var child in children)
                {
                    Destroy(child);                    
                }
            }
        }
        
        void OnClickCommandButton(int index)
        {
            if (!IsShowQuestionText)
            {
                return;
            }

            talkTexts.Add((false, currentTexts[index].QuestionText));
            
            var currentRobot = robots.First(robot => robot.State == Robot.RobotState.InBox);
            var answerTexts = currentRobot.IsBadAI ? currentTexts[index].BadAnswerTexts : currentTexts[index].BestAnswerTexts;
            var answerText = answerTexts[Random.Range(0, answerTexts.Length)];
                
            talkTexts.Add((true, answerText));
            
            HideQuestionText();
            
            if (talkCoroutine == null)
            {
                talkCoroutine = StartCoroutine(Talk());
            }
        }

        void DequeueBeltConveyor()
        {
            foreach (var robot in robots)
            {
                if (robot.State == Robot.RobotState.InBeltConveyor)
                {
                    robot.BeltConveyorQueueIndex--;
                    robot.SetTargetPosition(Vector3.left * robot.BeltConveyorQueueIndex * 300);
                }
            }
        }

        Robot GetRobot()
        {
            var inactiveRobot = robots.FirstOrDefault(robot => robot.State == Robot.RobotState.Cache);
            if (inactiveRobot != null)
            {
                inactiveRobot.gameObject.SetActive(true);
                return inactiveRobot;
            }

            var newRobot = Instantiate(robotPrefab, laneParent);
            robots.Add(newRobot);
            return newRobot;
        }

        void ReleaseRobot(Robot robot)
        {
            robot.State = Robot.RobotState.Cache;
            robot.gameObject.SetActive(false);
        }
    }
}