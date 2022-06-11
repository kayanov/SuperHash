using System.Text;

namespace SuperHash
{
    internal class Program
    {
        static void Main()
        {
            Tests.AddDictionaries(
                (@"../../russian_1.txt", "Первый русский словарь", Encoding.UTF8),
                (@"../../russian_2.txt", "Второй русский словарь", Encoding.UTF8),
                (@"../../russian_3.txt", "Третий русский словарь", Encoding.UTF8),
                (@"../../russian_4.txt", "Четвертый русский словарь", Encoding.UTF8),

                (@"../../english_1.txt", "Первый английский словарь", Encoding.UTF8),
                (@"../../russian_4.txt", "Второй английский словарь", Encoding.UTF8)
                );

            Tests.AddHashFunctions(
                (Hash_Functions.SuperHash, "SuperHash"),
                (Hash_Functions.JenkinsHash, "JenkinsHash"),
                (Hash_Functions.OneAtTimeHash, "OneAtTimeHash"),
                (Hash_Functions.MurmurHash, "MurmurHash"),
                (Hash_Functions.PJWHash, "PJWHash"),
                (Hash_Functions.FNWHash, "FNWHash")
                );

            Hash_Functions.Initialize();
            Tests.Start();
        }
    }
}
