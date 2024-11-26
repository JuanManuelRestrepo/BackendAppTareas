using Domain;

public class TareaDTO
{

    public Guid id {  get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }

    // Solo el ID del proyecto en lugar del objeto completo
    public Guid? ProyectoId { get; set; }

    // Representación del estado
    public Tarea.EstadoTarea Estado { get; set; }

    // Lista opcional de IDs de responsables (usuarios)
    public List<Guid>? IdsResponsables { get; set; }
}
