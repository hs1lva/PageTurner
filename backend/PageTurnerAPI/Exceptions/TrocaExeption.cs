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

        }
    }

    public class TrocaNaoEncontradaException : TrocaException
    {
        public TrocaNaoEncontradaException(List<Troca> lista) : base("Troca não encontrada")
        {
            if (lista.Count == 0)
            {
                // logging
                Console.WriteLine("Troca não encontrada" + ", lista vazia " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
        }
    }

    public class LivroNaoEncontradoException : TrocaException
    {
        public LivroNaoEncontradoException(List<Utilizador> lista) : base("Livro não existe em nenhuma estante de troca")
        {
            if (lista.Count == 0)
            {
                // logging
                Console.WriteLine("Livro não existe em nenhuma estante de troca" + ", lista vazia " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
        }
    }
}