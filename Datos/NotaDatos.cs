using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Datos;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class NotaDatos
    {
        private readonly Conexion conexion = new Conexion();

        public void InsertarEstudianteConNotas(Estudiante estudiante)
        {
            using (SqlConnection conn = conexion.ObtenerConexion())
            {
                conn.Open();

                int idEstudiante;

                // Verificar si el estudiante ya existe
                SqlCommand cmdGetId = new SqlCommand("SELECT Id FROM Estudiantes WHERE legajo = @legajo", conn);
                cmdGetId.Parameters.AddWithValue("@legajo", estudiante.Legajo);
                object resultado = cmdGetId.ExecuteScalar();

                if (resultado != null)
                {
                    idEstudiante = (int)resultado;
                }
                else
                {
                    // Insertar nuevo estudiante y obtener su ID
                    SqlCommand cmdInsert = new SqlCommand(
                        "INSERT INTO Estudiantes (Nombre_y_Apellido, legajo, fecha_carga_estudiante) " +
                        "VALUES (@nombre, @legajo, GETDATE()); SELECT SCOPE_IDENTITY();", conn);

                    cmdInsert.Parameters.AddWithValue("@nombre", estudiante.NombreYApellido);
                    cmdInsert.Parameters.AddWithValue("@legajo", estudiante.Legajo);
                    idEstudiante = Convert.ToInt32(cmdInsert.ExecuteScalar());
                }

                // Insertar notas para el estudiante
                foreach (var materiaNota in estudiante.NotasPorMateria)
                {
                    try
                    {
                        string nombreMateria = materiaNota.Key;
                    decimal nota = materiaNota.Value;

                    // Obtener ID de la materia
                    SqlCommand cmdMat = new SqlCommand("SELECT Id FROM Materias WHERE Nombre = @nombre", conn);
                    cmdMat.Parameters.AddWithValue("@nombre", nombreMateria);
                    object idMateriaObj = cmdMat.ExecuteScalar();

                    // Validar que la materia exista
                    if (idMateriaObj == null)
                    {
                        throw new Exception($"La materia '{nombreMateria}' no existe en la base de datos.");
                    }

                    int idMateria = (int)idMateriaObj;

                    // Insertar nota
                    SqlCommand cmdNota = new SqlCommand(
                        "INSERT INTO Notas (Id_alumno, Id_materia, nota, Aprobado) " +
                        "VALUES (@idAlumno, @idMateria, @nota, @aprobado)", conn);
                    cmdNota.Parameters.AddWithValue("@idAlumno", idEstudiante);
                    cmdNota.Parameters.AddWithValue("@idMateria", idMateria);
                    cmdNota.Parameters.AddWithValue("@nota", nota);
                    cmdNota.Parameters.AddWithValue("@aprobado", nota >= 6 ? 1 : 0);
                    cmdNota.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                    }
                    
                }
            }
        }

        public List<Estudiante> ObtenerEstudiantesConNotas()
        {
            List<Estudiante> lista = new List<Estudiante>();
            Dictionary<string, Estudiante> mapa = new Dictionary<string, Estudiante>();

            using (SqlConnection conn = conexion.ObtenerConexion())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
                SELECT e.Nombre_y_Apellido, e.legajo, m.Nombre AS Materia, n.nota
                FROM Notas n
                JOIN Estudiantes e ON e.Id = n.Id_alumno
                JOIN Materias m ON m.Id = n.Id_materia", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string legajo = reader["legajo"].ToString();

                    if (!mapa.ContainsKey(legajo))
                    {
                        mapa[legajo] = new Estudiante
                        {
                            NombreYApellido = reader["Nombre_y_Apellido"].ToString(),
                            Legajo = legajo
                        };
                    }

                    string materia = reader["Materia"].ToString();
                    decimal nota = Convert.ToDecimal(reader["nota"]);
                    mapa[legajo].AgregarNota(materia, nota);
                }
            }

            return mapa.Values.ToList();
        }

    }
}
