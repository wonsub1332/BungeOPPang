using UnityEngine;
using System;

namespace Bungeoppang.Core
{
    /// <summary>
    /// 게임 내 재화 및 재고(반죽, 팥, 슈크림)를 관리하는 싱글톤 매니저입니다.
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        [Header("Currencies")]
        public int currentGold = 1000;

        [Header("Ingredients")]
        public int batterCount = 10;
        public int redBeanCount = 5;
        public int creamCount = 5;

        // UI 갱신을 위한 이벤트
        public event Action OnInventoryChanged;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool UseBatter()
        {
            if (batterCount > 0)
            {
                batterCount--;
                OnInventoryChanged?.Invoke();
                return true;
            }
            return false;
        }

        public void AddGold(int amount)
        {
            currentGold += amount;
            OnInventoryChanged?.Invoke();
        }

        public void RefillIngredients(int batter, int redBean, int cream)
        {
            batterCount += batter;
            redBeanCount += redBean;
            creamCount += cream;
            OnInventoryChanged?.Invoke();
        }
    }
}
