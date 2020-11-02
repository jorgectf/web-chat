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
    public class DetailsQuery : IRequest<DetailsViewModel>
    {
        public int ConversationId { get; set; }

        private class Handler : IRequestHandler<DetailsQuery, DetailsViewModel>
        {
            private readonly IWebChatContext _context;
            private readonly IMapper _mapper;

            public Handler(IWebChatContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<DetailsViewModel> Handle(DetailsQuery request, CancellationToken cancellationToken)
            {
                return await _context.ConversationMessages
                    .Where(c => c.ConversationId == request.ConversationId)
                    .OrderByDescending(c => c.Publish)
                    .Take(1)
                    .ProjectTo<DetailsViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}