using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ardoise
{
    class Program
    {
        static void Main(string[] args)
        {
            ardoise monArdoise = new ardoise();

            monArdoise.entree_ardoise();

            monArdoise.gestion_ardoise();
        }
    }
}
