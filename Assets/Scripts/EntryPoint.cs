using UnityEngine;

namespace AnswerPlease
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] TitleScene titleScene;
        [SerializeField] GameScene gameScene;
        
        void Awake()
        {
            Application.targetFrameRate = 60;
            
            titleScene.gameObject.SetActive(false);
            gameScene.gameObject.SetActive(false);
            
            StartTitle();
        }

        void StartTitle()
        {
            titleScene.StartTitle(StartGame);
        }

        void StartGame()
        {
            gameScene.StartGame(StartTitle);
            
            titleScene.gameObject.SetActive(false);
            gameScene.gameObject.SetActive(true);
        }
    }    
}
