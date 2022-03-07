namespace CharacterStateMachine
{
    public interface ICharacterStateObservable
    {
        void AddObserver(ICharacterStateObserver o);
        void RemoveObserver(ICharacterStateObserver o);
        void NotifyObservers(CharacterStateUpdatedInfo updatedDataInfo);
    }
}