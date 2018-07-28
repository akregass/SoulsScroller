using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsEngine {

    public enum ActorState {
        IDLE,
        MOVING,
        ATTACKING,
        BLOCKING
    }

    [System.Serializable]
    public class Stats
    {

        [SerializeField]
        float _baseHealth;
        public float BaseHealth
        {
            get { return _baseHealth; }
            set { _baseHealth = value; }
        }

        [SerializeField]
        float _baseStamina;
        public float BaseStamina
        {
            get { return _baseStamina; }
            set { _baseStamina = value; }
        }


        [SerializeField]
        float _health;
        public float Health
        {
            get { return _health; }
            set { _health = value; }
        }

        [SerializeField]
        float _stamina;
        public float Stamina
        {
            get { return _stamina; }
            set { _stamina = value; }
        }

        [SerializeField]
        float _moveSpeed;
        public float MoveSpeed
        {
            get { return _moveSpeed; }
            set { _moveSpeed = value; }
        }


        [SerializeField]
        float _vitality;
        public float Vitality
        {
            get { return _vitality; }
            set { _vitality = value; }
        }

        [SerializeField]
        float _endurance;
        public float Endurance
        {
            get { return _endurance; }
            set { _endurance = value; }
        }

    }

	public class GodManager : MonoBehaviour
    {
        public Player _player;
		public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

		public DialogManager dialogManager;
        
		public Camera[] cameras;

		public List<Actor> actors;
        public List<Actor> actorsActive;

        public Dictionary<string, string> worldState;

        public ItemDatabase ItemDB { get; set; }

		void Awake()
        {
            actors = new List<Actor>();
			dialogManager = GetComponent<DialogManager>();
            worldState = new Dictionary<string, string>();
            worldState.Add("Zone", "Ruins");
        }
	}
}