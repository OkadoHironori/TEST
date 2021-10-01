namespace Xs.DioMonitor
{
    public class DiModel : DioModel
    {
        public override void Execute(bool onoff)
        {
            if(!ReadOnly && Controlable)
                if (null != Dio) 
                    Dio.SetDi(onoff, this.Index);
        }
    }
}
