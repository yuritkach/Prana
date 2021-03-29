using Android.Content;

namespace Prana.src.infrared
{ 

    public class ConsumerIrManagerCompat:ConsumerIrManager {

        private readonly Android.Hardware.ConsumerIrManager service;

        public ConsumerIrManagerCompat(Context context)
        {
            service = (Android.Hardware.ConsumerIrManager) context.GetSystemService(Context.ConsumerIrService);
        }

    
        public override bool hasIrEmitter()
        {
            return service.HasIrEmitter;
        }

    
        public override void transmit(int carrierFrequency, int[] pattern)
        {
            service.Transmit(carrierFrequency, pattern);
        }

    
        public override Android.Hardware.ConsumerIrManager.CarrierFrequencyRange[] getCarrierFrequencies()
        {
            return service.GetCarrierFrequencies();
        }
    }
}