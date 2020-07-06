using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Huffmann_Translator.Model
{
    public class Model
    {
        public Model()
        {
            HuffmannToAscii = new Dictionary<string, string>();
            AsciiToHuffmann = new Dictionary<string, string>();
        }


        public Dictionary<string, string> HuffmannToAscii { get; set; }

        public Dictionary<string, string> AsciiToHuffmann { get; set; }

        /// <summary>
        /// Liest die Codetabelle aus dem übergebenen Dateinamen aus und Speichert die Werte in den Dictionaries
        /// </summary>
        /// <param name="filename"></param>
        public void ReadCodeTable(string filename)
        {
            HuffmannToAscii.Clear();
            AsciiToHuffmann.Clear();
            
            var file = new System.IO.StreamReader(filename);            
            while (!file.EndOfStream)
            {
                var line = file.ReadLine();
                var parts = line.Split(';');
                if(parts.Length == 2) { 
                HuffmannToAscii.Add(parts[1], parts[0]);
                AsciiToHuffmann.Add(parts[0], parts[1]);
                }
            }
        }

        /// <summary>
        /// Ersetzt den Huffmanncode durch die ASCII zeichen
        /// </summary>
        /// <param name="huffmann"></param>
        /// <returns></returns>
        public string Huffmann_Ascii(string huffmann)
        {
            var keynotFound = false;
            var valueString = "";
            string buchstabe = "";
            for (int i = 0; i < huffmann.Length; i++)
            {
                if(keynotFound)
                    buchstabe += huffmann.Substring(i, 1);
                else
                    buchstabe = huffmann.Substring(i, 1);

                if (HuffmannToAscii.ContainsKey(buchstabe))
                {
                    valueString += HuffmannToAscii[buchstabe];
                    keynotFound = false;
                }
                    
                else
                    keynotFound = true;
            }
            return valueString;

        }

        /// <summary>
        /// Ersetzt die ascii Zeichen durch deren Huffmancode
        /// </summary>
        /// <param name="ascii"></param>
        /// <returns></returns>
        public string Ascii_Huffmann(string ascii)
        {
            var valueString = "";
            for (int i = 0; i < ascii.Length; i++)
            {
                string buchstabe = ascii.Substring(i, 1);
                if (AsciiToHuffmann.ContainsKey(buchstabe))
                    valueString += AsciiToHuffmann[buchstabe];
                else
                    valueString += "###" + buchstabe + "###";   // Zeichen in Codetabelle nicht vorhanden
            }
            return valueString;
        }


    }
}
