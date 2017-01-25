Imports System.Data
Partial Class wfrmSolicitudesAprobadas
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim clsFunciones As New clsFunciones
            Dim dtsEstatus As New DataSet
            Dim strMensaje As String

            Dim filtroEstatus As String = ""
            Dim filtroDesde As String = ""
            Dim filtroHasta As String = ""
            Dim filtroEmpleado As Integer = -1

            filtroEstatus = ""
            filtroDesde = txtFechaDesde.Text
            filtroHasta = txtFechaHasta.Text
            filtroEmpleado = Session("intEmpleado")

            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(7) {New DataColumn("numsolicitud"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("proveedor"), New DataColumn("estatus"), New DataColumn("fecha"), New DataColumn("observaciones"), New DataColumn("seleccionar")})

            Dim dtsSolicitudes As New DataSet
            strMensaje = clsFunciones.Llena_Grid(grvSolicitudes, "GAMMA", dtsSolicitudes, "Solicitudes", "Cargar_Solicitudes", _
                       "A," & filtroDesde & "," & filtroHasta & "," & Session("intEmpleado"), _
                        "@vchIdEstatus,@vchFechaDesde,@vchFechaHasta,@intIdEmpleado")
            If strMensaje = "OK" Then

                ViewState("Solicitudes") = dtsSolicitudes.Tables("Solicitudes")
                grvSolicitudes.DataSource = dtsSolicitudes.Tables("Solicitudes")
                grvSolicitudes.DataBind()
                'ViewState("Productos") = Nothing
                'grvProducto.DataSource = Nothing
                'grvProducto.DataBind()
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar las Requisiciones: " & strMensaje.Replace("'", "") & "');", True)
            End If
        End If
    End Sub

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        Dim strMensaje As String = ""
        Dim clsFunciones As New clsFunciones
        Dim dtsSolicitudes As New DataSet
        txtServicio.Text = ""
        strMensaje = clsFunciones.Llena_Grid(grvSolicitudes, "GAMMA", dtsSolicitudes, "Solicitudes", "Cargar_Solicitudes", _
                   "A," & txtFechaDesde.Text.Replace("-", "") & "," & txtFechaHasta.Text.Replace("-", "") & "," & Session("intEmpleado"), _
                    "@vchIdEstatus,@vchFechaDesde,@vchFechaHasta,@intIdEmpleado")
        If strMensaje = "OK" Then

            ViewState("Solicitudes") = dtsSolicitudes.Tables("Solicitudes")
            grvSolicitudes.DataSource = dtsSolicitudes.Tables("Solicitudes")
            grvSolicitudes.DataBind()
            'ViewState("Productos") = Nothing
            'grvProducto.DataSource = Nothing
            'grvProducto.DataBind()
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar las Requisiciones: " & strMensaje.Replace("'", "") & "');", True)
        End If
    End Sub

    Protected Sub grvSolicitudes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvSolicitudes.RowCommand
        If (e.CommandName = "Seleccionar") Then
            ' Retrieve the row index stored in the CommandArgument property.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            hdnIdDepartamento.Value = ""
            ' Retrieve the row that contains the button 
            ' from the Rows collection.
            Dim rowen As GridViewRow = grvSolicitudes.Rows(index)
            txtServicio.Text = ViewState("Solicitudes").Rows(index).item(2)
            '    Dim dt As New DataTable()
            '    dt.Columns.AddRange(New DataColumn(9) {New DataColumn("numrequisicion"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("salida"), New DataColumn("intidestatus"), New DataColumn("cco_numero"), New DataColumn("cco_nombre"), New DataColumn("ordencompra"), New DataColumn("importe")})
            '    Dim clsFunciones As New clsFunciones
            '    Dim dtsProductos As New DataSet
            '    Dim strMensaje As String
            '    'strMensaje = clsFunciones.Llena_Grid(grvProducto, "MIGRACION", dtsProductos, "Productos", "Cargar_DetalleRequisiciones", _
            '    '                                     grvSolicitudes.Rows(index).Cells(0).Text & "," & ViewState("Solicitudes").Rows(index).item(7), _
            '    '                                     "@intIdRequisicion,@intIdEstatus")
            '    If strMensaje = "OK" Then
            '        hdnIdDepartamento.Value = ViewState("Solicitudes").Rows(index).item(8)
            '        Dim i As Integer
            '        For i = 0 To ViewState("Solicitudes").Rows.Count - 1
            '            grvSolicitudes.Rows(i).BackColor = Drawing.Color.White
            '        Next
            '        txtMotivoo.ReadOnly = False

            '        ViewState("Productos") = dtsProductos.Tables("Productos")
            '        grvProducto.DataSource = dtsProductos.Tables("Productos")
            '        grvProducto.DataBind()
            '        grvSolicitudes.Rows(index).BackColor = Drawing.Color.LightGreen
            '    Else
            '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar el detalle de la Requisicion: " & strMensaje.Replace("'", "") & "');", True)
            '    End If
        End If
    End Sub

    'Protected Sub btnAprobar_Click(sender As Object, e As EventArgs) Handles btnAprobar.Click
    '    Dim strMensaje As String
    '    Dim clsFunciones As New clsFunciones
    '    Try
    '        If grvProducto.Rows.Count > 0 Then
    '            strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Actualiza_Estatus_Requisicion", _
    '                        ViewState("Productos").Rows(0).item(0) & "," & ViewState("Productos").Rows(0).item(5) + 1 & "," & Session("IdUsuario") & ",Normal", _
    '                        "@intRequisicion,@intEstatus,@intIdUsuarioCaptura,@vchComentarios")
    '            'strMensaje = "OK"
    '            If strMensaje = "OK" Then
    '                Alert("Requisicion de almacen R" & ViewState("Productos").Rows(0).item(0) & " aprobada con exito")
    '                Dim i As Integer
    '                Dim acumulado As Double = 0
    '                For i = 0 To ViewState("Productos").Rows().count - 1
    '                    acumulado = acumulado + ViewState("Productos").Rows(i).item(11)
    '                Next
    '                If acumulado >= 10000 Then
    '                    Dim dtsDepartamentos As New DataSet
    '                    strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsDepartamentos, "Telefono", "Cargar_Departamentos", _
    '                                hdnIdDepartamento.Value, "@intDepartamento", dtsDepartamentos.Tables("Telefono"))
    '                    If strMensaje = "OK" Then 'Si se realizo la consulta del telefono
    '                        If dtsDepartamentos.Tables("Telefono").Rows(1).Item(5) <> "" Then
    '                            strMensaje = clsFunciones.EjecutaProcedimiento("SMS", "Agrega_BitacoraMensajes_Usuario", _
    '                                        "5,-" & dtsDepartamentos.Tables("Telefono").Rows(1).Item(5) & "-,,," & _
    '                                        "Avisos Salida de almacen R" & ViewState("Productos").Rows(0).item(0) & " aprobada por el Departamento " & dtsDepartamentos.Tables("Telefono").Rows(1).Item(1) & " por $" & CStr(acumulado) & " ,0", _
    '                                        "@intidOrigen,@vchTelefono,@vchFecha,@vchHora,@vchComando,@intidUsuario")
    '                        End If
    '                    End If
    '                End If
    '                If TimeOfDay.Hour >= 16 Or Now.DayOfWeek > 5 Then
    '                    strMensaje = clsFunciones.EjecutaProcedimiento("SMS", "Agrega_BitacoraMensajes_Usuario", _
    '                                "5,-6141788586-,,," & _
    '                                "Avisos Salida de almacen R" & ViewState("Productos").Rows(0).item(0) & " esperando validacion de almacen en http://201.147.15.182/,0", _
    '                                "@intidOrigen,@vchTelefono,@vchFecha,@vchHora,@vchComando,@intidUsuario")
    '                End If
    '                Timer1.Enabled = True
    '            Else
    '                Alert("Error al aprobar la requisicion R" & ViewState("Productos").Rows(0).item(0) & ": " & strMensaje)
    '            End If
    '        Else
    '            Alert("Favor de seleccionar Requisicion de Almacen a aprobar")
    '        End If

    '    Catch ex As Exception
    '        Alert("Error al guardar la autorizacion de la requisicion: " & ex.Message & ": ")
    '    End Try

    'End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Se redirecciona para aprobar otra requisicion de almacen
        Response.Redirect("wfrmSolicitudesAprobadas.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
    End Sub

    'Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
    '    Dim strMensaje As String
    '    Dim clsFunciones As New clsFunciones
    '    Try
    '        If grvProducto.Rows.Count > 0 Then
    '            If txtMotivoo.Text.Replace(" ", "") <> "" Then
    '                strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Actualiza_Estatus_Requisicion", _
    '                            ViewState("Productos").Rows(0).item(0) & ",6," & Session("IdUsuario") & "," & txtMotivoo.Text.Replace(",", "*"), _
    '                            "@intRequisicion,@intEstatus,@intIdUsuarioCaptura,@vchComentarios")
    '                If strMensaje = "OK" Then
    '                    Alert("Requisicion de almacen " & ViewState("Productos").Rows(0).item(0) & " cancelada con exito")
    '                    Timer1.Enabled = True
    '                Else
    '                    Alert("Error al cancelar la requisicion " & ViewState("Productos").Rows(0).item(0) & ": " & strMensaje)
    '                End If
    '            Else
    '                Alert("Favor de anotar el motivo de la cancelación")
    '            End If

    '        Else
    '            Alert("Favor de seleccionar Requisicion de Almacen a cancelar")
    '        End If

    '    Catch ex As Exception
    '        Alert("Error al cancelar la requisicion: " & ex.Message & ": ")
    '    End Try
    'End Sub

    'Protected Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
    '    Dim strMensaje As String
    '    Dim clsFunciones As New clsFunciones
    '    Try
    '        If grvProducto.Rows.Count > 0 Then
    '            If txtMotivoo.Text.Replace(" ", "") <> "" Then
    '                Response.Redirect("wfrmRequisicionAlmacen.aspx?key=" & Request.QueryString("key").Replace("+", "%2B") & _
    '                          "&idrequisicion=" & ViewState("Productos").Rows(0).item(0) & "&intidestatus=" & ViewState("Productos").Rows(0).item(5) & "&Motivo=" & txtMotivoo.Text)
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
