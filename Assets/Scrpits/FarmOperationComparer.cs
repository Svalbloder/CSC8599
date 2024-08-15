using System.Collections.Generic;

public class FarmOperationComparer : IComparer<FarmOperation>
{
    public int Compare(FarmOperation x, FarmOperation y)
    {
        if (x == null || y == null)
        {
            return 0;
        }
        // 优先级越高，数字越小
        return y.Priority.CompareTo(x.Priority);
    }
}
