using Microsoft.EntityFrameworkCore;
using Domain;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Proyecto> Proyectos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Tarea> Tareas { get; set; }
    public DbSet<TareaUsuario> TareaUsuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relación muchos a muchos entre Tarea y Usuario (a través de TareaUsuario)
        modelBuilder.Entity<Tarea>()
            .HasMany(t => t.TareaUsuarios)
            .WithOne(tu => tu.Tarea)
            .HasForeignKey(tu => tu.TareaId);

        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.TareaUsuarios)
            .WithOne(tu => tu.Usuario)
            .HasForeignKey(tu => tu.UsuarioId);

        modelBuilder.Entity<TareaUsuario>()
            .HasKey(tu => new { tu.TareaId, tu.UsuarioId });

        // Relación uno a muchos entre Proyecto y Tarea
        modelBuilder.Entity<Proyecto>()
            .HasMany(p => p.Tareas)
            .WithOne(t => t.Proyecto)
            .HasForeignKey(t => t.ProyectoId);
    }
}
