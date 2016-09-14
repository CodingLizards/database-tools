namespace Coding.Lizards.Database.Creation.Attributes {

    using System;
    using System.Data;

    /// <summary>
    /// This attribute identifies fields that should be used as columns
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute {

        /// <summary>
        /// Creates a new instance of the <see cref="ColumnAttribute"/>
        /// </summary>
        /// <param name="notnull">True when the column should be not null</param>
        public ColumnAttribute(bool notnull) {
            NotNull = notnull;
        }

        /// <summary>
        /// The name of the column, if empty the property name is used
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the column, if empty the type is guessed based on the property type
        /// </summary>
        public SqlDbType? Type { get; set; }

        /// <summary>
        /// True when the column should be not null
        /// </summary>
        public bool NotNull { get; set; }
    }
}