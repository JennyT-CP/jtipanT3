using System.Text;
namespace jtipanT3.Views;

public partial class vInformacion : ContentPage
{
	public vInformacion(
        string tipo,
        string identificacion,
        string nombres,
        string apellidos,
        string fecha,
        string correo,
        decimal salario
        )
	{
		InitializeComponent();

        // MOSTRAR LOS DATOS RECIBIDOS
        lblTipo.Text = tipo;
        lblIdentificacion.Text = identificacion;
        lblNombres.Text = nombres;
        lblApellidos.Text = apellidos;
        lblFecha.Text = fecha;
        lblCorreo.Text = correo;
        lblSalario.Text = "$" + salario.ToString("F2");

        // CALCULAR EL APORTE AL IESS
        decimal aporteIESS = salario * 0.0945m;

        // MOSTRAR EL APORTE CALCULADO
        lblIESS.Text = "$" + aporteIESS.ToString("F2");
    }

    private async void btnExportar_Clicked(object sender, EventArgs e)
    {
        // OBTENER LOS DATOS DE LOS LABEL
        string tipo = lblTipo.Text;
        string identificacion = lblIdentificacion.Text;
        string nombres = lblNombres.Text;
        string apellidos = lblApellidos.Text;
        string fecha = lblFecha.Text;
        string correo = lblCorreo.Text;
        string salario = lblSalario.Text;
        string aporteIESS = lblIESS.Text;

        // CONTENIDO DEL ARCHIVO
        string datos =
            "===== REGISTRO DE CONTACTOS =====\n" +
            "Tipo de identificación: " + tipo + "\n" +
            "Identificación: " + identificacion + "\n" +
            "Nombres: " + nombres + "\n" +
            "Apellidos: " + apellidos + "\n" +
            "Fecha: " + fecha + "\n" +
            "Correo: " + correo + "\n" +
            "Salario: " + salario + "\n" +
            "Aporte IESS: " + aporteIESS + "\n" +
            "=================================\n\n";

        try
        {
            // DEFINIR LA RUTA DEL ARCHIVO
            string ruta = Path.Combine(
                FileSystem.AppDataDirectory,
                "contactos.txt"
            );

            // GUARDAR SIN BORRAR REGISTROS ANTERIORES
            await File.AppendAllTextAsync(
                ruta,
                datos,
                Encoding.UTF8
            );

            // MOSTRAR MENSAJE DE CONFIRMACIÓN
            await DisplayAlertAsync(
                "Exportación exitosa",
                "La información se guardó correctamente." +
                "\n\nRuta: " + ruta,
                "Aceptar"
            );
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Error",
                "No se pudo guardar el archivo: " + ex.Message,
                "Aceptar"
            );
        }
    }
}