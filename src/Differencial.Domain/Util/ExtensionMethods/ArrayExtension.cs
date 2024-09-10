using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain.Util.ExtensionMethods
{
    public static class ArrayExtension
    {
        public static void ForEach<T>(this T[] array, Action<T> action) => Array.ForEach(array, action);

    }
}
