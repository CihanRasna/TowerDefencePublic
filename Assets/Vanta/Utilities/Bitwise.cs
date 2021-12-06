namespace Vanta.Utilities
{

    public class Bitwise
    {

        public static int SetBitValue(int index, int data)
        {
            if (index < 0 || index > 31)
            {
                return data;
            }

            return data | (1 << index);
        }

        public static int ResetBitValue(int index, int data)
        {
            if (index < 0 || index > 31)
            {
                return data;
            }

            return data & ~(1 << index);
        }

        public static bool GetBitValue(int index, int data)
        {
            if (index < 0 || index > 31)
            {
                return false;
            }

            return (data & (1 << index)) != 0;
        }

    }

}