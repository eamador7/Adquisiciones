Imports System.Data
Imports clsFunciones
Imports CrystalDecisions.ReportSource

Partial Class wfrmCuadroFrio
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Usuario prueba
        'Session("IdUsuario") = 4
        If Not Page.IsPostBack Then

            Dim clsFunciones As New clsFunciones
            Dim dtsEstatus As New DataSet
            Dim strMensaje As String

            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(13) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus")})




            Dim dtsRequisiciones As New DataSet

            strMensaje = clsFunciones.Llena_Grid(grvRequisiciones, "MIGRACION", dtsRequisiciones, "Requisiciones", "Cargar_RequisicionesCompra",
             ",3,,,",
            "@intIdRequisicion,@vchIdEstatus,@vchFechaDesde,@vchFechaHasta,@intDepartamento")

            If strMensaje = "OK" Then

                ViewState("Requisiciones") = dtsRequisiciones.Tables("Requisiciones")
                grvRequisiciones.DataSource = dtsRequisiciones.Tables("Requisiciones")
                grvRequisiciones.DataBind()
                ViewState("Selected") = Nothing
                grvSelected.DataSource = Nothing
                grvSelected.DataBind()
                ViewState("Articulos") = Nothing
                grvDetalle.DataSource = Nothing
                grvDetalle.DataBind()
                ViewState("Cotizaciones") = Nothing
                grvCotizaciones.DataSource = Nothing
                grvCotizaciones.DataBind()

            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar las Requisiciones: " & strMensaje.Replace("'", "") & "');", True)
            End If

        End If
    End Sub

    Protected Sub grvRequisiciones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvRequisiciones.RowCommand
        If (e.CommandName = "Seleccionar") Then
            ' Retrieve the row index stored in the CommandArgument property.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            ' Retrieve the row that contains the button 
            ' from the Rows collection.
            Dim row As GridViewRow = grvRequisiciones.Rows(index)
            hdnIdRequisicionSel.Value = grvRequisiciones.Rows(index).Cells(0).Text
            Dim clsFunciones As New clsFunciones

            Dim strMensaje As String
            Dim dtsDetalle As New DataSet

            Dim dtRequis As New DataTable()
            If Not ViewState("Requisiciones") Is Nothing Then
                dtRequis = ViewState("Requisiciones")
            End If
            Dim dtSelected As New DataTable()

            For Each rowsel As GridViewRow In grvRequisiciones.Rows
                rowsel.BackColor = Drawing.Color.White
            Next
            row.BackColor = Drawing.Color.LightBlue
            hdnDetalleIndex.Value = index

            dtSelected.Columns.AddRange(New DataColumn(4) {New DataColumn("numrequisicion"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("total")})
            dtSelected.Rows.Add(dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray(0),
                                            dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray(1),
                                            dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray(3),
                                            dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray(4),
                                            0)
            ViewState("Selected") = dtSelected
            grvSelected.DataSource = dtSelected
            grvSelected.DataBind()

            pnlRequis.Visible = False
            pnlSelected.Visible = True
            btnCancelar.Visible = False

            strMensaje = clsFunciones.Llena_Grid(grvRequisiciones, "MIGRACION", dtsDetalle, "Detalle", "Cargar_DetallesRequisicionCompraAprobacion",
                       hdnIdRequisicionSel.Value.ToString & ", , , 3,",
                        "@intIdRequisicion,@vchFechaDesde,@vchFechaHasta,@vchIdEstatus,@intDepartamento")

            If strMensaje = "OK" Then

                cbComite.Visible = True
                btnCancelar.Visible = True
                btnGuardar.Visible = True

                ViewState("Detalle") = dtsDetalle.Tables("Detalle")
                grvDetalle.DataSource = dtsDetalle.Tables("Detalle")
                grvDetalle.DataBind()


                pnlDetalle.Visible = True
                Dim detailIndex As Integer = 0
                'Obtener las cotizaciones de todos los detalles
                For Each rowen As GridViewRow In grvDetalle.Rows
                    Dim dtsCotizaciones As New DataSet
                    strMensaje = clsFunciones.Llena_Grid(grvCotizaciones, "MIGRACION", dtsCotizaciones, "Cotizaciones" & detailIndex.ToString(), "Cargar_Cotizaciones",
                             rowen.Cells(0).Text & "," & rowen.Cells(1).Text & ",2-3",
                                "@intIdRequisicion,@intIdDetRequisicion,@vchIdEstatus")
                    If strMensaje = "OK" Then
                        Dim dummyTab As DataTable = dtsCotizaciones.Tables("Cotizaciones" & detailIndex.ToString())
                        dummyTab.Columns.Add("seleccionada")
                        ViewState("Cotizaciones" & detailIndex.ToString()) = dummyTab

                        detailIndex += 1
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar las cotizaciones: " & strMensaje.Replace("'", "") & "');", True)
                    End If
                Next
                'Se recorren las cotizaciones para seleccionar la mas optima segun el precio mas bajo
                For currentIndex = 0 To detailIndex - 1
                    Dim menor As Decimal = 0
                    Dim indice As Integer = 0
                    Dim indiceMenor As Integer = -1
                    For Each theRow In ViewState("Cotizaciones" & currentIndex).Rows()
                        If menor = 0 Then
                            menor = CDec(theRow.item(5))
                            indiceMenor = 0

                        ElseIf CDec(theRow.item(5)) < menor Then
                            menor = CDec(theRow.item(5))
                            indiceMenor = indice
                        End If
                        theRow.item(10) = 0 'Se marcan todos para que no estén seleccionados
                        indice += 1
                    Next
                    If ViewState("Cotizaciones" & currentIndex).Rows.count > 0 Then
                        ViewState("Cotizaciones" & currentIndex).Rows(indiceMenor).item(10) = 1 'Se marca el menor como seleccionado
                        ViewState("Selected").rows(0).item(4) += (menor * ViewState("Detalle").Rows(currentIndex).item(4))
                    End If
                Next

                dtSelected = ViewState("Selected")
                grvSelected.DataSource = dtSelected
                grvSelected.DataBind()
                grvSelected.Rows(index).BackColor = Drawing.Color.LightBlue
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar el detalle de la Requisicion: " & strMensaje.Replace("'", "") & "');", True)
            End If

        End If
    End Sub


    Protected Sub grvDetalle_rowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvDetalle.RowCommand
        If (e.CommandName = "Seleccionar") Then
            ' Retrieve the row index stored in the CommandArgument property.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            ' Retrieve the row that contains the button 
            ' from the Rows collection.

            hdnDetallerequisicion.Value = grvDetalle.Rows(index).Cells(1).Text
            Dim clsFunciones As New clsFunciones

            Dim strMensaje As String


            'Se cargan las cotizaciones ya guardadas

            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(11) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("numeroproveedor"), New DataColumn("nombreproveedor"), New DataColumn("precio"), New DataColumn("requisicion"), New DataColumn("detallerequisicion"), New DataColumn("marca"), New DataColumn("tiempoentrega"), New DataColumn("intEstatus"), New DataColumn("seleccionada")})


            If Not ViewState("Cotizaciones" & index) Is Nothing Then
                dt = ViewState("Cotizaciones" & index)
            End If
            hdnCotizacionIndex.Value = index

            grvCotizaciones.DataSource = dt
            grvCotizaciones.DataBind()
            cotizacionesPanel.Visible = True
            Dim indiceRenglon As Integer = 0
            For Each rowen As GridViewRow In grvCotizaciones.Rows
                If ViewState("Cotizaciones" & index).rows(indiceRenglon).item(10) = 1 Then
                    rowen.BackColor = Drawing.Color.LightBlue
                End If
                indiceRenglon += 1
            Next

            For Each rowen As GridViewRow In grvDetalle.Rows
                rowen.BackColor = Drawing.Color.White
            Next
            grvDetalle.Rows(index).BackColor = Drawing.Color.LightBlue

            'Dim dtsCotizaciones As New DataSet
            'strMensaje = clsFunciones.Llena_Grid(grvCotizaciones, "MIGRACION", dtsCotizaciones, "Cotizaciones", "Cargar_Cotizaciones",
            '          hdnIdRequisicionSel.Value & "," & hdnDetallerequisicion.Value,
            '            "@intIdRequisicion,@intIdDetRequisicion")
            'If strMensaje = "OK" Then

            '    ViewState("Cotizaciones") = dtsCotizaciones.Tables("Cotizaciones")
            '    grvCotizaciones.DataSource = dtsCotizaciones.Tables("Cotizaciones")
            '    grvCotizaciones.DataBind()
            '    cotizacionesPanel.Visible = True
            'Else
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('Error al cargar las cotizaciones: " & strMensaje.Replace("'", "") & "');", True)
            'End If

        End If
    End Sub



    Protected Sub grvCotizaciones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvCotizaciones.RowCommand
        If (e.CommandName = "Seleccionar") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            'Descontar el previamente seleccionado
            For Each theRow In ViewState("Cotizaciones" & hdnCotizacionIndex.Value.ToString()).Rows()
                If theRow.item(10) = 1 Then

                    ViewState("Selected").rows(0).item(4) -= (theRow.item(5) * ViewState("Detalle").Rows(hdnDetalleIndex.Value).item(4))
                    theRow.item(10) = 0
                End If
            Next
            ViewState("Cotizaciones" & hdnCotizacionIndex.Value.ToString()).Rows(index).item(10) = 1
            ViewState("Selected").rows(0).item(4) += CDec(ViewState("Cotizaciones" & hdnCotizacionIndex.Value.ToString()).Rows(index).item(5) * ViewState("Detalle").Rows(hdnDetalleIndex.Value).item(4))

            For Each rowen As GridViewRow In grvCotizaciones.Rows
                rowen.BackColor = Drawing.Color.White
            Next
            grvCotizaciones.Rows(index).BackColor = Drawing.Color.LightBlue


            Dim dtselected As DataTable
            dtselected = ViewState("Selected")
            grvSelected.DataSource = dtselected
            grvSelected.DataBind()
            grvSelected.Rows(0).BackColor = Drawing.Color.LightBlue
        ElseIf (e.CommandName = "Editar") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim rowen = ViewState("Cotizaciones" & hdnCotizacionIndex.Value.ToString()).Rows(index)
            Dim strMensaje As String = ""
            Dim clsFunciones As New clsFunciones
            'Se actualiza el estatus del detalle de la requisicion
            strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Actualiza_Estatus_Cotizacion",
             rowen.item(6) & "," & rowen.item(7) & "," & rowen.item(0) & ",4," & Session("IdUsuario"),
               "@intRequisicion,@intDetRequisicion,@intIdCotizacion,@intEstatus,@intIdUsuarioCaptura")

            If strMensaje = "OK" Then
                btnGuardar.Enabled = False
                Alert("Se ha solicitado al proveedor actualizar la cotización, se refrescará la pagina para recalcular los valores.", Me, 1, 3)
                Timer1.Enabled = True
            Else
                Alert("Error al guardar cambios " & strMensaje, Me, 1, 4)
                Exit Sub
            End If
        End If
    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Response.Redirect("wfrmCuadroFrio.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim strMensaje As String
        Dim clsFunciones As New clsFunciones
        Dim dtsCotiza As New DataSet
        Dim dtTmp As New DataTable()



        Try
            Dim permitir As Boolean = False
            'Determinar si se puede avanzar
            For indice As Integer = 0 To ViewState("Detalle").Rows.count - 1
                If ViewState("Cotizaciones" & indice).Rows.count > 0 Then
                    permitir = True
                Else
                    permitir = False
                End If
            Next
            'Si hay cotizaciones seleccionadas para todos los elementos de la requisicion
            If permitir Then

                Dim index As Integer = 0
                'Recorre el detalle para actualizar el estatus
                For Each rowen In ViewState("Detalle").Rows
                    'Busca el precio seleccionado
                    For Each priceRow In ViewState("Cotizaciones" & index).Rows
                        If priceRow.item(10) = 1 Then
                            hdnPrecio.Value = priceRow.item(5)
                            hdnProveedor.Value = priceRow.item(3)
                        End If
                    Next

                    hdnIdRequisicionSel.Value = rowen.item(0)
                    hdnDetallerequisicion.Value = rowen.item(1)
                    'Usuario de prueba
                    Session("IdUsuario") = 4
                    Dim precioInsert As Decimal = 0
                    precioInsert = hdnPrecio.Value
                    'Se actualiza el estatus del detalle de la requisicion
                    strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Actualiza_Estatus_RequisicionCompra",
                     hdnIdRequisicionSel.Value & "," & hdnDetallerequisicion.Value & "," & If(cbComite.Checked, 4, 5).ToString() & "," & Session("IdUsuario") & ",Normal," & precioInsert.ToString("###,###,##0.00") & "," & hdnProveedor.Value,
                       "@intRequisicion,@intDetRequisicion,@intEstatus,@intIdUsuarioCaptura,@vchComentarios,@decPrecio,@intIdProveedor")

                    If strMensaje = "OK" Then

                    Else
                        Alert("Error al guardar cambios " & strMensaje, Me, 1, 4)
                        Exit Sub
                    End If
                    index += 1
                Next
                Alert("Se han guardado los cambios correctamente ", Me, 1, 5)
                Timer1.Enabled = True
            Else
                Alert("Debe seleccionarse un precio para cada elemento de la requisicion.", Me, 1, 3)
            End If
        Catch ex As Exception
            Alert("Error al guardar cambios: " & ex.Message & ": ", Me, 1, 4)
        End Try
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("wfrmCuadroFrio.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
    End Sub


    Protected Sub OnRowEditing(sender As Object, e As GridViewEditEventArgs) Handles grvCotizaciones.RowEditing
        grvCotizaciones.EditIndex = e.NewEditIndex
        Dim dtCotizaciones As DataTable
        If Not ViewState("Cotizaciones" & hdnCotizacionIndex.Value.ToString()) Is Nothing Then
            dtCotizaciones = ViewState("Cotizaciones" & hdnCotizacionIndex.Value.ToString())
            grvCotizaciones.DataSource = dtCotizaciones
            grvCotizaciones.DataBind()
        End If


    End Sub

    Protected Sub OnUpdate(sender As Object, e As EventArgs)
        Dim row As GridViewRow = TryCast(TryCast(sender, LinkButton).NamingContainer, GridViewRow)
        Dim price As String = TryCast(row.Cells(5).Controls(0), TextBox).Text
        If Not IsNumeric(price) Then Exit Sub
        Dim dt As DataTable = TryCast(ViewState("Cotizaciones" & hdnCotizacionIndex.Value.ToString()), DataTable)
        dt.Rows(row.RowIndex)("Precio") = price

        ViewState("Cotizaciones" & hdnCotizacionIndex.Value.ToString()) = dt
        grvCotizaciones.EditIndex = -1
        Dim dtCotizaciones As DataTable
        If Not ViewState("Cotizaciones" & hdnCotizacionIndex.Value.ToString()) Is Nothing Then
            dtCotizaciones = ViewState("Cotizaciones" & hdnCotizacionIndex.Value.ToString())
            grvCotizaciones.DataSource = dtCotizaciones
            grvCotizaciones.DataBind()
        End If
    End Sub

    Protected Sub OnCancel(sender As Object, e As EventArgs)
        grvCotizaciones.EditIndex = -1
        Dim dtCotizaciones As DataTable
        If Not ViewState("Cotizaciones" & hdnCotizacionIndex.Value.ToString()) Is Nothing Then
            dtCotizaciones = ViewState("Cotizaciones" & hdnCotizacionIndex.Value.ToString())
            grvCotizaciones.DataSource = dtCotizaciones
            grvCotizaciones.DataBind()
        End If
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
