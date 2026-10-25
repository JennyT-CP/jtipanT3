using System.Globalization;
using System.Text.RegularExpressions;

namespace jtipanT3.Views;

public partial class vContacto : ContentPage
{
	public vContacto()
	{
		InitializeComponent();

        // FECHA ACTUAL
        dtFecha.Date = DateTime.Today;
    }

    private void pkIdentificacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        // LIMPIAR LA IDENTIFICACIÓN SELECCIONADA
        txtIdentificacion.Text = "";

        if (pkIdentificacion.SelectedIndex == 0)
        {
            txtIdentificacion.Placeholder = "Ingrese 10 dígitos";
            txtIdentificacion.Keyboard = Keyboard.Numeric;
            txtIdentificacion.MaxLength = 10;
        }
        else if (pkIdentificacion.SelectedIndex == 1)
        {
            txtIdentificacion.Placeholder = "Ingrese 13 dígitos";
            txtIdentificacion.Keyboard = Keyboard.Numeric;
            txtIdentificacion.MaxLength = 13;
        }
        else if (pkIdentificacion.SelectedIndex == 2)
        {
            txtIdentificacion.Placeholder = "Ingrese pasaporte";
            txtIdentificacion.Keyboard = Keyboard.Text;
            txtIdentificacion.MaxLength = int.MaxValue;
        }

    }

    private async void btnContinuar_Clicked(object sender, EventArgs e)
    {
        // OBTENER LA INFORMACIÓN INGRESADA
        string identificacion = txtIdentificacion.Text?.Trim() ?? "";
        string nombres = txtNombres.Text?.Trim() ?? "";
        string apellidos = txtApellidos.Text?.Trim() ?? "";
        string correo = txtCorreo.Text?.Trim() ?? "";
        string salario = txtSalario.Text?.Trim() ?? "";

        // VALIDAR SELECCIÓN DEL PICKER
        if (pkIdentificacion.SelectedIndex == -1)
        {
            await DisplayAlertAsync(
                "Campo obligatorio",
                "Debe seleccionar un tipo de identificación.",
                "Aceptar");
            return;
        }

        // VALIDAR CAMPOS VACÍOS
        if (string.IsNullOrWhiteSpace(identificacion) ||
            string.IsNullOrWhiteSpace(nombres) ||
            string.IsNullOrWhiteSpace(apellidos) ||
            string.IsNullOrWhiteSpace(correo) ||
            string.IsNullOrWhiteSpace(salario))
        {
            await DisplayAlertAsync(
                "Campos obligatorios",
                "Debe completar todos los campos del formulario.",
                "Aceptar");
            return;
        }

        // VALIDAR CÉDULA DE IDENTIDAD --- 10 NÚMEROS
        if (pkIdentificacion.SelectedIndex == 0)
        {
            if (!Regex.IsMatch(identificacion, @"^[0-9]{10}$"))
            {
                await DisplayAlertAsync(
                    "Identificación incorrecta",
                    "La cédula debe contener exactamente 10 números.",
                    "Aceptar");
                return;
            }
        }

        // VALIDAR RUC --- 13 NÚMEROS
        if (pkIdentificacion.SelectedIndex == 1)
        {
            if (!Regex.IsMatch(identificacion, @"^[0-9]{13}$"))
            {
                await DisplayAlertAsync(
                    "Identificación incorrecta",
                    "El RUC debe contener exactamente 13 números.",
                    "Aceptar");
                return;
            }
        }

        // VALIDAR NOMBRES Y APELLIDOS --- INGRESAR SOLO LETRAS
        string patronLetras = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ ]+$";

        if (!Regex.IsMatch(nombres, patronLetras))
        {
            await DisplayAlertAsync(
                "Error en nombres",
                "Los nombres solo deben contener letras.",
                "Aceptar");
            return;
        }

        if (!Regex.IsMatch(apellidos, patronLetras))
        {
            await DisplayAlertAsync(
                "Error en apellidos",
                "Los apellidos solo deben contener letras.",
                "Aceptar");
            return;
        }

        // VALIDAR FORMATO DEL CORREO
        if (!Regex.IsMatch(
            correo,
            @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
        {
            await DisplayAlertAsync(
                "Correo incorrecto",
                "Ingrese un correo electrónico válido.",
                "Aceptar");
            return;
        }

        // VALIDAR SALARIO NUMÉRICO --- SOLO NÚMEROS Y PERMITIR EL INGRESO DE PUNTO O COMA DECIMAL
        if (!Regex.IsMatch(salario, @"^[0-9]+([.,][0-9]+)?$"))
        {
            await DisplayAlertAsync(
                "Salario incorrecto",
                "El salario solo debe contener números.",
                "Aceptar");
            return;
        }

        // VALIDAR MÁXIMO DOS DECIMALES
        if (!Regex.IsMatch(salario, @"^[0-9]+([.,][0-9]{1,2})?$"))
        {
            await DisplayAlertAsync(
                "Salario incorrecto",
                "El salario no puede tener más de dos decimales.",
                "Aceptar");
            return;
        }

        // VALIDAR SALARIO MAYOR A CERO
        if (!decimal.TryParse(
            salario.Replace(',', '.'),
            NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out decimal sueldo) || sueldo <= 0)
        {
            await DisplayAlertAsync(
                "Salario incorrecto",
                "El salario debe ser mayor a cero.",
                "Aceptar");
            return;
        }

        // OBTENER EL TIPO DE IDENTIFICACIÓN
        string tipo = pkIdentificacion.Items[
            pkIdentificacion.SelectedIndex
        ];

        // OBTENER LA FECHA SELECCIONADA
        string fecha = dtFecha.Date?.ToString("dd/MM/yyyy")
            ?? DateTime.Today.ToString("dd/MM/yyyy");

        // INFORMACION INGRESADA CORRECTA
        await DisplayAlertAsync(
            "Registro correcto",
            "Todos los datos ingresados son válidos.",
            "Aceptar");

        // NAVEGAR A LA VENTANA DE INFORMACIÓN
        await Navigation.PushAsync(new vInformacion(
            tipo,
            identificacion,
            nombres,
            apellidos,
            fecha,
            correo,
            sueldo
            ));
    }
}