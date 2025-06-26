using Datos;
using Entidades;
using System;
using Negocio;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Estudiantes
{
    public partial class FormEstudiantes : Form
    {
        private SqlConnection conexion;
        public FormEstudiantes()
        {
            InitializeComponent();
            tcEstudiantes.SelectedIndexChanged += tcEstudiantes_SelectedIndexChanged;
            this.Load += FormEstudiantes_Load;
            this.FormClosed += FormEstudiantes_FormClosed;
        }

        // Base de datos
        private void FormEstudiantes_Load(object sender, EventArgs e)
        {
            try
            {
                Conexion con = new Conexion();
                conexion = con.ObtenerConexion();
                conexion.Open();

                NotaNegocio negocio = new NotaNegocio();
                estudiantes = negocio.ObtenerTodos();  // Carga la lista desde la base
                MessageBox.Show("Conexión abierta correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la conexión: " + ex.Message);
            }

        }
        //---------------------------------------

        private void FormEstudiantes_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (conexion != null && conexion.State == System.Data.ConnectionState.Open)
                conexion.Close();
        }


        //nueva lista de estudiantes

        private List<Estudiante> estudiantes = new List<Estudiante>();

        public void btRegistrar_Click(object sender, EventArgs e)
        {

            string nombreYapellido = tbNombre.Text.Trim();
            string legajo = tbLegajo.Text.Trim();

            //Controlar si el usuario no ingreso nombre, apellido o legajo del estudiante

            if (string.IsNullOrEmpty(nombreYapellido))
            {
                MessageBox.Show("Por favor, ingresa el nombre del estudiante.");
                return;
            }
            else if (string.IsNullOrEmpty(legajo))
            {
                MessageBox.Show("Por favor, ingresa el legajo del estudiante.");
                return;
            }

            //Arreglo de materias en los checkboxes y textboxes

            MateriaDatos materiaRepo = new MateriaDatos();
            string[] materias = materiaRepo.ObtenerNombresMaterias();
            System.Windows.Forms.CheckBox[] checkBoxes = { cbMatematica, cbLengua, cbHistoria, cbGeografia, cbBiología, cbQuimica, cbFisica, cbIngles };
            System.Windows.Forms.TextBox[] textBoxes = { tbMatematica, tbLengua, tbHistoria, tbGeografia, tbBiologia, tbQuimica, tbFisica, tbIngles };

            //Controlar si puso al menos una nota en la materia

            bool alMenosUnoMarcado = false;

            foreach (System.Windows.Forms.CheckBox cb in checkBoxes)
            {
                if (cb.Checked)
                {
                    alMenosUnoMarcado = true;
                    break;
                }
            }

            if (alMenosUnoMarcado)
            {
                // Crear el estudiante con su nombre y legajo

                Estudiante estudiante = new Estudiante { NombreYApellido = nombreYapellido, Legajo = legajo };

                // Cargar la/s materia/s y su/s nota/s

                for (int i = 0; i < materias.Length; i++)
                {
                    if (checkBoxes[i].Checked)
                    {
                        if (decimal.TryParse(textBoxes[i].Text, out decimal nota))
                        {
                            estudiante.AgregarNota(materias[i], nota);
                        }
                        else
                        {
                            MessageBox.Show($"La nota ingresada en {materias[i]} no es válida.");
                            return; 
                        }
                    }
                }

                //Controlar si el usuario ingreso una nota pero no marco el checkbox

                for (int i = 0; i < checkBoxes.Length; i++)
                {
                    string notaTexto = textBoxes[i].Text.Trim();

                    if (!string.IsNullOrEmpty(notaTexto))
                    {
                        // Verifica que el checkbox esté marcado
                        if (!checkBoxes[i].Checked)
                        {
                            MessageBox.Show($"Has ingresado una nota en {checkBoxes[i].Text}, pero no seleccionaste la materia.");
                            return;
                        }

                        // Verifica que la nota sea un número decimal entre 1.0 y 10.0
                        if (!decimal.TryParse(notaTexto, out decimal nota) || nota < 1.0m || nota > 10.0m)
                        {
                            MessageBox.Show($"La nota ingresada en {checkBoxes[i].Text} no es válida. Debe ser un número entre 1 y 10");
                            return;
                        }
                    }
                }

                //Agregar estudiante al listado
                NotaNegocio negocio = new NotaNegocio();
                negocio.GuardarEstudianteConNotas(estudiante);
                estudiantes.Add(estudiante);
                LimpiarFormulario();
                MessageBox.Show("Estudiante guardado correctamente.");
            }
            else
            {
                MessageBox.Show("Debe seleccionar al menos una materia.");
            }
            
        }

        private void tcEstudiantes_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            //Controlar si se cargaron estudiantes

            if (tcEstudiantes.SelectedIndex == 1) // Segunda pestaña: lista
            {
                lbEstudiantes.Items.Clear();

                NotaNegocio negocio = new NotaNegocio();
                var estudiantes = negocio.ObtenerTodos();

                if (estudiantes.Count == 0)
                {
                    lbEstudiantes.Items.Add("No hay estudiantes cargados.");
                    return;
                }

                foreach (var est in estudiantes)
                {
                    lbEstudiantes.Items.Add("Estudiante:");
                    lbEstudiantes.Items.Add($"  Nombre y Apellido: {est.NombreYApellido}");
                    lbEstudiantes.Items.Add($"  Legajo: {est.Legajo}");

                    foreach (var materia in est.NotasPorMateria)
                    {
                        lbEstudiantes.Items.Add($"   {materia.Key}: {materia.Value}");
                    }

                    lbEstudiantes.Items.Add("");
                }
            }

        }

        private void LimpiarFormulario()
        {
            tbNombre.Text = "";
            tbLegajo.Text = "";

            //Limpiar pantalla donde se cargan los estudiantes

            System.Windows.Forms.CheckBox[] checkBoxes = { cbMatematica, cbLengua, cbHistoria, cbGeografia, cbBiología, cbQuimica, cbFisica, cbIngles };
            System.Windows.Forms.TextBox[] textBoxes = { tbMatematica, tbLengua, tbHistoria, tbGeografia, tbBiologia, tbQuimica, tbFisica, tbIngles };

            for (int i = 0; i < checkBoxes.Length; i++)
            {
                checkBoxes[i].Checked = false;
                textBoxes[i].Text = "";
            }
        }
    }
}
