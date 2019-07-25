// ReSharper disable InvalidXmlDocComment
namespace UVACanvasAccess.Util {
    
    /// <summary>
    /// Classes implementing this interface provide an implementation for formatted output in the form of:
    /// <code>
    /// ClassName1 {<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;Field1: foo,<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;Field2: bar,<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;Field3: ClassName2 {<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Field1: baz<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;}<br/>
    /// }
    /// </code>
    /// </summary>
    public interface IPrettyPrint {
        string ToPrettyString();
    }
}