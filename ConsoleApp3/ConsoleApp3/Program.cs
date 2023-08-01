using System;
using System.Net;
using System.Net.Http;

namespace SampleConsoleApp
class Program
{
    static async Task Main(string[] args)
    {
        HttpClient client = new HttpClient();
        Program program = new Program();
        await program.GetTodoItems();
    }
    private asyanc Task GetTodoItems()
    {
        string response = await client.GetStringAsync("")
            Console.WriteLine(response);
    }
}





   