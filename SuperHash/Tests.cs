using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SuperHash
{
    public static class Tests
    {
        static LinkedList<Dictionary> dictionaries = new LinkedList<Dictionary>();
        static LinkedList<Function> functions = new LinkedList<Function>();
        static LinkedList<(Hashtable, string)> hashTables = new LinkedList<(Hashtable, string)>();

        public static void AddDictionaries(params (string path, string name, Encoding encoding)[] dicts)
        {
            foreach (var (path, name, encoding) in dicts)
            {
                var dict = new Dictionary(path, name, encoding);
                if (!dictionaries.Contains(dict))
                    dictionaries.AddLast(dict);
            }
        }

        public static void AddHashFunctions(params (Func<byte[], uint> function, string funcName)[] funcs)
        {
            foreach (var (function, funcName) in funcs)
            {
                Function item = new Function(function, funcName);
                if (!functions.Contains(item))
                    functions.AddLast(item);
            }
        }

        static LinkedList<string> currentDictionary = new LinkedList<string>();
        static void LoadDictionary(Dictionary dict)
        {
            currentDictionary.Clear();
            FileStream fs = new FileStream(dict.path, FileMode.Open);
            using (StreamReader sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                    currentDictionary.AddLast(sr.ReadLine());
                Console.WriteLine($"Словарь [{dict.name}] загружен. Количество слов: {currentDictionary.Count}\n");
            }
            fs.Close();
        }

        public static void Start()
        {
            foreach (var dict in dictionaries)
            {
                LoadDictionary(dict);
                foreach (var f in functions)
                {
                    long time;
                    LinkedList<(uint, string)> collisionsList;
                    uint[] counters;
                    int numberOfParts = 64;
                    (time, collisionsList, counters) = CreateHashTable(f, numberOfParts);
                    f.totalTime += time;
                    f.totalCollisions += collisionsList.Count;
                    f.totalUniformity += counters.Max() - counters.Min();

                    Console.WriteLine($"---{f.name}---");
                    Console.WriteLine($"Время: {time}");

                    double alpha = (double)(currentDictionary.Count - collisionsList.Count) / currentDictionary.Count,
                           beta = (double)collisionsList.Count / currentDictionary.Count;
                    Console.WriteLine($"Число коллизий: {collisionsList.Count}");
                    Console.WriteLine($"Показатель α: {alpha:F6}");
                    Console.WriteLine($"Показатель β: {beta:F6}");

                    /*for (int i = 0; i < numberOfParts; i++)
                    {
                        Console.WriteLine($"Часть {i + 1} - {counters[i]}");
                    }*/

                    Console.WriteLine($"Равномерность распределения: {counters.Max() - counters.Min()}");
                    Console.WriteLine("------------------\n");
                }
            }

            Console.WriteLine("\n\n\tИТОГ\n");
            foreach (var f in functions)
            {
                Console.WriteLine($" == {f.name} == ");
                Console.WriteLine($" Суммарное время: {f.totalTime} ms");
                Console.WriteLine($" Суммарное коллизии: {f.totalCollisions}");
                Console.WriteLine($" Суммарное равномерность: {f.totalUniformity}\n");
            }
        }

        static (long, LinkedList<(uint, string)>, uint[]) CreateHashTable(Function f, int numberOfParts)
        {
            Hashtable ht = new Hashtable();
            LinkedList<(uint, string)> collisionsList = new LinkedList<(uint, string)>();
            uint[] counters = new uint[numberOfParts];
            Stopwatch sw = new Stopwatch();

            uint k = (uint)(uint.MaxValue / numberOfParts);

            for (LinkedListNode<string> node = currentDictionary.First; node != null; node = node.Next)
            {
                string line = node.Value;
                sw.Start();
                uint hash = f.func(Encoding.UTF8.GetBytes(line));
                sw.Stop();
                if (!ht.Contains(hash)) ht.Add(hash, line);
                else collisionsList.AddLast((hash, line));
                ++counters[hash / k];
            }
            hashTables.AddLast((ht, f.name));
            return (sw.ElapsedMilliseconds, collisionsList, counters);
        }
    }
}
