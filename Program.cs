using System;
using ZARN.CLI;

namespace ZARN
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandHandler = new CommandHandler();
            commandHandler.HandleCommand(args);
        }
    }
}

