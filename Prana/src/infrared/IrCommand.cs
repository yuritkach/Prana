//import static com.github.timnew.androidinfrared.IrCommandBuilder.irCommandBuilder;
//import static com.github.timnew.androidinfrared.IrCommandBuilder.simpleSequence;

namespace Prana.src.infrared
{

    public class IrCommand
    {

        public readonly int frequency;
        public readonly int[] pattern;

        public IrCommand(int frequency, int[] pattern)
        {
            this.frequency = frequency;
            this.pattern = pattern;
        }

        public static class NEC
        {

            private static readonly int FREQUENCY = 38028;  // T = 26.296 us
            private static readonly int HDR_MARK = 342;
            private static readonly int HDR_SPACE = 171;
            private static readonly int BIT_MARK = 21;
            private static readonly int ONE_SPACE = 60;
            private static readonly int ZERO_SPACE = 21;


            private static readonly ISequenceDefinition SEQUENCE_DEFINITION = IrCommandBuilder.simpleSequence(BIT_MARK, ONE_SPACE, BIT_MARK, ZERO_SPACE);

            public static IrCommand BuildNEC(int bitCount, int data)
            {
                return IrCommandBuilder.irCommandBuilder(FREQUENCY)
                        .pair(HDR_MARK, HDR_SPACE)
                        .sequence(SEQUENCE_DEFINITION, bitCount, data)
                        .mark(BIT_MARK)
                        .build();
            }
        }
    }
}