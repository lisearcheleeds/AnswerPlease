using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AnswerPlease
{
    public class TitleScene : MonoBehaviour
    {
        [SerializeField] Button titleButton;
        [SerializeField] Text infoText;
        
        Action endCallback;

        public void StartTitle(Action endCallback)
        {
            this.endCallback = endCallback;
            
            gameObject.SetActive(true);
            StartCoroutine(TitleWaiter());
        }

        void Awake()
        {
            titleButton.onClick.AddListener(OnClickTitleButton);
        }

        IEnumerator TitleWaiter()
        {
            titleButton.interactable = false;

            /*
            infoText.text = "ゲームを初期化中";
            yield return new WaitForSeconds(1.5f);
            infoText.text = "ネットワークに接続中";
            yield return new WaitForSeconds(0.5f);
            infoText.text = "アセットをロード中";
            yield return new WaitForSeconds(1.0f);
            infoText.text = "読み込んでます";
            yield return new WaitForSeconds(2.0f);
            infoText.text = "他のプレイヤーを待っています";
            yield return new WaitForSeconds(1.0f);
            infoText.text = "他のプレイヤーを待っています.";
            yield return new WaitForSeconds(1.0f);
            infoText.text = "他のプレイヤーを待っています..";
            yield return new WaitForSeconds(1.0f);
            infoText.text = "他のプレイヤーを待っています...";
            yield return new WaitForSeconds(1.0f);
            infoText.text = "他のプレイヤーを待っています....";
            */
            yield return new WaitForSeconds(1.0f);
            infoText.text = "タップしてゲームを開始";
            
            titleButton.interactable = true;
        }

        void OnClickTitleButton()
        {
            gameObject.SetActive(false);
            endCallback();
        }
    }
}