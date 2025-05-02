namespace Stella.Conditions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ConditionAttribute<TCondition> : Attribute, ITypedCondition
    where TCondition : ICondition
{
    public Type Condition => typeof(TCondition);
}