// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;

namespace DotNetDo.Internal
{
    /// <summary>
    /// For non-Windows platform consoles which understand the ANSI escape code sequences to represent color
    /// </summary>
    public class AnsiLogConsole : IConsole
    {
        private readonly StringBuilder _outputBuilder;
        private readonly IAnsiSystemConsole _systemConsole;

        public AnsiLogConsole(IAnsiSystemConsole systemConsole)
        {
            _outputBuilder = new StringBuilder();
            _systemConsole = systemConsole;
        }

        public void Write(string message, ConsoleColor? background, ConsoleColor? foreground)
        {
            // Order: backgroundcolor, foregroundcolor, Message, reset foregroundcolor, reset backgroundcolor
            if (background.HasValue)
            {
                _outputBuilder.Append(GetBackgroundColorEscapeCode(background.Value));
            }

            if (foreground.HasValue)
            {
                _outputBuilder.Append(GetForegroundColorEscapeCode(foreground.Value));
            }

            _outputBuilder.Append(message);

            if (foreground.HasValue)
            {
                _outputBuilder.Append("\x1B[39m"); // reset to default foreground color
            }

            if (background.HasValue)
            {
                _outputBuilder.Append("\x1B[49m"); // reset to the background color
            }
        }

        public void WriteLine(string message, ConsoleColor? background, ConsoleColor? foreground)
        {
            Write(message, background, foreground);
            _outputBuilder.AppendLine();
        }

        public void Flush()
        {
            _systemConsole.Write(_outputBuilder.ToString());
            _outputBuilder.Clear();
        }

        private static string GetForegroundColorEscapeCode(ConsoleColor color)
        {
            // Foreground colors are at 30-37 and 90-97, so add 30 to whatever we get back
            var code = 30 + GetColorEscapeCode(color);
            return $"\x1B[{code}m";
        }

        private static string GetBackgroundColorEscapeCode(ConsoleColor color)
        {
            // Foreground colors are at 40-47 and 100-107, so add 40 to whatever we get back
            var code = 40 + GetColorEscapeCode(color);
            return $"\x1B[{code}m";
        }

        private static int GetColorEscapeCode(ConsoleColor color)
        {
            switch (color)
            {
                case ConsoleColor.Black:
                    return 0;
                case ConsoleColor.Blue:
                    return 64;
                case ConsoleColor.Cyan:
                    return 66;
                case ConsoleColor.DarkBlue:
                    return 4;
                case ConsoleColor.DarkCyan:
                    return 6;
                case ConsoleColor.DarkGray:
                    return 60;
                case ConsoleColor.DarkGreen:
                    return 2;
                case ConsoleColor.DarkMagenta:
                    return 5;
                case ConsoleColor.DarkRed:
                    return 1;
                case ConsoleColor.DarkYellow:
                    return 3;
                case ConsoleColor.Gray:
                    return 7;
                case ConsoleColor.Green:
                    return 62;
                case ConsoleColor.Magenta:
                    return 65;
                case ConsoleColor.Red:
                    return 61;
                case ConsoleColor.White:
                    return 67;
                case ConsoleColor.Yellow:
                    return 63;
                default:
                    return 9;
            }
        }
    }
}
