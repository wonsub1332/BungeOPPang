using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Bungeoppang.Core
{
    /// <summary>
    /// 게임 종료 시 나타나는 결과 팝업 창입니다.
    /// </summary>
    public class ResultPopupUI : MonoBehaviour
    {
        [Header("Statistics Texts")]
        public Text scoreText;
        public Text perfectCountText;
        public Text earnedGoldText;

        [Header("Buttons")]
        public Button restartButton;
        public Button exitButton;

        private void Start()
        {
            if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
            if (exitButton != null) exitButton.onClick.AddListener(ExitToMain);
            
            // 초기에는 비활성화
            gameObject.SetActive(false);
        }

        public void Show(int score, int perfects, int gold)
        {
            gameObject.SetActive(true);
            if (scoreText != null) scoreText.text = $"최종 점수: {score}";
            if (perfectCountText != null) perfectCountText.text = $"Perfect 개수: {perfects}";
            if (earnedGoldText != null) earnedGoldText.text = $"획득 골드: +{gold} G";
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void ExitToMain()
        {
            // 메인 화면으로 이동 (현재는 씬이 하나이므로 리로드)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
