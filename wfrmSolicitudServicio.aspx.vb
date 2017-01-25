Imports System.Data
Imports System.Net.Mail.MailMessage

Partial Class wfrmSolicitudServicio
    Inherits System.Web.UI.Page


#Region "Eventos"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim clsFunciones As New clsFunciones
            Dim dtArticulos As New DataTable()

        End If
        txtNompleado.Text = Session("intEmpleado")
        txtSolictado.Text = Session("nombre")
        txtFecha.Text = Now
    End Sub



    Protected Sub btnSolicitar_Click(sender As Object, e As EventArgs) Handles btnSolicitar.Click
        Dim strMensaje As String = ""
        Dim clsFunciones As New clsFunciones
        If Session.Contents.Count <= 0 Then
            Alert("Se requiere entre de nuevo al sistema por superar 20 minutos sin usarlo")

            Exit Sub
        End If
        Try
            Dim partes() As String = txtSolictado.Text.Split(","c)
            Dim nombre As String = ""
            If partes.Length = 2 Then
                nombre = partes(1) & " " & partes(0)
            Else
                nombre = txtSolictado.Text
            End If
            'btnSolicitar.Enabled = False
            If txtServicio.Text <> "" Then 'Si esta capturado el numero de empleado
                strMensaje = clsFunciones.EjecutaProcedimiento("GAMMA", "InsertaSolicitudServicio", txtServicio.Text & "," & nombre.Replace(",", "") & "," & txtNompleado.Text, "@vchObservaciones,@vchSolicitante,@intNoempleado")
                If strMensaje = "" Then ' Si la solicitud se guardó correctamente
                    Alert("La solicitud se ha agregado correctamente")
                Else
                    Alert("No se ha podido guardar correctamente la solicitud")
                End If
            End If
        Catch ex As Exception
            Alert("Error al tratar de guardar solicitud de servicio: " & ex.Message)
        End Try

    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Se redirecciona para capturar otra requisicion de almacen
        Response.Redirect("wfrmBuscaEmpleado.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
    End Sub


#End Region

#Region "Procedimientos"

    Private Sub EnviarCorreo(ByVal Mensaje As String, ByVal EMail As String)
        Try
            'MsgBox(EMail + vbNewLine + ArchivoXML)
            Dim _Message As New System.Net.Mail.MailMessage()
            Dim _SMTP As New System.Net.Mail.SmtpClient
            'CONFIGURACIÓN DEL STMP

            _SMTP.Credentials = New System.Net.NetworkCredential("extranet@jmaschihuahua.gob.mx", "Jmaschihuahua-15")
            '_SMTP.Host = "mail.jmaschihuahua.gob.mx"
            _SMTP.Host = "74.220.207.110"
            _SMTP.Port = 25
            _SMTP.EnableSsl = False


            ' CONFIGURACION DEL MENSAJE 
            _Message.[To].Add(EMail)


            'Cuenta de Correo al que se le quiere enviar el e-mail 
            _Message.From = New System.Net.Mail.MailAddress("extranet@jmaschihuahua.gob.mx", "Extranet", System.Text.Encoding.UTF8)
            'Quien lo envía 
            _Message.Subject = "Salida de Almacen JMAS Chihuahua"
            'Sujeto del e-mail 
            _Message.SubjectEncoding = System.Text.Encoding.UTF8
            'Codificacion 
            _Message.Body = Mensaje
            'contenido del mail 
            _Message.BodyEncoding = System.Text.Encoding.UTF8
            _Message.Priority = System.Net.Mail.MailPriority.High
            _Message.IsBodyHtml = False

            Try
                _SMTP.Send(_Message)
                _SMTP = Nothing
                _Message = Nothing
            Catch ex As System.Net.Mail.SmtpException
                Alert("Error al enviar el correo de confirmacion:" & ex.Message)
                'MsgBox(ex.ToString)
            End Try
        Catch ex As Exception
            Alert("Error al enviar el correo de confirmacion:" & ex.Message)
        End Try
    End Sub

    Private Sub Alert(ByVal strMensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('" & strMensaje & "');", True)
    End Sub

#End Region

#Region "Funciones"

#End Region




End Class
