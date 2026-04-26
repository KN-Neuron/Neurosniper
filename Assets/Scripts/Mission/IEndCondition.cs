namespace NeuroSniper.Mission
{
    public interface IEndCondition
    {
        bool IsConditionMet();
        string GetConditionDescription();
    }
}