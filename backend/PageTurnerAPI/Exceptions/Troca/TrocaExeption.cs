using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace PageTurnerAPI.Exceptions
{
    public class TrocaException : Exception
    {
        // private List<Utilizador> lista;
        // Livro não existe em nenhuma estante de troca
        public TrocaException() : base("Erro!!! ") { 
            System.Console.WriteLine("Passa na TrocaException");
        }
        
    }
}

