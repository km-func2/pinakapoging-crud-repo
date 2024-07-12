namespace crud_redo.Data;
using crud_redo.Models;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext :DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options){

    }

    public DbSet<BooksEntity> Books {get; set;}

}