using Microsoft.EntityFrameworkCore;
using Notes.Domain;

namespace Notes.Application.Interface;

public interface INotesDbContext
{
    DbSet<Note> Notes { get; set; }
    Task<int> SaveChangesAsync(CancellationToken token = default);
}