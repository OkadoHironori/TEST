namespace Xs.DioMonitor
{
    public class DoModel : DioModel
    {
        public override void Execute(bool onoff)
        {
            if (Controlable)
                if (null != Dio) 
                    Dio.SetDo(onoff, this.Index);
        }
    }
}
