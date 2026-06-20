using System;
using System.Collections.Generic;

namespace _Project.Scripts.Utils
{
    public class ListUtils
    {
        public static void Shuffle<T>(IList<T> list)
        {
            var random = new Random();
            for (int index = list.Count - 1; index >= 1; index--)
            {
                int other = random.Next(0, index + 1);
                (list[index], list[other]) = (list[other], list[index]);
            }
        }
    }
}