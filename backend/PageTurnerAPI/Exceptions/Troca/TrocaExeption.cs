using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace PageTurnerAPI.Exceptions
{
    public class TrocaException : Exception
    {
        public TrocaException(string message) : base(message + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
        {
            // logging
            Console.WriteLine("Registo funciona (TrocaException)");
            Console.WriteLine(message + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        }

        public static void TrocaNaoEncontradaException(List<Utilizador> lista)
        {
            if (lista.Count == 0)
            {
                // logging
                // Console.WriteLine("Livro não existe em nenhuma estante de troca" + ", lista vazia " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                throw new TrocaException("Livro não existe em nenhuma estante de troca");
            }
        }
    }


}