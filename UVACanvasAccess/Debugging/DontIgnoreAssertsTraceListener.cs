using System;
using System.Diagnostics;

namespace UVACanvasAccess.Debugging {
    
    // The default behavior for debug asserts seems to be ignoring them completely, even when compiling in Debug.
    // To make them function, we have to implement this class and affirmatively set this as our trace listener.
    internal class DontIgnoreAssertsTraceListener : TraceListener {
        public override void Write(string msg) {
            throw new Exception("Assertion " + msg);
        }
        public override void WriteLine(string msg) {
            throw new Exception("Assertion" + msg);
        }
    }
}