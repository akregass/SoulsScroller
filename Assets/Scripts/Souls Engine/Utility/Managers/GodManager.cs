using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MEC;

namespace SoulsEngine {

    public class GodManager : MonoBehaviour
    {
        public Player _player;
        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        private DialogManager _dialogManager;
        public DialogManager DialogManager
        {
            get { return _dialogManager; }
            set { _dialogManager = value; }
        }

        private QuestManager _questManager;
        public QuestManager QuestManager
        {
            get { return _questManager; }
            set { _questManager = value; }
        }

        public Camera[] cameras;

        public List<Actor> actors;
        public List<Actor> actorsActive;

        public List<Entity> entities;

        public ItemDatabase ItemDB { get; set; }
        public GameObject inventoryUI;

        void Awake()
        {
            entities = new List<Entity>();
            actors = new List<Actor>();
            actorsActive = new List<Actor>();

            DialogManager = GetComponent<DialogManager>();
            QuestManager = GetComponent<QuestManager>();

            //inventoryUI = GameObject.FindGameObjectWithTag("Inventory UI");
        }

        public static void DeathSplash()
        {
            Debug.Log("YOU DIED");
        }

        #region SCENE MANAGEMENT

        public static void RestartScene()
        {
            Timing.RunCoroutine(_RestartScene(0));
        }

        public static void RestartScene(Action[] actions)
        {
            for (int i = 0; i < actions.Length; i++)
                actions[i]();

            Timing.RunCoroutine(_RestartScene(0));
        }

        public static void RestartScene(float delay)
        {
            Timing.RunCoroutine(_RestartScene(delay));
        }
        
        public static void RestartScene(Action[] actions, float delay)
        {
            for (int i = 0; i < actions.Length; i++)
                actions[i]();

            Timing.RunCoroutine(_RestartScene(delay));
        }
        
        static IEnumerator<float> _RestartScene(float delay)
        {
            if(delay > 0f)
                yield return Timing.WaitForSeconds(delay);

            SceneManager.LoadScene(0);
        }

        #endregion
    }
}