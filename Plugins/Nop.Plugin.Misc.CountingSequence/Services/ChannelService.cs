using Nop.Core;
using Nop.Data;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Channel service 
    /// </summary>
    public class ChannelService : IChannelService
    {
        #region Fields

        protected readonly IRepository<Channel> _channeRrepository;

        #endregion

        #region Ctor

        public ChannelService(IRepository<Channel> channeRrepository)
        {
            _channeRrepository = channeRrepository;
        }

        #endregion

        #region Methods 

        /// <summary>
        /// Gets a channel by identifier
        /// </summary>
        /// <param name="id">Channel identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the channel
        /// </returns>
        public async Task<Channel> GetChannelByIdAsync(int id)
        {
            return await _channeRrepository.GetByIdAsync(id);
        }

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
        public async Task<IPagedList<Channel>> GetAllChannelPagedAsync(
            bool showHidden = false,
           string channelName = "", 
           bool? visible = null, 
           int? channelTypeId = null,
           int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query = _channeRrepository.Table;

            if (!showHidden)
            {
                query = query.Where(m => m.Visible);
            }
            else if (visible.HasValue)
            {
                query = query.Where(m => m.Visible == visible.Value);
            }

            if (channelTypeId.HasValue)
            {
                query = query.Where(m => m.ChannelId == channelTypeId.Value);
            }

            if (!string.IsNullOrWhiteSpace(channelName))
                query = query.Where(m => m.Name.Contains(channelName));

            return await query.OrderBy(x => x.DisplayOrder).ToPagedListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// Inserts a channel
        /// </summary>
        /// <param name="channel">Channel</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InsertChannelAsync(Channel channel)
        {
            await _channeRrepository.InsertAsync(channel);
        }

        /// <summary>
        /// Updates the channel
        /// </summary>
        /// <param name="channel">Channel</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task UpdateChannelAsync(Channel channel)
        {
            await _channeRrepository.UpdateAsync(channel);
        }

        /// <summary>
        /// Delete the channel
        /// </summary>
        /// <param name="channel">Channel</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeleteChannelAsync(Channel channel)
        {
            await _channeRrepository.DeleteAsync(channel);
        }

        #endregion
    }
}
