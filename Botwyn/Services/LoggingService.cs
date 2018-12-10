using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace Botwyn.Services
{
    public sealed class LoggingService
    {
        public static void Log(string source, LogSeverity severity, string message, Exception exception = null)
        {
            if (severity.Equals(null))
            {
                severity = LogSeverity.Warning;
            }
            Append($"{GetSeverityString(severity)}", GetConsoleColor(severity));
            Append($" [{Minify(source)}] ", ConsoleColor.DarkGray);

            if (!string.IsNullOrWhiteSpace(message))
                Append($"{message}\n", ConsoleColor.White);
            else if (exception == null)
            {
                Append("Uknown Exception. Exception Returned Null.\n", ConsoleColor.DarkRed);
            }
            else if (exception.Message == null)
                Append($"Unknownk \n{exception.StackTrace}\n", GetConsoleColor(severity));
            else
                Append($"{exception.Message ?? "Unknownk"}\n{exception.StackTrace ?? "Unknown"}\n", GetConsoleColor(severity));
        }

        public static void LogCritical(string source, string message, Exception exc = null)
            => Log(source, LogSeverity.Critical, message, exc);

        public static void LogInformation(string source, string message)
            => Log(source, LogSeverity.Info, message);

        private static void Append(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
        }

        private static string Minify(string source)
        {
            switch (source.ToLower())
            {
                case "discord":
                    return "DISCD";
                case "victoria":
                    return "VICTR";
                case "audio":
                    return "AUDIO";
                case "admin":
                    return "ADMIN";
                case "gateway":
                    return "GTWAY";
                case "blacklist":
                    return "BLAKL";
                case "lavanode_0_socket":
                    return "LAVAS";
                case "lavanode_0":
                    return "LAVA#";
                case "bot":
                    return "BOTWN";
                  default:
                    return "UNKN";
            }
        }

        private static string GetSeverityString(LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Critical:
                    return "CRIT";
                case LogSeverity.Debug:
                    return "DBUG";
                case LogSeverity.Error:
                    return "EROR";
                case LogSeverity.Info:
                    return "INFO";
                case LogSeverity.Verbose:
                    return "VERB";
                case LogSeverity.Warning:
                    return "WARN";
                default: return "UNKN";
            }
        }

        private static ConsoleColor GetConsoleColor(LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Critical:
                    return ConsoleColor.Red;
                case LogSeverity.Debug:
                    return ConsoleColor.Magenta;
                case LogSeverity.Error:
                    return ConsoleColor.DarkRed;
                case LogSeverity.Info:
                    return ConsoleColor.Green;
                case LogSeverity.Verbose:
                    return ConsoleColor.DarkCyan;
                case LogSeverity.Warning:
                    return ConsoleColor.Yellow;
                default: return ConsoleColor.White;
            }
        }
    }
}

