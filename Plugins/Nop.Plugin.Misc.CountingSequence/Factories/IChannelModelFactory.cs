using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.Channel;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Channel model factory interface
    /// </summary>
    public interface IChannelModelFactory
    {
        /// <summary>
        /// Prepare channel search model
        /// </summary>
        /// <param name="searchModel">Channel search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the channel search model
        /// </returns>
        Task<ChannelSearchModel> PrepareChannelSearchModelAsync(ChannelSearchModel searchModel);

        /// <summary>
        /// Prepare channel list model
        /// </summary>
        /// <param name="searchModel">Channel search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the channel list model
        /// </returns>
        Task<ChannelListModel> PrepareChannelListModelAsync(ChannelSearchModel searchModel);

        /// <summary>
        /// Prepare channel model
        /// </summary>
        /// <param name="model">Channel model</param>
        /// <param name="channel">Channel entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the channel model
        /// </returns>
        Task<ChannelModel> PrepareChannelModelAsync(ChannelModel model, Channel channel);
    }
}
