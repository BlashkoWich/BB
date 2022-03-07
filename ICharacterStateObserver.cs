namespace CharacterStateMachine
{
    public interface ICharacterStateObserver
    {
        void Update(CharacterStateUpdatedInfo updatedDataInfo);
    }
}