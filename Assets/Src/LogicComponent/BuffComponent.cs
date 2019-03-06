using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BuffComponent : LogicComponentBase
{
    List<Buff> allBuffs = new List<Buff>();

    public BuffComponent(LogicEntity entity) : base(entity)
    {

    }
}

public class Buff
{

}