namespace UVACanvasAccess.Util {
    
    /// <summary>
    /// Classes implementing this interface provide an implementation for formatted output in the form of:
    /// <code>
    /// ClassName1 {
    ///     Field1: foo,
    ///     Field2: bar,
    ///     Field3: ClassName2 {
    ///         Field1: baz
    ///     }
    /// }
    /// </code>
    /// </summary>
    public interface IPrettyPrint {
        string ToPrettyString();
    }
}