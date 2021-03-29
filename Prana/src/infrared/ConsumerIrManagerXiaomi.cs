using Android.Content;
using Java.Lang;
using Java.Lang.Reflect;
using System;
using System.Reflection;

namespace Prana.src.infrared
{

    public delegate void WriteIrSendMethod(object service, string code);

    public class ConsumerIrManagerXiaomi:ConsumerIrManager
    {
        

        public static readonly string IRDA_SERVICE = "irda";

        public static readonly int MICRO_SECONDS_IN_A_SECOND = 1000000;

        public static ConsumerIrManagerXiaomi getIrdaManager(Context applicationContext)
        {
            object irdaService = applicationContext.GetSystemService(IRDA_SERVICE);

            if (irdaService == null)
                return null;

            return new ConsumerIrManagerXiaomi(irdaService);
        }

        private readonly object irdaService;
        private WriteIrSendMethod writeIrSendMethod;

        private ConsumerIrManagerXiaomi(object irdaService)
        {
            this.irdaService = irdaService;

            Type irdaServiceClass = irdaService.GetType();

            object reflectedMethod;

            try
            {
                reflectedMethod = irdaServiceClass.GetMethod("write_irsend");
            } 
            catch (NoSuchMethodException e) 
            {
                e.PrintStackTrace();
                reflectedMethod = null;
            }
            writeIrSendMethod = (WriteIrSendMethod) reflectedMethod;
        }

        private void rawWrite(string code)
        {
            try
            {
                writeIrSendMethod(irdaService, code);
            }
            catch (IllegalAccessException e)
            {
                e.PrintStackTrace();
            }
            catch (InvocationTargetException e)
            {
                e.PrintStackTrace();
            }
        }

    
        public override bool hasIrEmitter() {
            return writeIrSendMethod != null;
        }

    
        public override void transmit(int carrierFrequency, int[] pattern)
        {
            if (!hasIrEmitter())
                return;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(carrierFrequency);
            foreach (int bit in pattern)
            {
                stringBuilder.Append(',');
                stringBuilder.Append(bit * carrierFrequency / MICRO_SECONDS_IN_A_SECOND);
            }
            rawWrite(stringBuilder.ToString());
        }

    
        public override Android.Hardware.ConsumerIrManager.CarrierFrequencyRange[] getCarrierFrequencies()
        {
            return null; // 36khz - 40khz
        }
    }
}