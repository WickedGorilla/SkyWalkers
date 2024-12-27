using System.Collections.Generic;

namespace UI.Views
{
    public static class BitmaskHelper
    {
        public static int GenerateBitmask(IEnumerable<int> indexes)
        {
            int mask = 0;
            
            foreach (var index in indexes) 
                mask |= 1 << index;

            return mask;
        }

        public static bool CheckContainsInBitmask(int mask, int index) 
            => (mask & (1 << index)) != 0;
    }
}