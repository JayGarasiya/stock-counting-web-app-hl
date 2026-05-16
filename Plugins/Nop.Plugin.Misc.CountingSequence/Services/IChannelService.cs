using Nop.Core;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Channel service interface
    /// </summary>
    public interface IChannelService
    {
        /// <summary>
        /// Gets a channel by identifier
        /// </summary>
        /// <param name="id">Channel identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the channel
        /// </returns>
        Task<Channel> GetChannelByIdAsync(int id);

        /// <summary>
        /// Get all channel entries
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">Show hidden</param>
        /// <param name="channelName">Channel name</param>
        /// <param name="visible">Visible</param>
        /// <param name="channelTypeId">Channel type identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        Task<IPagedList<Channel>> GetAllChannelPagedAsync( 
            bool showHidden = false,
           string channelName = "", 
           bool? visible = null, 
           int? channelTypeId = null,
           int pageIndex = 0,
           int pageSize = int.MaxValue);

        /// <summary>
        /// Inserts a channel
        /// </summary>
        /// <param name="channel">Channel</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertChannelAsync(Channel channel);

        /// <summary>
        /// Updates the channel
        /// </summary>
        /// <param name="channel">Channel</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateChannelAsync(Channel channel);

        /// <summary>
        /// Delete the channel
        /// </summary>
        /// <param name="channel">Channel</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteChannelAsync(Channel channel);
    }
}
