using UnityEngine;

namespace NeuroSniper.Mission {

    public abstract class EndCondition : MonoBehaviour, IEndCondition
    {
        [SerializeField] protected string conditionDescription;

        public abstract bool IsConditionMet();

        public virtual string GetConditionDescription()
        {
            return conditionDescription;
        }
    }
}