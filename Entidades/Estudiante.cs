using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Estudiante
    {
        #region Atributos

        //public string Legajo { get; set; }
        //public string NombreYApellido { get; set; }

        private string legajo;

        private string nombreYApellido;

        public string Legajo { get => legajo; set => legajo = value; }
        public string NombreYApellido { get => nombreYApellido; set => nombreYApellido = value; }
        public Dictionary<string, decimal> NotasPorMateria { get; set; }

        #endregion

        #region Constructores
        public Estudiante()
        {
            NotasPorMateria = new Dictionary<string, decimal>();
        }
        #endregion

        #region Metodos
        public void AgregarNota(string materia, decimal nota)
        {
            NotasPorMateria[materia] = nota;
        }
        #endregion
    }
}
