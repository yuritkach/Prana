using Android.Content;
using Android.OS;

namespace Prana.src.infrared
{

    public class ConsumerIrManager
    {

        public virtual bool hasIrEmitter()
        {
            return false;
        }

        public virtual void transmit(int carrierFrequency, int[] pattern)
        {
        }

        public virtual void transmit(IrCommand command)
        {
            transmit(command.frequency, command.pattern);
        }

        public virtual Android.Hardware.ConsumerIrManager.CarrierFrequencyRange[] getCarrierFrequencies()
        {
            return null;
        }

        public static ConsumerIrManager getSupportConsumerIrManager(Context context)
        {
            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Kitkat)
            {
                return new ConsumerIrManagerCompat(context);
            }

            ConsumerIrManager consumerIrManagerXiaomi = ConsumerIrManagerXiaomi.getSupportConsumerIrManager(context);

            if (consumerIrManagerXiaomi != null)
                return consumerIrManagerXiaomi;

            return new ConsumerIrManager();
        }

    }

}