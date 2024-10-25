namespace Notes.Persistence;

public class DbInitializer
{
    public static void Initializer(NotesDbContext context)
    {
        context.Database.EnsureCreated();
    }
}