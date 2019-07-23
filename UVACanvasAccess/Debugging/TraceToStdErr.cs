using System;
using System.Diagnostics;

namespace UVACanvasAccess.Debugging {
    
    internal class TraceToStdErr : TraceListener {
        public override void Write(string msg) {
            Console.Error.Write(msg);
        }
        public override void WriteLine(string msg) {
            Console.Error.WriteLine(msg);
        }
    }
}