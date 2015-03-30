using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Toolkit;
using DNTProfiler.Common.WebToolkit;

namespace DNTProfiler.Common.Logger
{
    public class CallingMethod
    {
        private readonly SortedSet<string> _assembliesToExclude = new SortedSet<string>
                {
                    typeof(CallingMethod).Assembly.GetName().Name,
                    "Anonymously Hosted DynamicMethods Assembly",
                    "System.Core",
                    "System.Data",
                    "System.Data.Entity",
                    "System.Data.Linq",
                    "System.Web",
                    "System.Web.Mvc",
                    "System.Windows.Forms",
                    "mscorlib",
                    "EntityFramework",
                    "StructureMap",
                    "NHibernate"
                };

        readonly SortedSet<string> _methodsToExclude = new SortedSet<string>
                {
                    "lambda_method",
                    ".ctor",
                    "System.Web.HttpApplication.IExecutionStep.Execute"
                };

        private readonly SortedSet<Type> _typesToExclude =
            new SortedSet<Type>(new TypeComparer()) { typeof(CallingMethod) };

        public SortedSet<string> AssembliesToExclude
        {
            set
            {
                if (value == null)
                    return;

                foreach (var item in value)
                {
                    _assembliesToExclude.Add(item);
                }
            }
            get { return _assembliesToExclude; }
        }

        public SortedSet<string> MethodsToExclude
        {
            set
            {
                if (value == null)
                    return;

                foreach (var item in value)
                {
                    _methodsToExclude.Add(item);
                }
            }
            get { return _methodsToExclude; }
        }

        public SortedSet<Type> TypesToExclude
        {
            set
            {
                if (value == null)
                    return;

                foreach (var item in value)
                {
                    _typesToExclude.Add(item);
                }
            }
            get { return _typesToExclude; }
        }

        public CallingMethodStackTrace GetCallingMethodInfo(bool onlyIncludeInfoWithFileLine = false)
        {
            var results = new CallingMethodStackTrace();

            var stackTrace = new StackTrace(true);
            var frameCount = stackTrace.FrameCount;

            var info = new StringBuilder();
            var prefix = "-- ";
            for (var i = frameCount - 1; i >= 0; i--)
            {
                var sf = stackTrace.GetFrame(i);
                var methodInfo = getStackFrameInfo(sf, onlyIncludeInfoWithFileLine);
                if (methodInfo == null || string.IsNullOrWhiteSpace(methodInfo.StackTrace))
                    continue;

                info.AppendLine(prefix + methodInfo.StackTrace);
                prefix = "-" + prefix;

                results.CallingMethodInfoList.Add(methodInfo);
            }

            var stackTraceString = info.ToString().Trim();
            if (string.IsNullOrWhiteSpace(stackTraceString))
            {
                stackTraceString = "no-info";
            }
            results.StackTraceHash = stackTraceString.ComputeHash();

            return results;
        }

        private static MethodBase getRealMethodFromAsyncMethod(MethodBase methodBase)
        {
            var generatedType = methodBase.DeclaringType;
            if (generatedType == null)
                return methodBase;

            var originalType = generatedType.DeclaringType;
            if (originalType == null)
                return methodBase;

            foreach (var methodInfo in originalType.GetMethods())
            {
                if (methodInfo.GetCustomAttributes(false)
                              .Any(attr => attr.GetType().FullName ==
                                  "System.Runtime.CompilerServices.AsyncStateMachineAttribute"))
                {
                    return methodInfo;
                }
            }

            return methodBase;
        }

        private static bool isMicrosoftType(MethodBase method)
        {
            if (method.ReflectedType == null)
                return false;

            return method.ReflectedType.FullName.StartsWith("System.") ||
                   method.ReflectedType.FullName.StartsWith("Microsoft.");
        }

        private static bool isTempFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            var aspnetTemporaryFilesFolder = HttpContextUtils.GetAspnetTemporaryFilesFolder();
            return !string.IsNullOrWhiteSpace(aspnetTemporaryFilesFolder)
                && path.StartsWith(aspnetTemporaryFilesFolder, StringComparison.OrdinalIgnoreCase);
        }

        private CallingMethodInfo getStackFrameInfo(StackFrame stackFrame, bool onlyIncludeInfoWithFileLine)
        {
            var methodInfo = new CallingMethodInfo { StackTrace = string.Empty };

            if (stackFrame == null)
                return null;

            var method = stackFrame.GetMethod();
            if (method == null)
                return null;

            method = getRealMethodFromAsyncMethod(method);

            var type = method.ReflectedType;
            if (type == null)
                return null;

            var assemblyName = method.Module.Assembly.GetName().Name;
            if (_assembliesToExclude.Contains(assemblyName) ||
                _methodsToExclude.Contains(method.Name) ||
                shouldExcludeType(method) ||
                isMicrosoftType(method))
            {
                return null;
            }

            var methodString = method.ToString();

            var returnName = string.Empty;
            var methodSignature = methodString;

            var splitIndex = methodString.IndexOf(' ');
            if (splitIndex > 0)
            {
                returnName = methodString.Substring(0, splitIndex);
                methodSignature = methodString.Substring(splitIndex + 1, methodString.Length - splitIndex - 1);
            }

            var typeNameFull = type.FullName;
            var lineNumber = stackFrame.GetFileLineNumber();

            var fileLine = string.Empty;
            var filePath = stackFrame.GetFileName();

            if (isTempFile(filePath))
                return null;

            if (!string.IsNullOrEmpty(filePath))
            {
                var fileName = Path.GetFileName(filePath);
                fileLine = string.Format("File={0}, Line={1}", fileName, lineNumber);

                methodInfo.CallingFile = fileName;
                methodInfo.CallingLine = lineNumber;
                methodInfo.CallingCol = stackFrame.GetFileColumnNumber();
                methodInfo.CallingFileFullName = filePath;
            }

            //there is no valid .pdb file
            if (onlyIncludeInfoWithFileLine && !File.Exists(filePath))
                return null;

            //couldn't extract the source file name
            if (onlyIncludeInfoWithFileLine && string.IsNullOrWhiteSpace(fileLine))
                return null;

            var methodSignatureFull = string.Format("{0} {1}.{2}", returnName, typeNameFull, methodSignature);
            methodInfo.CallingMethod = method.Name;
            methodInfo.StackTrace = string.Format("{0}", methodSignatureFull);

            return methodInfo;
        }

        private bool shouldExcludeType(MethodBase method)
        {
            return method.ReflectedType != null && _typesToExclude.Contains(method.ReflectedType);
        }
    }
}