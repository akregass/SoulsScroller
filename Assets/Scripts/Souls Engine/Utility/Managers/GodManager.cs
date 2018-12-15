using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MEC;

namespace SoulsEngine {

    public class GodManager : MonoBehaviour
    {
        public static GodManager godManager;

        private static Player _player;
        public static Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        private static DialogManager _dialogManager;
        public static DialogManager DialogManager
        {
            get { return _dialogManager; }
            set { _dialogManager = value; }
        }

        public static Camera[] cameras;

        public static List<Actor> actors;
        public static List<Actor> actorsActive;

        public static List<Entity> entities;

        public static ItemDatabase ItemDB { get; set; }

        void Awake()
        {
            entities = new List<Entity>();
            actors = new List<Actor>();
            actorsActive = new List<Actor>();

            DialogManager = GetComponent<DialogManager>();
        }

        public static void DeathSplash()
        {
            Debug.Log("YOU DIED");
        }

        public static void RegisterActor(Actor actor)
        {
            if (!actors.Contains(actor))
            {
                actors.Add(actor);
            }
        }

        public static void UnregisterActor(Actor actor)
        {
            if (actors.Contains(actor))
            {
                actors.Remove(actor);
            }
        }
        
        public static void SubscribeActor(Actor actor)
        {
            if (!actorsActive.Contains(actor))
            {
                actorsActive.Add(actor);
            }
        }

        public static void UnsubscribeActor(Actor actor)
        {
            if (actorsActive.Contains(actor))
            {
                actorsActive.Remove(actor);
            }
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