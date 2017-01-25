Imports System.Data
Partial Class wfrmEstatusRequisiciones
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Session("caller") = "wfrmEstatusRequisiciones.aspx"
        'If Not Page.IsPostBack Then

        Dim clsFunciones As New clsFunciones
        Dim dtsEstatus As New DataSet
        Dim strMensaje As String

        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn(13) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("estatus"), New DataColumn("fecha"), New DataColumn("intidestatus"), New DataColumn("intdepartamento"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("ccoid"), New DataColumn("cconumero"), New DataColumn("seleccionar")})


        Dim dtsRequisiciones As New DataSet
        strMensaje = clsFunciones.Llena_Grid(grvRequisiciones, "MIGRACION", dtsRequisiciones, "Requisiciones", "Cargar_DetallesRequisicionCompra", _
                    txtRequisicion.Text & "," & txtFechaDesde.Text.Replace("-", "") & "," & txtFechaHasta.Text.Replace("-", "") & ", ," & Session("idUsuario"), _
                    "@intIdRequisicion,@vchFechaDesde,@vchFechaHasta,@vchIdEstatus,@intIdSolicitante")
        If strMensaje = "OK" Then

            ViewState("Requisiciones") = dtsRequisiciones.Tables("Requisiciones")
            grvRequisiciones.DataSource = dtsRequisiciones.Tables("Requisiciones")
            grvRequisiciones.DataBind()
            'ViewState("Detalle") = Nothing
            'grvCotizaciones.DataSource = Nothing
            'grvCotizaciones.DataBind()
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar las Requisiciones: " & strMensaje.Replace("'", "") & "');", True)
        End If
        'End If
    End Sub


    'Protected Sub grvRequisiciones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvRequisiciones.RowCommand
    '    If (e.CommandName = "Seleccionar") Then
    '        ' Retrieve the row index stored in the CommandArgument property.
    '        Dim index As Integer = Convert.ToInt32(e.CommandArgument)

    '        ' Retrieve the row that contains the button 
    '        ' from the Rows collection.


    '        hdnRequisicionSelIndex.Value = index
    '        hdnCodigo.Value = ViewState("Requisiciones").Rows(index).Item(2)
    '        hdnConcepto.Value = ViewState("Requisiciones").Rows(index).Item(3)
    '        hdnCantidad.Value = ViewState("Requisiciones").Rows(index).Item(4)
    '        hdnRequisicion.Value = ViewState("Requisiciones").Rows(index).Item(0)
    '        hdnDetallerequisicion.Value = ViewState("Requisiciones").Rows(index).Item(1)

    '        Dim i As Integer
    '        For i = 0 To ViewState("Requisiciones").Rows.Count - 1
    '            grvRequisiciones.Rows(i).BackColor = Drawing.Color.White
    '        Next


    '        'ViewState("Detalle") = dtsProductos.Tables("Productos")
    '        'grvCotizaciones.DataSource = dtsProductos.Tables("Productos")
    '        'grvCotizaciones.DataBind()
    '        grvRequisiciones.Rows(index).BackColor = Drawing.Color.LightGreen

    '        'Se cargan las cotizaciones ya guardadas
    '        Dim strMensaje As String = ""
    '        Dim dt As New DataTable()
    '        dt.Columns.AddRange(New DataColumn(7) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("numeroproveedor"), New DataColumn("nombreproveedor"), New DataColumn("precio"), New DataColumn("requisicion"), New DataColumn("detallerequisicion")})
    '        Dim clsFunciones As New clsFunciones

    '        If Not ViewState("Detalle") Is Nothing Then
    '            dt = ViewState("Detalle")
    '        End If

    '        ViewState("Detalle") = dt
    '        grvCotizaciones.DataSource = dt
    '        grvCotizaciones.DataBind()


    '        Dim dtsCotizaciones As New DataSet
    '        strMensaje = clsFunciones.Llena_Grid(grvCotizaciones, "MIGRACION", dtsCotizaciones, "Detalle", "Cargar_Cotizaciones", _
    '                  hdnRequisicion.Value & "," & hdnDetallerequisicion.Value, _
    '                    "@intIdRequisicion,@intIdDetRequisicion")
    '        If strMensaje = "OK" Then

    '            ViewState("Detalle") = dtsCotizaciones.Tables("Detalle")
    '            grvCotizaciones.DataSource = dtsCotizaciones.Tables("Detalle")
    '            grvCotizaciones.DataBind()

    '        Else
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar las cotizaciones: " & strMensaje.Replace("'", "") & "');", True)
    '        End If


    '    End If
    'End Sub



    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Se redirecciona para aprobar otra requisicion de almacen
        Response.Redirect("wfrmEstatusRequisiciones.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
    End Sub


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
            ''_SMTP.UseDefaultCredentials = True

            ' CONFIGURACION DEL MENSAJE 
            _Message.[To].Add(EMail)
            '_Message.[To].Add("mcfierro@jmaschihuahua.gob.mx")
            '_Message.[To].Add("hcedillo78@hotmail.com")

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
            'ADICION DE DATOS ADJUNTOS 
            ''Dim _File As String = My.Application.Info.DirectoryPath & Archivo
            'Dim _FileXML As String = ArchivoXML
            'archivo que se quiere adjuntar 
            'Dim _AttachmentXML As New System.Net.Mail.Attachment(_FileXML, System.Net.Mime.MediaTypeNames.Application.Octet)
            '_Message.Attachments.Add(_AttachmentXML)
            'Dim _FilePDF As String = ArchivoPDF
            'archivo que se quiere adjuntar 
            'Dim _AttachmentPDF As New System.Net.Mail.Attachment(_FilePDF, System.Net.Mime.MediaTypeNames.Application.Octet)
            '_Message.Attachments.Add(_AttachmentPDF)

            'ENVIO 
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




End Class
