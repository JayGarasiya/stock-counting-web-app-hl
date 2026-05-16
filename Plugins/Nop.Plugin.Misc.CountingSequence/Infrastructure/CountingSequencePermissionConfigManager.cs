using Nop.Core.Domain.Customers;
using Nop.Services.Security;

namespace Nop.Plugin.Misc.CountingSequence.Infrastructure
{
    public class CountingSequencePermissionConfigManager : IPermissionConfigManager
    {
        public const string COUNTING_SEQUENCE_TABLIST = "CountingSequence.TabList";

        public IList<PermissionConfig> AllConfigs => new List<PermissionConfig>
        {
             new PermissionConfig(
                name: "Counting Sequence Tab List",
                systemName: COUNTING_SEQUENCE_TABLIST,
                category: "Counting Sequence",
                defaultCustomerRoles: new[] { NopCustomerDefaults.AdministratorsRoleName }
            )
        };
    }
}