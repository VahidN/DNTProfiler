using System;
using System.IO;
using System.Text;

namespace DNTProfiler.Dumper.Core
{
    public class JsonLogger : IDisposable
    {
        private readonly string _path;
        private TextWriter _writer;

        public JsonLogger(string path, bool append = true)
        {
            _path = path;
            _writer = new StreamWriter(path, append, Encoding.UTF8)
            {
                AutoFlush = true
            };
            _writer.WriteLine("[");
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Write(string data)
        {
            if (_writer == null || string.IsNullOrWhiteSpace(data)) return;
            _writer.Write(data);
        }

        public void WriteJsonObject(string data)
        {
            if (_writer == null || string.IsNullOrWhiteSpace(data)) return;
            _writer.Write(data);
            _writer.Write(",");
            _writer.Write(Environment.NewLine);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _writer == null) return;
            _writer.WriteLine("]");
            _writer.Dispose();
            _writer = null;
            deleteEmptyFile();
        }

        private void deleteEmptyFile()
        {
            if (!File.Exists(_path))
                return;

            var info = new FileInfo(_path);
            if (info.Length >= 1024)
                return;

            File.Delete(_path);
        }
    }
}