// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace DotNetDo.Internal
{
    public class WindowsLogConsole : IConsole
    {
        public void Write(string message, ConsoleColor? background, ConsoleColor? foreground)
        {
            if (background.HasValue)
            {
                System.Console.BackgroundColor = background.Value;
            }

            if (foreground.HasValue)
            {
                System.Console.ForegroundColor = foreground.Value;
            }

            System.Console.Write(message);

            System.Console.ResetColor();
        }

        public void WriteLine(string message, ConsoleColor? background, ConsoleColor? foreground)
        {
            Write(message + Environment.NewLine, background, foreground);
        }

        public void Flush()
        {
            // No action required as for every write, data is sent directly to the console
            // output stream
        }
    }
}
