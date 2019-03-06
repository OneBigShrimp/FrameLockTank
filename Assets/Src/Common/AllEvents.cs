using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class EntityDieEventArgs : EventArgs
{
    public int KillerEntityKey;
    public int DeadEnityKey;
}


public class EntityDestroyEventArgs : EventArgs
{
    public readonly int EntityKey;
    public EntityDestroyEventArgs(int entityKey)
    {
        this.EntityKey = entityKey;
    }
}
