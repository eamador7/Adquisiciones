Imports System.Data
Imports clsFunciones


Partial Class wfrmAprobarDetRequisicionesCompraCopy
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Usuario y perfil de prueba
        Session("IdUsuario") = 4
        Session("idPerfil") = 24
        Session("caller") = "wfrmAprobarDetRequisicionesCompra.aspx"
        grvSolicitudes.Columns(6).Visible = False
        If Not Page.IsPostBack Then
            Alert("Recuerde que todas los artículos de las requisiciones utilizadas que no sean autorizados se cancelaran automáticamente.", Me, 1, 5)
            Dim clsFunciones As New clsFunciones
            Dim dtsEstatus As New DataSet
            Dim strMensaje As String

            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(13) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus")})


            Dim filterEstatus As String = ""
            hdnProfileId.Value = Session("idPerfil")
            ' 22 - Jefe de departamento | 24 - Director correspondiente | 26 - Director Administrativo
            If hdnProfileId.Value <> "" Then
                Select Case hdnProfileId.Value
                    Case 22
                        filterEstatus = "1"

                    Case 24
                        filterEstatus = "5"
                    Case 26
                        filterEstatus = "5-6"
                    Case Else
                        filterEstatus = "1"
                End Select
            End If

            Dim dtsSolicitudes As New DataSet
            'strMensaje = clsFunciones.Llena_Grid(grvSolicitudes, "MIGRACION", dtsRequisiciones, "Requisiciones", "Cargar_RequisicionesCompra", _
            '            txtRequisicion.Text & "," & filterEstatus & "," & txtFechaDesde.Text & "," & txtFechaHasta.Text & "," & Session("departamento") & ",0", _
            '            "@intIdRequisicion,@vchIdEstatus,@vchFechaDesde,@vchFechaHasta,@intDepartamento,@intConContrato")

            strMensaje = clsFunciones.Llena_Grid(grvSolicitudes, "MIGRACION", dtsSolicitudes, "Solicitudes", "Cargar_RequisicionesCompra",
            txtRequisicion.Text & "," & filterEstatus & "," & txtFechaDesde.Text & "," & txtFechaHasta.Text & "," & Session("departamento") & ",1",
            "@intIdRequisicion,@vchIdEstatus,@vchFechaDesde,@vchFechaHasta,@intDepartamento")

            If strMensaje = "OK" Then
                dtsSolicitudes.Tables("Solicitudes").Columns.Add("total").DefaultValue = 0
                ViewState("Solicitudes") = dtsSolicitudes.Tables("Solicitudes")
                grvSolicitudes.DataSource = dtsSolicitudes.Tables("Solicitudes")
                grvSolicitudes.DataBind()
                ViewState("Requisiciones") = Nothing
                grvRequisiciones.DataSource = Nothing
                grvRequisiciones.DataBind()
                ViewState("Detalle") = Nothing
                grvAprobadas.DataSource = Nothing
                grvAprobadas.DataBind()
                'Canceladas manual
                'ViewState("Detalle2") = Nothing
                'grvCanceladas.DataSource = Nothing
                'grvCanceladas.DataBind()
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar las Requisiciones: " & strMensaje.Replace("'", "") & "');", True)
            End If

            '------------------------------------------------------Lista de detalle-------------------------------------------------------------
            'Dim dtsRequisiciones As New DataSet
            'strMensaje = clsFunciones.Llena_Grid(grvRequisiciones, "MIGRACION", dtsRequisiciones, "Requisiciones", "Cargar_DetallesRequisicionCompraAprobacion", _
            '            "-1," & txtFechaDesde.Text & "," & txtFechaHasta.Text & "," & filterEstatus & "," & Session("departamento"), _
            '            "@intIdRequisicion,@vchFechaDesde,@vchFechaHasta,@vchIdEstatus,@intDepartamento")
            'If strMensaje = "OK" Then

            '    ViewState("Requisiciones") = dtsRequisiciones.Tables("Requisiciones")
            '    grvRequisiciones.DataSource = dtsRequisiciones.Tables("Requisiciones")
            '    grvRequisiciones.DataBind()
            '    ViewState("Detalle") = Nothing
            '    grvAprobadas.DataSource = Nothing
            '    grvAprobadas.DataBind()
            '    ViewState("Detalle2") = Nothing
            '    grvCanceladas.DataSource = Nothing
            '    grvCanceladas.DataBind()
            'Else
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar las Requisiciones: " & strMensaje.Replace("'", "") & "');", True)
            'End If
            '------------------------------------------------------Lista de detalle-------------------------------------------------------------
        End If
    End Sub

    Protected Sub grvSolicitudes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvSolicitudes.RowCommand
        If (e.CommandName = "Seleccionar") Then
            ' Retrieve the row index stored in the CommandArgument property.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            ' Retrieve the row that contains the button 
            ' from the Rows collection.
            Dim row As GridViewRow = grvSolicitudes.Rows(index)
            hdnIdRequisicionSel.Value = grvSolicitudes.Rows(index).Cells(0).Text
            hdnRequiIndex.Value = index
            Dim clsFunciones As New clsFunciones

            Dim strMensaje As String
            Dim dtsRequisiciones As New DataSet

            Dim filterEstatus As String = ""
            hdnProfileId.Value = Session("idPerfil")
            ' 22 - Jefe de departamento | 24 - Director correspondiente | 26 - Director Administrativo
            If hdnProfileId.Value <> "" Then
                Select Case hdnProfileId.Value
                    Case 22
                        filterEstatus = "1"
                        grvRequisiciones.Columns(5).Visible = False
                        grvSolicitudes.Columns(6).Visible = False
                    Case 24
                        filterEstatus = "5"
                        grvRequisiciones.Columns(5).Visible = True
                        grvSolicitudes.Columns(6).Visible = True
                    Case 26
                        filterEstatus = "5-6"
                        grvRequisiciones.Columns(5).Visible = True
                        grvSolicitudes.Columns(6).Visible = True
                    Case Else
                        filterEstatus = "1"
                End Select
            End If

            strMensaje = clsFunciones.Llena_Grid(grvRequisiciones, "MIGRACION", dtsRequisiciones, "Requisiciones", "Cargar_DetallesRequisicionCompraAprobacion",
                       hdnIdRequisicionSel.Value.ToString & ", , , " & filterEstatus & "," & Session("departamento"),
                        "@intIdRequisicion,@vchFechaDesde,@vchFechaHasta,@vchIdEstatus,@intDepartamento")

            If strMensaje = "OK" Then

                Dim i As Integer
                For i = 0 To ViewState("Solicitudes").Rows.Count - 1
                    grvSolicitudes.Rows(i).BackColor = Drawing.Color.White
                Next

                ViewState("Solicitudes").rows(index).item(12) = 0.00
                ViewState("Requisiciones") = dtsRequisiciones.Tables("Requisiciones")
                grvRequisiciones.DataSource = dtsRequisiciones.Tables("Requisiciones")
                grvRequisiciones.DataBind()
                Dim dt As DataTable
                dt = ViewState("Solicitudes")
                grvSolicitudes.DataSource = dt
                grvSolicitudes.DataBind()
                grvSolicitudes.Rows(index).BackColor = Drawing.Color.LightBlue
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar el detalle de la Requisicion: " & strMensaje.Replace("'", "") & "');", True)
            End If
        End If
    End Sub


    Protected Sub grvRequisiciones_rowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvRequisiciones.RowCommand
        If (e.CommandName = "Aprobar") Then
            Dim dtAprobadas As New DataTable()
            dtAprobadas.Columns.AddRange(New DataColumn(13) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus")})

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim dtRequis As New DataTable()
            If Not ViewState("Requisiciones") Is Nothing Then
                dtRequis = ViewState("Requisiciones")
            End If

            If Not ViewState("Detalle") Is Nothing Then
                dtAprobadas = ViewState("Detalle")
            End If

            'Recorrer el detalle verificando que no se haya duplicado ningun elemento
            Dim i As Integer
            For i = 0 To grvAprobadas.Rows.Count - 1
                If ViewState("Requisiciones").Rows(index).Item(0) = grvAprobadas.Rows(i).Cells(0).Text And ViewState("Requisiciones").Rows(index).Item(1) = grvAprobadas.Rows(i).Cells(1).Text Then
                    Alert("El elemento ya fue agregado previamente", Me, 1, 3)
                    Exit Sub
                End If
            Next
            'Canceladas manualmente
            'For i = 0 To grvCanceladas.Rows.Count - 1
            '    If ViewState("Requisiciones").Rows(index).Item(0) = grvCanceladas.Rows(i).Cells(0).Text And ViewState("Requisiciones").Rows(index).Item(1) = grvCanceladas.Rows(i).Cells(1).Text Then
            '        Alert("El elemento ya fue agregado previamente")
            '        Exit Sub
            '    End If
            'Next

            'Incrementar lo seleccionado
            ViewState("Solicitudes").rows(hdnRequiIndex.Value).item(12) += (ViewState("Requisiciones").Rows(index).Item(4) * ViewState("Requisiciones").Rows(index).Item(5))

            Dim dt As DataTable = ViewState("Solicitudes")
            grvSolicitudes.DataSource = dt
            grvSolicitudes.DataBind()

            dtAprobadas.Rows.Add(dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray)
            ViewState("Detalle") = dtAprobadas
            grvAprobadas.DataSource = dtAprobadas
            grvAprobadas.DataBind()

            dtRequis.Rows.RemoveAt(CInt(e.CommandArgument))
            ViewState("Requisiciones") = dtRequis
            grvRequisiciones.DataSource = dtRequis
            grvRequisiciones.DataBind()

        ElseIf (e.CommandName = "Cancelar") Then
            'Dim dtCanceladas As New DataTable()
            'dtCanceladas.Columns.AddRange(New DataColumn(13) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus")})
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim dtRequis As New DataTable()
            If Not ViewState("Requisiciones") Is Nothing Then
                dtRequis = ViewState("Requisiciones")
            End If

            'If Not ViewState("Detalle2") Is Nothing Then
            '    dtCanceladas = ViewState("Detalle2")
            'End If
            'Recorrer el detalle verificando que no se haya duplicado ningun elemento
            Dim i As Integer
            For i = 0 To grvAprobadas.Rows.Count - 1
                If ViewState("Requisiciones").Rows(index).Item(0) = grvAprobadas.Rows(i).Cells(0).Text And ViewState("Requisiciones").Rows(index).Item(1) = grvAprobadas.Rows(i).Cells(1).Text Then
                    Alert("El elemento ya fue agregado previamente", Me, 1, 3)
                    Exit Sub
                End If
            Next
            'Canceladas manualmente
            'For i = 0 To grvCanceladas.Rows.Count - 1
            '    If ViewState("Requisiciones").Rows(index).Item(0) = grvCanceladas.Rows(i).Cells(0).Text And ViewState("Requisiciones").Rows(index).Item(1) = grvCanceladas.Rows(i).Cells(1).Text Then
            '        Alert("El elemento ya fue agregado previamente")
            '        Exit Sub
            '    End If
            'Next


            'dtCanceladas.Rows.Add(dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray)

            'ViewState("Detalle2") = dtCanceladas
            'grvCanceladas.DataSource = dtCanceladas
            'grvCanceladas.DataBind()


            dtRequis.Rows.RemoveAt(CInt(e.CommandArgument))
            ViewState("Requisiciones") = dtRequis
            grvRequisiciones.DataSource = dtRequis
            grvRequisiciones.DataBind()
        End If

    End Sub

    Protected Sub grvAprobadas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvAprobadas.RowCommand
        If (e.CommandName = "Eliminar") Then
            Dim dtRequis As New DataTable()
            dtRequis.Columns.AddRange(New DataColumn(13) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus")})


            Dim dtAprobadas As New DataTable()
            If Not ViewState("Detalle") Is Nothing Then
                dtAprobadas = ViewState("Detalle")
            End If

            If Not ViewState("Requisiciones") Is Nothing Then
                dtRequis = ViewState("Requisiciones")
            End If

            'Regresa a la lista de la parte superior si se trata de la misma requisicion, de lo contrario, unicamente se remueve (simulando que volvio a la req origen)
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim aRemoverId As String = grvAprobadas.Rows(index).Cells(0).Text
            If hdnIdRequisicionSel.Value = aRemoverId Then
                dtRequis.Rows.Add(dtAprobadas.Rows.Item(CInt(e.CommandArgument)).ItemArray)
                ViewState("Requisiciones") = dtRequis
                grvRequisiciones.DataSource = dtRequis
                grvRequisiciones.DataBind()
            End If


            dtAprobadas.Rows.RemoveAt(CInt(e.CommandArgument))
            ViewState("Detalle") = dtAprobadas
            grvAprobadas.DataSource = dtAprobadas
            grvAprobadas.DataBind()

        End If
    End Sub

    'Protected Sub grvCanceladas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvCanceladas.RowCommand
    '    If (e.CommandName = "Eliminar") Then
    '        Dim dtRequis As New DataTable()
    '        dtRequis.Columns.AddRange(New DataColumn(13) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus")})


    '        Dim dtCanceladas As New DataTable()
    '        If Not ViewState("Detalle2") Is Nothing Then
    '            dtCanceladas = ViewState("Detalle2")
    '        End If

    '        If Not ViewState("Requisiciones") Is Nothing Then
    '            dtRequis = ViewState("Requisiciones")
    '        End If

    '        'Regresa a la lista de la parte superior si se trata de la misma requisicion, de lo contrario, unicamente se remueve (simulando que volvio a la req origen)
    '        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
    '        Dim aRemoverId As String = grvCanceladas.Rows(index).Cells(0).Text
    '        If hdnIdRequisicionSel.Value = aRemoverId Then
    '            dtRequis.Rows.Add(dtCanceladas.Rows.Item(CInt(e.CommandArgument)).ItemArray)
    '            ViewState("Requisiciones") = dtRequis
    '            grvRequisiciones.DataSource = dtRequis
    '            grvRequisiciones.DataBind()
    '        End If

    '        dtCanceladas.Rows.RemoveAt(CInt(e.CommandArgument))
    '        ViewState("Detalle2") = dtCanceladas
    '        grvCanceladas.DataSource = dtCanceladas
    '        grvCanceladas.DataBind()
    '    End If
    'End Sub


    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Response.Redirect("wfrmAprobarDetRequisicionesCompra.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim strMensaje As String
        Dim clsFunciones As New clsFunciones
        Dim dtsCotiza As New DataSet
        Dim dtTmp As New DataTable()
        Dim listaCancelar As String = ""
        Try
            If grvAprobadas.Rows.Count > 0 Then
                'If grvAprobadas.Rows.Count > 0 Or grvCanceladas.Rows.Count > 0 Then
                Dim filterEstatus As String = ""
                ' 22 - Jefe de departamento | 24 - Director correspondiente | 26 - Director Administrativo
                If hdnProfileId.Value <> "" Then
                    Select Case hdnProfileId.Value
                        Case 22
                            filterEstatus = "2"
                        Case 24
                            filterEstatus = "6"
                        Case 26
                            filterEstatus = "7"
                        Case Else
                            filterEstatus = "2"
                    End Select
                End If
                Dim estatusAdd As String = ""

                For Each rowen As GridViewRow In grvAprobadas.Rows
                    ' Si es una requisicion con contrato debe pasarse a comité ya cotizado
                    If ViewState("Detalle").Rows(rowen.RowIndex).Item(8) <> 0 Then
                        If filterEstatus = "2" Then
                            estatusAdd = "4"
                        Else
                            estatusAdd = filterEstatus
                        End If
                    Else
                        estatusAdd = filterEstatus
                    End If
                    'Recopilar una lista de las requisiciones para insertar cancelaciones de los detalles no autorizados
                    listaCancelar &= If(listaCancelar = "", If(listaCancelar.Contains(rowen.Cells(0).Text), "", rowen.Cells(0).Text.ToString()), If(listaCancelar.Contains(rowen.Cells(0).Text), "", "|" & rowen.Cells(0).Text.ToString()))

                    strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Actualiza_Estatus_RequisicionCompra",
                           rowen.Cells(0).Text & "," & rowen.Cells(1).Text & "," & estatusAdd.Trim & "," & Session("IdUsuario") & ",Normal,0",
                            "@intRequisicion,@intDetRequisicion,@intEstatus,@intIdUsuarioCaptura,@vchComentarios,@decPrecio")

                    If strMensaje = "OK" Then
                        ' Se guarda cotizacion en caso de ser de contrato
                        If estatusAdd = "5" Then
                            strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsCotiza, "Cot", "Inserta_detRequisicionCompraCotizacion",
                       rowen.Cells(0).Text & "," & rowen.Cells(1).Text & "," & rowen.Cells(2).Text & "," & ViewState("Detalle").Rows(rowen.RowIndex).Item(5).ToString & "," & ViewState("Detalle").Rows(rowen.RowIndex).Item(9).ToString,
                        "@intIdRequi,@intIdDetRequi,@intIdArticulo,@decPrecio,@intIdProveedor",
                        dtTmp)
                            If strMensaje = "OK" Then


                            Else
                                Alert("Error al guardar cotizacion: " & strMensaje, Me, 1, 4)
                                Exit Sub
                            End If
                        End If

                    Else
                        Alert("Error al guardar cambios " & strMensaje, Me, 1, 4)
                        Exit Sub
                    End If

                Next
                'Canceladas manualmente
                'For Each rowen As GridViewRow In grvCanceladas.Rows
                '    strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Actualiza_Estatus_RequisicionCompra",
                '           rowen.Cells(0).Text & "," & rowen.Cells(1).Text & ",9," & Session("IdUsuario") & "," & txtObservaciones.Text & ",0",
                '            "@intRequisicion,@intDetRequisicion,@intEstatus,@intIdUsuarioCaptura,@vchComentarios,@decPrecio")

                '    If strMensaje = "OK" Then


                '    Else
                '        Alert("Error al guardar cambios " & strMensaje)
                '        Exit Sub
                '    End If

                'Next
                'strMensaje = "OK"

                'Cancelar articulos no autorizados
                If listaCancelar <> "" Then
                    strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Cancela_DetRequisicionCompra",
                             Session("IdUsuario") & "," & listaCancelar,
                                "@intIdUsuarioCaptura,@vchListaRequisiciones")

                    If strMensaje = "OK" Then


                    Else
                        Alert("Error al guardar cambios " & strMensaje, Me)
                        Exit Sub
                        '    End If
                    End If
                End If


                Alert("Se han autorizado los articulos seleccionados, el resto se ha cancelado.", Me, 1, 2)
                Timer1.Enabled = True

            Else
                Alert("Favor de seleccionar Requisicion de compra a aprobar", Me, 1, 3)
            End If

        Catch ex As Exception
            Alert("Error al guardar cambios: " & ex.Message & ": ", Me, 1, 4)
        End Try
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("wfrmAprobarDetRequisicionesCompra.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
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
                Alert("Error al enviar el correo de confirmacion:" & ex.Message, Me, 1, 4)
                'MsgBox(ex.ToString)
            End Try
        Catch ex As Exception
            Alert("Error al enviar el correo de confirmacion:" & ex.Message, Me, 1, 4)
        End Try
    End Sub

    'Private Sub Alert(ByVal strMensaje As String)
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('" & strMensaje & "');", True)
    'End Sub

#End Region



End Class
