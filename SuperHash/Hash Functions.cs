using HashLib;

namespace SuperHash
{
    internal class Hash_Functions
    {
        static IHash Jenkins3 = HashFactory.Hash32.CreateJenkins3();
        static IHash OneAtTime = HashFactory.Hash32.CreateOneAtTime();
        static IHash Murmur2 = HashFactory.Hash32.CreateMurmur2();
        static IHash PJW = HashFactory.Hash32.CreatePJW();
        static IHash FNW = HashFactory.Hash32.CreateFNV();


        public static void Initialize()
        {
            Jenkins3.Initialize();
            OneAtTime.Initialize();
            Murmur2.Initialize();
            PJW.Initialize();
            FNW.Initialize();
        }

        public static uint SuperHash(byte[] data)
        {
            uint hash = 0;
            for (int i = 0; i < data.Length; i++)
            {
                hash *= 255;
                hash += (uint)(data[i] << 12);
                hash ^= (hash << 10) | (hash >> 12);
                hash += data[i] ^ 4294967220;
            }
            return hash;
        }

        public static uint JenkinsHash(byte[] data)
        {
            return Jenkins3.ComputeBytes(data).GetUInt();
        }

        public static uint OneAtTimeHash(byte[] data)
        {
            return OneAtTime.ComputeBytes(data).GetUInt();
        }

        public static uint MurmurHash(byte[] data)
        {
            return Murmur2.ComputeBytes(data).GetUInt();
        }

        public static uint PJWHash(byte[] data)
        {
            return PJW.ComputeBytes(data).GetUInt();
        }

        public static uint FNWHash(byte[] data)
        {
            return FNW.ComputeBytes(data).GetUInt();
        }
    }
}
