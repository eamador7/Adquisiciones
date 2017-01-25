Imports System.Data
Partial Class wfrmListaDetRequisiciones
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Session("caller") = "wfrmListaDetRequisiciones.aspx"
        If Not Page.IsPostBack Then

            txtTotal.Text = "0.00"
            Dim clsFunciones As New clsFunciones
            Dim dtsEstatus As New DataSet
            Dim strMensaje As String

            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(14) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("estatus"), New DataColumn("fecha"), New DataColumn("intidestatus"), New DataColumn("intdepartamento"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("ccoid"), New DataColumn("cconumero"), New DataColumn("nombreProveedor"), New DataColumn("seleccionar")})


            Dim dtsRequisiciones As New DataSet
            strMensaje = clsFunciones.Llena_Grid(grvSolicitudes, "MIGRACION", dtsRequisiciones, "Requisiciones", "Cargar_DetallesRequisicionCompra",
                        "," & txtFechaDesde.Text & "," & txtFechaHasta.Text & ",7,",
                        "@intIdRequisicion,@vchFechaDesde,@vchFechaHasta,@vchIdEstatus,@intIdSolicitante")
            If strMensaje = "OK" Then

                ViewState("Solicitudes") = dtsRequisiciones.Tables("Requisiciones")
                grvSolicitudes.DataSource = dtsRequisiciones.Tables("Requisiciones")
                grvSolicitudes.DataBind()
                ViewState("Detalle") = Nothing
                grvDetOrden.DataSource = Nothing
                grvDetOrden.DataBind()
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar las Requisiciones: " & strMensaje.Replace("'", "") & "');", True)
            End If
        End If
    End Sub


    Protected Sub btnAgregar_click(sender As Object, e As EventArgs) Handles btnAgregar.Click

        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn(8) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("requisicion"), New DataColumn("detallerequisicion"), New DataColumn("ccoid"), New DataColumn("cconumero")})
        Dim clsFunciones As New clsFunciones
        Dim dtsProductos As New DataSet

        Dim total As Decimal = CDec(txtTotal.Text)
        If Not ViewState("Detalle") Is Nothing Then
            dt = ViewState("Detalle")
        End If

        If hdnProveedorOrden.Value = "" Then
            hdnProveedorOrden.Value = hdnProveedorRequi.Value
        End If

        If hdnProveedorOrden.Value <> hdnProveedorRequi.Value Then
            Alert("No es posible mezclar proveedores dentro de una orden ")
            Exit Sub
        End If

        'Dim i As Integer
        'For i = 0 To grvDetOrden.Rows.Count - 1
        '    If ViewState("Solicitudes").Rows(hdnRequisicionSelIndex.Value).Item(0) = grvDetOrden.Rows(i).Cells(5).Text And ViewState("Solicitudes").Rows(hdnRequisicionSelIndex.Value).Item(1) = grvDetOrden.Rows(i).Cells(6).Text Then
        '        Alert("El artículo " & ViewState("Solicitudes").Rows(hdnRequisicionSelIndex).Item(3) & " ya fue agregado previamente")
        '        Exit Sub
        '    End If
        'Next

        dt.Rows.Add(grvDetOrden.Rows.Count + 1, hdnCodigo.Value, hdnConcepto.Value, hdnCantidad.Value,
                    hdnPrecio.Value, hdnRequisicion.Value, hdnDetallerequisicion.Value, hdnCcoId.Value, hdnCcoNumero.Value)
        ViewState("Detalle") = dt
        grvDetOrden.DataSource = dt
        grvDetOrden.DataBind()

        total += CDec(hdnCantidad.Value * hdnPrecio.Value)
        txtTotal.Text = total.ToString("###,###,##0.00")

        Dim dt2 As New DataTable()
        If Not ViewState("Solicitudes") Is Nothing Then
            dt2 = ViewState("Solicitudes")
        End If
        dt2.Rows.RemoveAt(hdnRequisicionSelIndex.Value)
        ' dt2.Rows(hdnRequisicionSelIndex.Value).Delete()
        ViewState("Solicitudes") = dt2
        grvSolicitudes.DataSource = dt2
        grvSolicitudes.DataBind()

        
    End Sub

    Protected Sub grvDetOrden_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvDetOrden.RowCommand
        If (e.CommandName = "Eliminar") Then
            ' Obtenemos la fila seleccionada
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            'Cargamos los articulos a un datatable para eliminarlo
            Dim dt As New DataTable()
            If Not ViewState("Detalle") Is Nothing Then
                dt = ViewState("Detalle")
            End If
            dt.Rows(index).Delete()
            If dt.Rows.Count <= 0 Then
                hdnProveedorOrden.Value = ""
            End If
            ViewState("Detalle") = dt
            grvDetOrden.DataSource = dt
            grvDetOrden.DataBind()
        End If
    End Sub

    Protected Sub grvSolicitudes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvSolicitudes.RowCommand
        If (e.CommandName = "Seleccionar") Then
            ' Retrieve the row index stored in the CommandArgument property.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            ' Retrieve the row that contains the button 
            ' from the Rows collection.


            hdnRequisicionSelIndex.Value = index
            hdnCodigo.Value = ViewState("Solicitudes").Rows(index).Item(2)
            hdnConcepto.Value = ViewState("Solicitudes").Rows(index).Item(3)
            hdnCantidad.Value = ViewState("Solicitudes").Rows(index).Item(4)
            hdnPrecio.Value = ViewState("Solicitudes").Rows(index).Item(5)
            hdnRequisicion.Value = ViewState("Solicitudes").Rows(index).Item(0)
            hdnDetallerequisicion.Value = ViewState("Solicitudes").Rows(index).Item(1)
            hdnCcoId.Value = ViewState("Solicitudes").Rows(index).Item(6)
            hdnCcoNumero.Value = ViewState("Solicitudes").Rows(index).Item(7)
            hdnProveedorRequi.Value = ViewState("Solicitudes").Rows(index).Item(9)
            Dim i As Integer
            For i = 0 To ViewState("Solicitudes").Rows.Count - 1
                grvSolicitudes.Rows(i).BackColor = Drawing.Color.White
            Next


            grvSolicitudes.Rows(index).BackColor = Drawing.Color.LightGreen

        End If
    End Sub

    Protected Sub btnGenerarOrden_click(sender As Object, e As EventArgs) Handles btnGenerarOrden.Click
        Dim strMensaje As String
        Dim clsFunciones As New clsFunciones
        Dim dtsOrden As New DataSet
        Dim dtTmp As New DataTable()
        Try
            If grvDetOrden.Rows.Count > 0 Then
                If txtPromesa.Text <> "" Then


                    'Creamos un rango de columnas para la datatable temporal que almacenara los articulos
                    dtTmp.Columns.AddRange(New DataColumn(11) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("impuesto"), New DataColumn("totalart"), New DataColumn("recibido"), New DataColumn("ccoid"), New DataColumn("cconumero"), New DataColumn("idRequisicion"), New DataColumn("idDetRequisicion")})

                    For intContador = 0 To ViewState("Detalle").Rows.Count - 1 'Recorremos el ViewState para obtener los articulos
                        dtTmp.Rows.Add(CInt(ViewState("Detalle").Rows(intContador).Item(0)),
                                       CInt(ViewState("Detalle").Rows(intContador).Item(1)),
                                       ViewState("Detalle").Rows(intContador).Item(2),
                                       CDec(ViewState("Detalle").Rows(intContador).Item(3)),
                                       CDec(ViewState("Detalle").Rows(intContador).Item(4)),
                                       0,
                                       CDec(ViewState("Detalle").Rows(intContador).Item(3) * ViewState("Detalle").Rows(intContador).Item(4)),
                                       0,
                                       CInt(ViewState("Detalle").Rows(intContador).Item(7)),
                                       ViewState("Detalle").Rows(intContador).Item(8),
                                       CInt(ViewState("Detalle").Rows(intContador).Item(5)),
                                       CInt(ViewState("Detalle").Rows(intContador).Item(6))
                                       )

                    Next
                    btnGenerarOrden.Enabled = False
                    'Mandamos guardar la solicitud con sus articulos
                    strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsOrden, "Orden", "Inserta_OrdenSinContrato",
                                 hdnOrden.Value & "," & Session("IdUsuario") & "," & hdnProveedorOrden.Value & ",A," &
                                 "E," & txtObservaciones.Text.ToUpper.Replace(",", "*") & "," & CDec(txtTotal.Text) & "," & txtPromesa.Text.Replace("-"c, ""),
                                 "@intIdOrden,@intSolicitante,@intProveedor,@chEstatus,@chSerie,@vchObservaciones,@decTotal,@fechaPromesa",
                                 dtTmp)
                    If strMensaje = "OK" Then

                        For Each rowen As GridViewRow In grvDetOrden.Rows
                            strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Actualiza_Estatus_RequisicionCompra",
                                       rowen.Cells(5).Text & "," & rowen.Cells(6).Text & ",7," & Session("IdUsuario") & "," & txtObservaciones.Text & "," & rowen.Cells(4).Text,
                                        "@intRequisicion,@intDetRequisicion,@intEstatus,@intIdUsuarioCaptura,@vchComentarios,@decPrecio")

                            If strMensaje = "OK" Then


                            Else
                                Alert("Error al guardar cambios " & strMensaje)
                                Exit Sub
                            End If

                        Next

                        ifrOrdenCompra.Src = "wfrmReporteOrdenCompra.aspx?key=" & Request.QueryString("key").Replace("+", "%2B") &
                        "&intOrden=" & dtsOrden.Tables("Orden").Rows(0).Item(0).ToString
                        Alert("Se ha generado la orden de compra E" & dtsOrden.Tables("Orden").Rows(0).Item(0).ToString & "")

                        Timer1.Enabled = True
                    Else
                        Alert("Error al generar la orden de compra: " & strMensaje)
                    End If
                Else
                    Alert("Debe especificar una fecha promesa")
                End If
            Else
                Alert("Favor de agregar datos a la orden")
            End If

        Catch ex As Exception
            Alert("Error al guardar la autorizacion de la requisicion: " & ex.Message & ":  ")

        End Try
    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Se redirecciona para aprobar otra requisicion de almacen
        Response.Redirect("wfrmListaDetRequisiciones.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("wfrmListaDetRequisiciones.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
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
