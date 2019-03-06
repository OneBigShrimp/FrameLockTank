using System;
using System.Collections.Generic;


class AutoKey
{
    private static int _curKey = 0;
    public static int Next
    {
        get
        {
            return ++_curKey;
        }
    }
}
