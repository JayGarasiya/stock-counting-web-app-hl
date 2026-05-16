using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.CountingSequence.Models.CountingSequence;
using System.ComponentModel;

namespace Nop.Plugin.Misc.CountingSequence.Infrastructure
{
    /// <summary>
    /// Startup task for the counting sequence
    /// </summary>
    public partial class CountingSequenceStartupTask : IStartupTask
    {
        #region Methods

        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute()
        {
            TypeDescriptor.AddAttributes(typeof(List<CountSheetModel>),
                new TypeConverterAttribute(typeof(CountSheetRowListTypeConverter)));

            TypeDescriptor.AddAttributes(typeof(IList<CountSheetModel>),
                new TypeConverterAttribute(typeof(CountSheetRowListTypeConverter)));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets order of this startup task implementation
        /// </summary>
        public int Order => 2;

        #endregion
    }
}
