using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.DeleteNote
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand>
    {
        private readonly INotesDbContext _context;
        public DeleteNoteCommandHandler(INotesDbContext context) 
        {
            _context = context;
        }

        public async Task Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Notes.FirstOrDefaultAsync(n => n.ID == request.Id);
            if (entity != null && entity.UserID == request.UserId)
            {
                _context.Notes.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new NotFoundException(nameof(Note), request.Id);
            }
        }
    }
}
