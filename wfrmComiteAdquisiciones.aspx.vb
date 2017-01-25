Imports System.Data
Partial Class wfrmComiteAdquisiciones
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Session("caller") = "wfrmComiteAdquisiciones.aspx"
        If Not Page.IsPostBack Then

            Dim clsFunciones As New clsFunciones
            Dim dtsEstatus As New DataSet
            Dim strMensaje As String

            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(13) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("estatus"), New DataColumn("fecha"), New DataColumn("intidestatus"), New DataColumn("intdepartamento"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("ccoid"), New DataColumn("cconumero"), New DataColumn("seleccionar")})


            Dim dtsRequisiciones As New DataSet
            strMensaje = clsFunciones.Llena_Grid(grvRequisiciones, "MIGRACION", dtsRequisiciones, "Requisiciones", "Cargar_DetallesRequisicionCompra", _
                        ", , ,5,", _
                        "@intIdRequisicion,@vchFechaDesde,@vchFechaHasta,@vchIdEstatus,@intIdSolicitante")
            If strMensaje = "OK" Then

                ViewState("Requisiciones") = dtsRequisiciones.Tables("Requisiciones")
                grvRequisiciones.DataSource = dtsRequisiciones.Tables("Requisiciones")
                grvRequisiciones.DataBind()
                ViewState("Detalle") = Nothing
                grvCotizaciones.DataSource = Nothing
                grvCotizaciones.DataBind()
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar las Requisiciones: " & strMensaje.Replace("'", "") & "');", True)
            End If
        End If
    End Sub


    Protected Sub grvCotizaciones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvCotizaciones.RowCommand
        If (e.CommandName = "Seleccionar") Then
            ' Obtenemos la fila seleccionada
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim strMensaje As String = ""
            Dim clsFunciones As New clsFunciones
            Try
                If grvCotizaciones.Rows.Count > 0 Then


                    Dim dt As New DataTable()
                    If Not ViewState("Detalle") Is Nothing Then
                        dt = ViewState("Detalle")
                    End If
                    'dt.Columns.AddRange(New DataColumn(7) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("numeroproveedor"), New DataColumn("nombreproveedor"), New DataColumn("precio"), New DataColumn("requisicion"), New DataColumn("detallerequisicion")})


                    hdnPrecio.Value = ViewState("Detalle").Rows(index).Item(5)


                    'Se actualiza el estatus del detalle de la requisicion
                    strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Actualiza_Estatus_RequisicionCompra", _
                     hdnRequisicion.Value & "," & hdnDetallerequisicion.Value & ",6," & Session("IdUsuario") & ",Aprobado por comité," & hdnPrecio.Value, _
                       "@intRequisicion,@intDetRequisicion,@intEstatus,@intIdUsuarioCaptura,@vchComentarios,@decPrecio")

                    If strMensaje = "OK" Then
                        Alert("Se ha seleccionado un precio para la requisición: " & hdnRequisicion.Value.ToString & ",  artículo " & hdnConcepto.Value.ToString)

                        Timer1.Enabled = True

                    Else
                        Alert("Error al guardar cambios " & strMensaje)
                        Exit Sub
                    End If


                Else
                    Alert("Favor de agregar datos a la orden")
                End If

            Catch ex As Exception
            Alert("Error al guardar la autorizacion de la requisicion: " & ex.Message & ": ")
        End Try
        End If
    End Sub


    Protected Sub grvRequisiciones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvRequisiciones.RowCommand
        If (e.CommandName = "Seleccionar") Then
            ' Retrieve the row index stored in the CommandArgument property.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            ' Retrieve the row that contains the button 
            ' from the Rows collection.


            hdnRequisicionSelIndex.Value = index
            hdnCodigo.Value = ViewState("Requisiciones").Rows(index).Item(2)
            hdnConcepto.Value = ViewState("Requisiciones").Rows(index).Item(3)
            hdnCantidad.Value = ViewState("Requisiciones").Rows(index).Item(4)
            hdnRequisicion.Value = ViewState("Requisiciones").Rows(index).Item(0)
            hdnDetallerequisicion.Value = ViewState("Requisiciones").Rows(index).Item(1)

            Dim i As Integer
            For i = 0 To ViewState("Requisiciones").Rows.Count - 1
                grvRequisiciones.Rows(i).BackColor = Drawing.Color.White
            Next


            'ViewState("Detalle") = dtsProductos.Tables("Productos")
            'grvCotizaciones.DataSource = dtsProductos.Tables("Productos")
            'grvCotizaciones.DataBind()
            grvRequisiciones.Rows(index).BackColor = Drawing.Color.LightBlue

            'Se cargan las cotizaciones ya guardadas
            Dim strMensaje As String = ""
            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(9) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("numeroproveedor"), New DataColumn("nombreproveedor"), New DataColumn("precio"), New DataColumn("requisicion"), New DataColumn("detallerequisicion"), New DataColumn("marca"), New DataColumn("tiempoentrega")})
            Dim clsFunciones As New clsFunciones

            If Not ViewState("Detalle") Is Nothing Then
                dt = ViewState("Detalle")
            End If

            ViewState("Detalle") = dt
            grvCotizaciones.DataSource = dt
            grvCotizaciones.DataBind()


            Dim dtsCotizaciones As New DataSet
            strMensaje = clsFunciones.Llena_Grid(grvCotizaciones, "MIGRACION", dtsCotizaciones, "Detalle", "Cargar_Cotizaciones", _
                      hdnRequisicion.Value & "," & hdnDetallerequisicion.Value, _
                        "@intIdRequisicion,@intIdDetRequisicion")
            If strMensaje = "OK" Then

                ViewState("Detalle") = dtsCotizaciones.Tables("Detalle")
                grvCotizaciones.DataSource = dtsCotizaciones.Tables("Detalle")
                grvCotizaciones.DataBind()

            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar las cotizaciones: " & strMensaje.Replace("'", "") & "');", True)
            End If


        End If
    End Sub



    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Se redirecciona para aprobar otra requisicion de almacen
        Response.Redirect("wfrmComiteAdquisiciones.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
    End Sub



    'Protected Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
    '    '   Dim strMensaje As String
    '    Dim clsFunciones As New clsFunciones
    '    Try
    '        If grvProducto.Rows.Count > 0 Then
    '            If txtMotivoo.Text.Replace(" ", "") <> "" Then
    '                Response.Redirect("wfrmRequisicionCompra.aspx?key=" & Request.QueryString("key").Replace("+", "%2B") & _
    '                          "&idrequisicion=" & ViewState("Requisiciones").Rows(0).item(0) & "&intidestatus=" & ViewState("Requisiciones").Rows(0).item(3) & "&Motivo=" & txtMotivoo.Text)
    '            Else
    '                Alert("Favor de anotar el motivo de la modificación")
    '            End If
    '        Else
    '            Alert("Favor de seleccionar Requisicion de Almacen a modificar")
    '        End If

    '    Catch ex As Exception
    '        Alert("Error al cancelar la requisicion: " & ex.Message & ": ")
    '    End Try
    'End Sub

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
