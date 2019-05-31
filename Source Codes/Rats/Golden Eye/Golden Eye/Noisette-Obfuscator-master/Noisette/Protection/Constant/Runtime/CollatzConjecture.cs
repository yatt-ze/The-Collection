using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisetteCore.Protection.Constant.Runtime
{
    public static class CollatzConjecture
    {
        //https://en.wikipedia.org/wiki/Collatz_conjecture

        //if it does not return 1 for every positive integer
        //then we've solved a huge mathematical problem

        public static int ConjetMe(int i)
        {
            while (i != 1)
            {
                if (i % 2 == 0)
                {
                    i = i / 2;
                }
                else
                {
                    i = (3 * i) + 1;
                }
            }
            return i;
        }
    }
}