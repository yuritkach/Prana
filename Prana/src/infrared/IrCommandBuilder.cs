using Prana.src.infrared;
using System.Collections;
using System.Collections.Generic;

namespace Prana.src.infrared
{

    public class IrCommandBuilder: ISequenceDefinition
    {

        public static readonly long TOP_BIT_32 = 0x1L << 31;
        public static readonly long TOP_BIT_64 = 0x1L << 63;

        private readonly int frequency;
        private readonly List<int> buffer;
        private bool? lastMark;

        public static IrCommandBuilder irCommandBuilder(int frequency)
        {
            return new IrCommandBuilder(frequency);
        }

        private IrCommandBuilder(int frequencyKHz) {
            this.frequency = frequencyKHz;

            buffer = new List<int>();

            lastMark = null;
        }

        private IrCommandBuilder appendSymbol(bool mark, int interval) {
            if (lastMark == null || lastMark != mark) {
                buffer.Add(interval);
                lastMark = mark;
            } else {
                int lastIndex = buffer.Count - 1;
                buffer[lastIndex] = buffer[lastIndex + interval];
            }

            return this;
        }

        public IrCommandBuilder mark(int interval) {
            return appendSymbol(true, interval);
        }

        public IrCommandBuilder space(int interval) {
            return appendSymbol(false, interval);
        }

        public IrCommandBuilder pair(int on, int off) {
            return mark(on).space(off);
        }

        public IrCommandBuilder reversePair(int off, int on) {
            return space(off).mark(on);
        }

        public IrCommandBuilder delay(int ms) {
            return space(ms * frequency / 1000);
        }

        public IrCommandBuilder sequence(ISequenceDefinition definition, int length, int data) {
            return sequence(definition, TOP_BIT_32, length, data);
        }

        public IrCommandBuilder sequence(ISequenceDefinition definition, int length, long data) {
            return sequence(definition, TOP_BIT_64, length, data);
        }

        public IrCommandBuilder sequence(ISequenceDefinition definition, long topBit, int length, long data) {
            for (int index = 0; index < length; index++) {
                if ((data & topBit) != 0) {
                    definition.One(this, index);
                } else {
                    definition.Zero(this, index);
                }

                data <<= 1;
            }

            return this;
        }

        public IrCommand build() {
            return new IrCommand(getFrequency(), buildSequence());
        }

        public int[] buildSequence() {
            return buildRawSequence(buffer);
        }

        public int getFrequency() {
            return frequency;
        }

        public List<int> getBuffer() {
            return buffer;
        }

        public static ISequenceDefinition simpleSequence(int oneMark, int oneSpace, int zeroMark, int zeroSpace) {
            return new MySequenceDefinition(oneMark,oneSpace,zeroMark,zeroSpace);
            }

        
        public static int[] buildRawSequence(int[] rawData)
        {
            return rawData;
        }


        public static int[] buildRawSequence(List<int> buffer)
        {
            int[] result = new int[buffer.Count];

            for (int i = 0; i < buffer.Count; i++)
            {
                result[i] = buffer[i];
            }

            return result;
        }



        public static int[] buildRawSequence(IEnumerable<int> dataStream)
        {
            if (dataStream is IList) 
            {
                return buildRawSequence((List<int>) dataStream);
            }

            List<int> buffer = new List<int>();
            foreach (int data in dataStream)
            {
                buffer.Add(data);
            }

            return buildRawSequence(buffer);
        }

    }

    public class MySequenceDefinition : ISequenceDefinition
    {
        protected int oneMark;
        protected int oneSpace;
        protected int zeroMark;
        protected int zeroSpace;

        public MySequenceDefinition(int oneMark, int oneSpace, int zeroMark, int zeroSpace)
        {
            this.oneMark = oneMark;
            this.oneSpace = oneSpace;
            this.zeroMark = zeroMark;
            this.zeroSpace = zeroSpace;
        }

        public override void One(IrCommandBuilder builder, int index)
        {
            builder.pair(oneMark, oneSpace);
        }

        public override void Zero(IrCommandBuilder builder, int index)
        {
            builder.pair(zeroMark, zeroSpace);
        }
    }

    public class ISequenceDefinition
    {
        public virtual void One(IrCommandBuilder builder, int index) { }
        public virtual void Zero(IrCommandBuilder builder, int index) { }
    }
}
