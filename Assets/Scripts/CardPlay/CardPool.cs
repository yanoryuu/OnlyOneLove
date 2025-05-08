using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class CardPool : MonoBehaviour
    {
        
        //全部のカードの種類
        public static CardPool Instance { get; private set; }

        private void Awake()
        {
            // すでにインスタンスが存在する場合は破棄（シングルトンの重複を防ぐ）
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいで保持したい場合
        }
        
        public List<CardScriptableObject> cardpool = new List<CardScriptableObject>();
    }
}