using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace PageTurnerAPI.Exceptions
{
public class TrocaException : Exception
{
    public TrocaException() { }
    public TrocaException(string message) : base(message) { }
    public TrocaException(string message, Exception inner) : base(message, inner) { }
    public TrocaException(List<Utilizador> lista) : base() {
        if (lista.Count == 0)
        {
            throw new TrocaException("Livro não existe em nenhuma estante de troca");
        }
        Console.WriteLine("Livro não existe em nenhuma estante de troca");
    }

    public static void TrocaNaoEncontradaException(List<Utilizador> lista)
    {
        if (lista.Count == 0)
        {
            throw new TrocaException("Livro não existe em nenhuma estante de troca");
        }
    }
}

}