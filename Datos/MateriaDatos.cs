using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
//using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Datos
{
    public class MateriaDatos
    {
        private readonly Conexion conexion;

        public MateriaDatos()
        {
            conexion = new Conexion();
        }

        // Devuelve solo los nombres de las materias en un arreglo
        public string[] ObtenerNombresMaterias()
        {
            List<string> nombres = new List<string>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string query = "SELECT Nombre FROM Materias ORDER BY Id";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    nombres.Add(reader["Nombre"].ToString());
                }

                reader.Close();
            }

            return nombres.ToArray();
        }

        // (opcional) Devuelve lista completa de objetos Materia
        public List<Materia> ObtenerMaterias()
        {
            List<Materia> materias = new List<Materia>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string query = "SELECT Id, CodigoMateria, Nombre FROM Materias";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    materias.Add(new Materia
                    {
                        Id = (int)reader["Id"],
                        CodigoMateria = reader["CodigoMateria"].ToString(),
                        Nombre = reader["Nombre"].ToString()
                    });
                }

                reader.Close();
            }

            return materias;
        }
    }
}
