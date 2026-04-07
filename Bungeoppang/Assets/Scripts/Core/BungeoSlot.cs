using UnityEngine;

namespace Bungeoppang.Core
{
    /// <summary>
    /// 붕어빵 한 칸의 상태와 굽기 로직을 관리하는 클래스입니다.
    /// 마우스 클릭(터치)으로 조작하며, 색상 변화로 상태를 확인할 수 있습니다.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class BungeoSlot : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("완벽하게 익을 때까지 걸리는 총 시간")]
        public float cookingTime = 5f; 
        [Tooltip("완벽한 상태(Perfect)를 유지하는 시간 (이후에는 타버림)")]
        public float perfectDuration = 2f; 
        
        [Header("Runtime Status")]
        public BungeoState currentState = BungeoState.Empty;
        
        private float timer = 0f;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            UpdateVisual();
        }

        private void Update()
        {
            if (currentState == BungeoState.Empty) return;

            timer += Time.deltaTime;

            // FSM 기반 상태 전이 로직
            switch (currentState)
            {
                case BungeoState.Batter:
                    // 반죽 상태에서 40% 시간이 흐르면 익어가는 중(Cooking)으로 전이
                    if (timer >= cookingTime * 0.4f) TransitionTo(BungeoState.Cooking);
                    break;
                case BungeoState.Cooking:
                    // 총 익는 시간(cookingTime)에 도달하면 완벽함(Perfect)으로 전이
                    if (timer >= cookingTime) TransitionTo(BungeoState.Perfect);
                    break;
                case BungeoState.Perfect:
                    // 완벽함 유지 시간(perfectDuration)을 초과하면 타버림(Burnt)으로 전이
                    if (timer >= cookingTime + perfectDuration) TransitionTo(BungeoState.Burnt);
                    break;
            }
        }

        /// <summary>
        /// 마우스 또는 터치 입력을 처리합니다. (BoxCollider2D 필요)
        /// </summary>
        private void OnMouseDown()
        {
            if (currentState == BungeoState.Empty)
            {
                StartCooking();
            }
            else if (currentState == BungeoState.Perfect || currentState == BungeoState.Burnt)
            {
                Harvest();
            }
        }

        private void StartCooking()
        {
            timer = 0f;
            TransitionTo(BungeoState.Batter);
            Debug.Log("[BungeoSlot] 반죽을 부었습니다. 굽기 시작!");
        }

        private void Harvest()
        {
            if (currentState == BungeoState.Perfect)
            {
                Debug.Log("[BungeoSlot] ★ 완벽한 붕어빵 수확! ★ (+100 Gold)");
            }
            else
            {
                Debug.Log("[BungeoSlot] ㅠㅠ 붕어빵이 타버렸습니다. (수익 없음)");
            }
            
            TransitionTo(BungeoState.Empty);
            timer = 0f;
        }

        private void TransitionTo(BungeoState newState)
        {
            currentState = newState;
            UpdateVisual();
        }

        /// <summary>
        /// 임시로 SpriteRenderer의 색상을 변경하여 상태를 시각화합니다.
        /// </summary>
        private void UpdateVisual()
        {
            if (spriteRenderer == null) return;

            switch (currentState)
            {
                case BungeoState.Empty:
                    spriteRenderer.color = Color.gray; // 빈 틀: 회색
                    break;
                case BungeoState.Batter:
                    spriteRenderer.color = Color.white; // 반죽: 흰색
                    break;
                case BungeoState.Cooking:
                    spriteRenderer.color = new Color(1f, 0.92f, 0.016f); // 익어가는 중: 밝은 노랑
                    break;
                case BungeoState.Perfect:
                    spriteRenderer.color = new Color(0.85f, 0.44f, 0.14f); // 완벽함: 노릇노릇한 갈색
                    break;
                case BungeoState.Burnt:
                    spriteRenderer.color = Color.black; // 타버림: 검은색
                    break;
            }
        }
    }
}
