using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class Conexion
    {
        //private readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BaseDeDatos\Estudiantes.mdf";
        private readonly string connectionString = @"Server=JOAQUINR7\SQLEXPRESS,50932;Database=Estudiantes;Uid=joacosql;Pwd=12345;";

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(connectionString);
        }
    }
}
