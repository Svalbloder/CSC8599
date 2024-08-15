using System.Collections.Generic;

public class FarmOperationComparer : IComparer<FarmOperation>
{
    public int Compare(FarmOperation x, FarmOperation y)
    {
        if (x == null || y == null)
        {
            return 0;
        }
        // ���ȼ�Խ�ߣ�����ԽС
        return y.Priority.CompareTo(x.Priority);
    }
}
