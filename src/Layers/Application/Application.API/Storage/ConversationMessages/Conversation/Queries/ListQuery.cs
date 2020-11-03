using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebChat.Application.API.Common.Interfaces;
using WebChat.Application.API.Storage.ConversationMessages.Core.Models;

namespace WebChat.Application.API.Storage.ConversationMessages.Conversation.Queries
{
    public class ListQuery : IRequest<ListViewModel>
    {
        public int ConversationId { get; set; }

        private class Handler : IRequestHandler<ListQuery, ListViewModel>
        {
            private readonly IWebChatContext _context;
            private readonly IMapper _mapper;

            public Handler(IWebChatContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ListViewModel> Handle(ListQuery request, CancellationToken cancellationToken)
            {
                return new ListViewModel
                {
                    Messages = await _context.ConversationMessages
                        .Where(e => e.ConversationId == request.ConversationId)
                        .OrderByDescending(c => c.Publish)
                        .ProjectTo<DetailsViewModel>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken)
                };
            }
        }
    }
}