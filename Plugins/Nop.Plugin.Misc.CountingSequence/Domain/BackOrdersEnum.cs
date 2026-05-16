namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a back orders enum
    /// </summary>
    public enum BackOrdersEnum
    {
        /// <summary>
        /// Open
        /// </summary>
        Open = 2,

        /// <summary>
        /// Partial
        /// </summary>
        Partial = 4,

        /// <summary>
        /// Fulfilled
        /// </summary>
        Fulfilled = 8,

        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled = 16,
    }
}
