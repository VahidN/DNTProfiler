using System;
using System.IO;
using System.Linq;
using DNTProfiler.Common.Models;
using ICSharpCode.NRefactory.CSharp;

namespace DNTProfiler.Infrastructure.StackTraces
{
    public static class MethodDeclarations
    {
        public static string GetMethodBody(string sourceFilePath, string methodName, int callingLine)
        {
            if (!File.Exists(sourceFilePath))
                return string.Format("{0} file not found.", Path.GetFileName(sourceFilePath));

            try
            {
                var parser = new CSharpParser();
                var syntaxTree = parser.Parse(File.ReadAllText(sourceFilePath));

                var result = syntaxTree.Descendants.OfType<MethodDeclaration>()
                    .FirstOrDefault(y => y.NameToken.Name == methodName && y.EndLocation.Line > callingLine);
                if (result != null)
                {
                    return result.ToString(FormattingOptionsFactory.CreateSharpDevelop()).Trim();
                }

                result = syntaxTree.Descendants.OfType<MethodDeclaration>()
                    .FirstOrDefault(y => y.NameToken.Name == methodName);
                if (result != null)
                {
                    return result.ToString(FormattingOptionsFactory.CreateSharpDevelop()).Trim();
                }
            }
            catch
            {
                return readLines(sourceFilePath, callingLine);
            }

            return readLines(sourceFilePath, callingLine);
        }

        public static object TryGetMethodBody(this object data)
        {
            var methodInfo = data as CallingMethodInfo;
            if (methodInfo == null) return data;
            return GetMethodBody(
                methodInfo.CallingFileFullName,
                methodInfo.CallingMethod,
                methodInfo.CallingLine);
        }

        private static string readLines(string sourceFilePath, int callingLine)
        {
            var lines = File.ReadAllLines(sourceFilePath);
            if (!lines.Any())
                return string.Empty;

            var codes = lines.Skip(callingLine - 5).Take(10);
            return codes.Aggregate((l1, l2) => l1.TrimEnd() + Environment.NewLine + l2.TrimEnd());
        }
    }
}