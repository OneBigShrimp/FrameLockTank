using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void MoveOverCallback(bool isInterrupt);

public class LogicMove : LogicComponentBase
{
    List<MoveCtrlBase> _allMoves;

    public LogicMove(LogicEntity entity)
    : base(entity)
    {
        _allMoves = new List<MoveCtrlBase>(0);
    }

    public override void LogicTick(int ms)
    {
        for (int i = 0; i < _allMoves.Count; i++)
        {
            if (_allMoves[i].TickMove(this, ms))
            {
                _allMoves.RemoveAt(i--);
            }
        }
    }

    public override void OnComponentDestroy()
    {
        for (int i = 0; i < _allMoves.Count; i++)
        {
            _allMoves[i].InvokeCallback(true);
        }
    }

    public void MoveOneStep(SafeV3 step)
    {
        this.HostEntity.LogicTran.position += step;
    }

    public void AddMove(int targetId, SafeFloat speed, MoveOverCallback overCall)
    {
        MoveByTarget mt = new MoveByTarget(targetId, overCall, speed);
        mt.Init();
        _allMoves.Add(mt);
    }

    public void AddMove(SafeV3 dir, int lifeTime, SafeFloat speed, MoveOverCallback overCall)
    {
        MoveByDir md = new MoveByDir(dir, lifeTime, overCall, speed);
        md.Init();
        _allMoves.Add(md);
    }


    private void _Move(SafeV3 dir, SafeFloat speed)
    {
        SafeV3 offset = speed * dir;
        MoveOneStep(offset);
    }

    abstract class MoveCtrlBase
    {
        /// <summary>
        /// 毫米/毫秒(也就是米/秒)
        /// </summary>
        protected SafeFloat speed { get; private set; }

        protected MoveOverCallback overCall { get; private set; }


        public MoveCtrlBase(SafeFloat speed, MoveOverCallback overCall)
        {
            this.speed = speed;
            this.overCall = overCall;
        }

        public virtual void Init() { }

        protected void MoveByDir(int delta, SafeV3 dir)
        {

        }

        public void InvokeCallback(bool isInterrupt)
        {
            if (this.overCall != null)
            {
                this.overCall(isInterrupt);
                this.overCall = null;
            }
        }

        public abstract bool TickMove(LogicMove lm, int ms);

    }

    class MoveByTarget : MoveCtrlBase
    {
        int _targetId;

        public MoveByTarget(int targetId, MoveOverCallback overCall, SafeFloat speed)
            : base(speed, overCall)
        {
            this._targetId = targetId;
        }


        public override bool TickMove(LogicMove lm, int ms)
        {
            LogicEntity targetEntity = EntityManager.Instance.GetEntity(this._targetId);
            if (targetEntity == null)
            {
                this.InvokeCallback(true);
                return true;
            }
            LogicTransform targetTran = targetEntity.GetComponent<LogicTransform>();
            SafeV3 dir = targetTran.position - lm.HostEntity.LogicTran.position;
            if (dir.magnitude * 1000 < speed * ms)
            {
                lm.HostEntity.LogicTran.position = targetTran.position;
                this.InvokeCallback(false);
                return true;
            }
            dir.SetNormalize();

            lm.MoveOneStep(ms * speed * 0.001f * dir);
            return false;
        }
    }

    class MoveByDir : MoveCtrlBase
    {
        SafeV3 _dir;
        /// <summary>
        /// 毫秒
        /// </summary>
        int _lifeRemain;
        public MoveByDir(SafeV3 dir, int life, MoveOverCallback overCall, SafeFloat speed)
            : base(speed, overCall)
        {
            this._dir = dir;
            this._lifeRemain = life;
        }

        public override bool TickMove(LogicMove lm,int ms)
        {
            int useDelta = this._lifeRemain < ms ? this._lifeRemain : ms;
            lm.MoveOneStep(useDelta * speed * 0.001f * this._dir);
            this._lifeRemain -= useDelta;
            return this._lifeRemain == 0;
        }
    }

}
