using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Bibliotecas do R
using RDotNet;
using RDotNet.Devices;
using RDotNet.Internals;
using System;

namespace IpeaGEO
{
    class CharacterDevice : ICharacterDevice
    {
        public string ReadConsole(string prompt, int capacity, bool history)
        {
            return null;
        }

        private string strOutput;
        public string Output
        {
            get
            {
                return strOutput;
            }
            set
            {
                strOutput = value;
            }
        }

        public void WriteConsole(string output, int length, RDotNet.Internals.ConsoleOutputType outputType)
        {
            strOutput += output+"\r\n";
        }

        public string Messages { get; set; }

        public void ShowMessage(string message)
        {
            Messages += message;
        }

        public void Busy(BusyType which)
        {
        }

        public void Callback()
        {
        }

        public YesNoCancel Ask(string question)
        {
            return YesNoCancel.Cancel;
        }
    }
}
