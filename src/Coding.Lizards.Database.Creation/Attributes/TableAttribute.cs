namespace Coding.Lizards.Database.Creation.Attributes {

    using System;

    /// <summary>
    /// This attribute identifies classes that should be treated as tables
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TableAttribute : Attribute {

        /// <summary>
        /// The name that the table should have, if not set the typename will be used
        /// </summary>
        public string Name { get; set; }
    }
}