using System.Collections;
using System.Collections.Generic;
using DamageReceiving;
using UnityEngine;

namespace CharacterStateMachine
{
    public class CharacterStateMachine : DamageReceiving.IHealthObserver, ICharacterStateObservable
    {
        private CharacterState _currentState;

        private List<ICharacterStateObserver> _observers = new List<ICharacterStateObserver>();

        public CharacterStateMachine(CharacterState startState)
        {
            _currentState = startState;
        }

        public void AddObserver(ICharacterStateObserver o)
        {
            _observers.Add(o);
        }

        public void NotifyObservers(CharacterStateUpdatedInfo updatedDataInfo)
        {
            foreach (var observer in _observers)
            {
                observer.Update(updatedDataInfo);
            }
        }

        public void RemoveObserver(ICharacterStateObserver o)
        {
            _observers.Remove(o);
        }

        public void Update(HealthStorageUpdateInfo updatedDataInfo)
        {
            var currentHealth = updatedDataInfo.PrevValue + updatedDataInfo.Delta;

            if (currentHealth <= 0)
            {
                //ChangeState(CharacterState.Die);
            }
            else if (updatedDataInfo.PrevValue == 0 && currentHealth > 0)
            {
                //ChangeState(CharacterState.Revival);
            }
        }

        public void ChangeState(CharacterState newState)
        {
            CharacterStateUpdatedInfo updatedDataInfo = new CharacterStateUpdatedInfo()
            {
                PrevState = _currentState,
                CurrentState = newState
            };

            _currentState = newState;

            NotifyObservers(updatedDataInfo);
        }
    }
}
