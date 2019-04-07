using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    bool Run(PlayerBase player);
}
