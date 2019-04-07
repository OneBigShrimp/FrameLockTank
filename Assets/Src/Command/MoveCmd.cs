using System;


public class MoveCmd : ICommand
{
    public int DirX;
    public int DirZ;
    public bool Run(PlayerBase player)
    {
        MoveStateArgs args = new MoveStateArgs() { DirX = this.DirX * 0.01f, DirZ = this.DirZ * 0.01f };
        LogicEntity entity = player.Entity;
        if (entity == null)
        {
            return false;
        }
        StateMachine sm = entity.GetComponent<StateMachine>();
        return sm.TryChangeState(EntityState.Move, args);
    }
}

