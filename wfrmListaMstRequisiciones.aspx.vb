Imports System.Data
Partial Class wfrmListaMstRequisiciones
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim clsFunciones As New clsFunciones
            Dim dtsEstatus As New DataSet
            Dim strMensaje As String

            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(8) {New DataColumn("numrequisicion"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("estatus"), New DataColumn("fecha"), New DataColumn("intidestatus"), New DataColumn("intdepartamento"), New DataColumn("intContrato"), New DataColumn("seleccionar")})

            Dim filterEstatus As String = ""
            hdnProfileId.Value = Session("idPerfil")
            ' 22 - Jefe de departamento | 24 - Director correspondiente | 26 - Director Administrativo
            If hdnProfileId.Value <> "" Then
                Select Case hdnProfileId.Value
                    Case 22
                        filterEstatus = "'1'"
                    Case 24
                        filterEstatus = "'2'"
                    Case 26
                        filterEstatus = "'2-3'"
                    Case Else
                        filterEstatus = ""
                End Select
            End If
            Dim dtsRequisiciones As New DataSet
            strMensaje = clsFunciones.Llena_Grid(grvSolicitudes, "MIGRACION", dtsRequisiciones, "Requisiciones", "Cargar_RequisicionesCompra", _
                        txtRequisicion.Text & "," & filterEstatus & "," & txtFechaDesde.Text & "," & txtFechaHasta.Text & "," & Session("departamento") & ",1", _
                        "@intIdRequisicion,@vchIdEstatus,@vchFechaDesde,@vchFechaHasta,@intDepartamento,@intConContrato")
            If strMensaje = "OK" Then

                ViewState("Solicitudes") = dtsRequisiciones.Tables("Requisiciones")
                grvSolicitudes.DataSource = dtsRequisiciones.Tables("Requisiciones")
                grvSolicitudes.DataBind()
                ViewState("Productos") = Nothing
                grvProducto.DataSource = Nothing
                grvProducto.DataBind()
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar las Requisiciones: " & strMensaje.Replace("'", "") & "');", True)
            End If
        End If
    End Sub


    Protected Sub grvSolicitudes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvSolicitudes.RowCommand
        If (e.CommandName = "Seleccionar") Then
            ' Retrieve the row index stored in the CommandArgument property.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            ' Retrieve the row that contains the button 
            ' from the Rows collection.
            Dim row As GridViewRow = grvSolicitudes.Rows(index)
            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(4) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad")})
            Dim clsFunciones As New clsFunciones
            Dim dtsProductos As New DataSet
            Dim strMensaje As String
            strMensaje = clsFunciones.Llena_Grid(grvProducto, "MIGRACION", dtsProductos, "Productos", "Cargar_DetalleRequisicionCompra", _
                                                 grvSolicitudes.Rows(index).Cells(0).Text, _
                                                 "@intIdRequisicion")

            If strMensaje = "OK" Then
                hdnIdRequisicionSel.Value = grvSolicitudes.Rows(index).Cells(0).Text
                Dim i As Integer
                For i = 0 To ViewState("Solicitudes").Rows.Count - 1
                    grvSolicitudes.Rows(i).BackColor = Drawing.Color.White
                Next
                txtMotivoo.ReadOnly = False

                ViewState("Productos") = dtsProductos.Tables("Productos")
                grvProducto.DataSource = dtsProductos.Tables("Productos")
                grvProducto.DataBind()
                grvSolicitudes.Rows(index).BackColor = Drawing.Color.LightGreen
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar el detalle de la Requisicion: " & strMensaje.Replace("'", "") & "');", True)
            End If
        End If
    End Sub

    Protected Sub btnGenerarOrden_click(sender As Object, e As EventArgs) Handles btnGenerarOrden.Click
        Dim strMensaje As String
        Dim clsFunciones As New clsFunciones
        Dim dtsOrden As New DataSet
        Dim dtTmp As New DataTable()
        Try
            If grvProducto.Rows.Count > 0 Then
  
                strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsOrden, "Orden", "Inserta_OcDesdeReq", _
                            ViewState("Solicitudes").Rows(0).item(0) & "," & Session("IdUsuario") & "," & ViewState("Solicitudes").Rows(0).item(9), _
                            "@intIdRequi,@intEmpleadoGenera,@intIdContrato", dtTmp)
                'strMensaje = "OK"
                If strMensaje = "OK" Then
                    ifrOrdenCompra.Src = "wfrmReporteOrdenCompra.aspx?key=" & Request.QueryString("key").Replace("+", "%2B") & _
                   "&intOrden=" & dtsOrden.Tables("Orden").Rows(0).Item(0).ToString
                    Alert("Se ha generado la orden de compra para la requisición RE" & ViewState("Productos").Rows(0).item(0) & "")


                    Timer1.Enabled = True
                Else
                    Alert("Error al generar ka orden de compra para la requisición RE" & ViewState("Productos").Rows(0).item(0) & ": " & strMensaje)
                End If
            Else
                Alert("Favor de seleccionar Requisicion de compra")
            End If

        Catch ex As Exception
            Alert("Error al guardar la autorizacion de la requisicion: " & ex.Message & ": ")
        End Try
    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Se redirecciona para aprobar otra requisicion de almacen
        Response.Redirect("wfrmListaMstRequisiciones.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim strMensaje As String
        Dim clsFunciones As New clsFunciones
        Try
            If grvProducto.Rows.Count > 0 Then
                If txtMotivoo.Text.Replace(" ", "") <> "" Then

                    strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Actualiza_Estatus_RequisicionCompra", _
                          ViewState("Solicitudes").Rows(0).item(0) & ",5," & Session("IdUsuario") & ",Normal", _
                          "@intRequisicion,@intEstatus,@intIdUsuarioCaptura,@vchComentarios")

                    If strMensaje = "OK" Then
                        Alert("Requisicion de compra  RE" & ViewState("Productos").Rows(0).item(0) & " cancelada con exito")
                        Timer1.Enabled = True
                    Else
                        Alert("Error al cancelar la requisicion RE" & ViewState("Productos").Rows(0).item(0) & ": " & strMensaje)
                    End If
                Else
                    Alert("Favor de anotar el motivo de la cancelación")
                End If

            Else
                Alert("Favor de seleccionar Requisicion de compra a cancelar")
            End If

        Catch ex As Exception
            Alert("Error al cancelar la requisicion: " & ex.Message & ": ")
        End Try
    End Sub


    'Protected Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
    '    '   Dim strMensaje As String
    '    Dim clsFunciones As New clsFunciones
    '    Try
    '        If grvProducto.Rows.Count > 0 Then
    '            If txtMotivoo.Text.Replace(" ", "") <> "" Then
    '                Response.Redirect("wfrmRequisicionCompra.aspx?key=" & Request.QueryString("key").Replace("+", "%2B") & _
    '                          "&idrequisicion=" & ViewState("Solicitudes").Rows(0).item(0) & "&intidestatus=" & ViewState("Solicitudes").Rows(0).item(3) & "&Motivo=" & txtMotivoo.Text)
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
