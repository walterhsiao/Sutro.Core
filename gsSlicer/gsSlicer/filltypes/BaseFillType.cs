﻿namespace gs.FillTypes
{
    public abstract class BaseFillType : IFillType
    {
        public virtual double AdjustVolume(double volume)
        {
            return volume;
        }

        public abstract string GetLabel();

        public virtual double ModifySpeed(double speed, SchedulerSpeedHint speedHint)
        {
            return speed;
        }

        public virtual bool IsEntryLocationSpecified()
        {
            return false;
        }

        public virtual bool IsPartShell()
        {
            return false;
        }
    }
}