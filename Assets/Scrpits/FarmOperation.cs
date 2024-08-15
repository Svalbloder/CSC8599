public class FarmOperation
{
    public FarmOperationType OperationType;
    public FarmLand TargetSeedPoint;
    public int Priority;

    public FarmOperation(FarmOperationType operationType, FarmLand targetSeedPoint, int priority)
    {
        OperationType = operationType;
        TargetSeedPoint = targetSeedPoint;
        Priority = priority;
    }
}
