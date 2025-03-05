using System;
using UniRx;
using UnityEngine;
using UpdateSys;

namespace Core.Characters.Player
{
    public class PlayerUpdateReceiver : MonoBehaviour, IUpdatable
    {
        private ReactiveCommand _onReceivedUpdateCall = new ReactiveCommand();
        
        public IObservable<Unit> OnReceivedUpdateCall => _onReceivedUpdateCall;

        private void Awake()
        {
            this.StartUpdate();
        }

        public void OnSystemUpdate(float deltaTime)
        {
            _onReceivedUpdateCall.Execute();
        }
    }
}