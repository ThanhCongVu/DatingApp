
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext //DataContext nows is deriving from DbContext
    //DbContext represents a session with the database: used to query and save instances of our entities
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Value> Values { get; set; }
        // it is conventional to pluralise the entity or our model ( in this case Value)
    }
}