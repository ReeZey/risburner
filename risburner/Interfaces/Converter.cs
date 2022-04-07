using System;
using System.IO;

namespace risburner.Interfaces
{
    public interface Converter : IFormattable
    {
        public void Init(FileInfo input) { }
    }
}