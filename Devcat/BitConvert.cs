using System.Runtime.InteropServices;

namespace Devcat
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // BitConvert
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // author: netics@nexon.co.kr
    public class BitConvert
    {
        //================================================================================================================================
        // Enum <==> Int
        //================================================================================================================================
#if UNITY_2017_1_OR_NEWER
#if NET_4_6 || NET_STANDARD_2_0
        struct Shell<T>
            where T : struct
        {
            public int IntValue;
            public T Enum;
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public static int Enum32ToInt<T>(T e)
            where T : struct
        {
            Shell<T> s;
            s.Enum = e;
            unsafe
            {
                int* pi = &s.IntValue;
                pi += 1;
                return *pi;
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public static T IntToEnum32<T>(int value)
            where T : struct
        {
            var s = new Shell<T>();
            unsafe
            {
                int* pi = &s.IntValue;
                pi += 1;
                *pi = value;
            }
            return s.Enum;
        }
#else
        [StructLayout(LayoutKind.Explicit)]
        struct EnumUnion32<T>
            where T : struct
        {
            [FieldOffset(0)]
            public T Enum;

            [FieldOffset(0)]
            public int Int;
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public static int Enum32ToInt<T>(T e)
            where T : struct
        {
            var u = default(EnumUnion32<T>);
            u.Enum = e;
            return u.Int;
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public static T IntToEnum32<T>(int value)
            where T : struct
        {
            var u = default(EnumUnion32<T>);
            u.Int = value;
            return u.Enum;
        }
#endif
#else
        //--------------------------------------------------------------------------------------------------------------------------------
        public static int Enum32ToInt<T>(T e)
            where T : struct
        {
            return ValueCastTo<int>.From<T>(e);
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public static T IntToEnum32<T>(int value)
            where T : struct
        {
            return ValueCastTo<T>.From<int>(value);
        }
#endif
    }
}