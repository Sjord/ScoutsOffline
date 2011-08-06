using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutsOffline.Sol
{
    class CsvConverter
    {
        private string[] lines;
        private int currentLine;

        public CsvConverter(string contents)
        {
            this.lines = contents.Split('\n');
            currentLine = 1;
        }

        public List<string> GetKeys()
        {
            return ParseLine(lines[0]);
        }

        public List<string> GetValues()
        {
            if (currentLine >= lines.Length) return null;
            return ParseLine(lines[currentLine++]);
        }

        public List<string> ParseLine(string line)
        {
            List<string> result = new List<string>();
            line = line.Substring(1, line.Length - 2);
            return line.Split(new[] { "\",\"" }, StringSplitOptions.None).ToList();
        }
    }
}