using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Entidades;

namespace Negocio
{
    public class NotaNegocio
    {
            private NotaDatos _repo;

            public NotaNegocio()
            {
                _repo = new NotaDatos();
            }

            public void GuardarEstudianteConNotas(Estudiante estudiante)
            {
                _repo.InsertarEstudianteConNotas(estudiante);
            }
            public List<Estudiante> ObtenerTodos()
            {
                return _repo.ObtenerEstudiantesConNotas();
            }
    }
}
