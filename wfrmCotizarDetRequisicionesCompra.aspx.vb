Imports System.Data
Partial Class wfrmCotizarDetRequisicionesCompra
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Session("caller") = "wfrmCotizarDetRequisicionesCompra.aspx"
        If Not Page.IsPostBack Then
       
            Dim clsFunciones As New clsFunciones
            Dim dtsEstatus As New DataSet
            Dim strMensaje As String

            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(13) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("estatus"), New DataColumn("fecha"), New DataColumn("intidestatus"), New DataColumn("intdepartamento"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("ccoid"), New DataColumn("cconumero"), New DataColumn("seleccionar")})


            Dim dtsRequisiciones As New DataSet
            strMensaje = clsFunciones.Llena_Grid(grvRequisiciones, "MIGRACION", dtsRequisiciones, "Requisiciones", "Cargar_DetallesRequisicionCompra", _
                        ", , ,4,", _
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

    Protected Sub btnBuscarProveedor_Click(sender As Object, e As EventArgs) Handles btnBuscarProveedor.Click
        Try
            If txtNumProveedor.Text.Replace(" ", "") <> "" Or txtNombreBuscar.Text.Replace(" ", "") <> "" Then ' si se capturo el numero o nombre de proveedor

                If txtNombreBuscar.Text.Replace(" ", "") <> "" And txtNombreBuscar.Text.Length < 3 Then ' si se capturonombre y  menos de tres letras del proveedor
                    Alert("Favor de capturar almenos tres letras del proveedor ")
                End If

                Dim strMensaje As String
                Dim dtsProveedores As New DataSet
                Dim clsFuncion As New clsFunciones
                strMensaje = clsFuncion.Llena_Dataset("Filtros", dtsProveedores, "Proveedores", "Cargar_Proveedores", txtNumProveedor.Text & "," & txtNombreBuscar.Text.Replace(" ", "%"), "intProveedor,vchNombre", dtsProveedores.Tables("Proveedores"))
                If strMensaje = "OK" Then '' se realizo la busqueda  con exito
                    If dtsProveedores.Tables("Proveedores").Rows.Count > 0 Then ' si existen proveedores
                        If dtsProveedores.Tables("Proveedores").Rows.Count = 1 Then

                            txtNumProveedor.Text = dtsProveedores.Tables("Proveedores").Rows(0).Item(0).ToString
                            txtNombreBuscar.Text = dtsProveedores.Tables("Proveedores").Rows(0).Item(1).ToString
                            hdnNumeroProveedor.Value = dtsProveedores.Tables("Proveedores").Rows(0).Item(0)
                            hdnNombreProveedor.Value = dtsProveedores.Tables("Proveedores").Rows(0).Item(1)

                            pnlProveedor.Visible = False
                        End If

                        ' lleno el grid de proveedores
                        Dim dtResultado As New DataTable
                        Dim Contador As Integer
                        '' se agreagan las columnas con los datos del proveedor para recibir el dataset
                        dtResultado.Columns.AddRange(New DataColumn(6) {New DataColumn("noproveedor"), New DataColumn("nombre"), New DataColumn("domicilio"), New DataColumn("cidudad"), New DataColumn("rfc"), New DataColumn("nomfis"), New DataColumn("Seleccionar")})
                        ' se llena la tabla con los datos del dataset
                        For Contador = 0 To dtsProveedores.Tables("Proveedores").Rows.Count - 1

                            dtResultado.Rows.Add(dtsProveedores.Tables("Proveedores").Rows(Contador).Item(0), _
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(1), _
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(2), _
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(3), _
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(4), _
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(5)
                                                 )

                        Next
                        ' se agrega la tabla al viewstate y al datagrid
                        ViewState("Proveedores") = dtResultado
                        grvProveedor.DataSource = dtResultado
                        grvProveedor.DataBind()

                        pnlProveedor.Visible = True

                    Else
                        Alert("No se encontraron proveedores con este nombre o no. de proveedor")
                    End If


                Else
                    Alert("No se pudo buscar el proveedor " & txtNombreBuscar.Text.Replace("'", "") & ": " & strMensaje.Replace("'", "") & "")

                End If

            Else

                Alert("Favor de buscar con el nombre o numero de proveedor ")
            End If


        Catch ex As Exception
            Alert("Error: " & ex.Message.Replace("'", "") & "")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "document.getElementById('dialog').style.display='none';alert('');", True)
        End Try
    End Sub

    Protected Sub btnAgregar_click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If txtPrecio.Text <> "" Then
            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(7) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("numeroproveedor"), New DataColumn("nombreproveedor"), New DataColumn("precio"), New DataColumn("requisicion"), New DataColumn("detallerequisicion")})
            Dim clsFunciones As New clsFunciones
            Dim dtsProductos As New DataSet


            If Not ViewState("Detalle") Is Nothing Then
                dt = ViewState("Detalle")
            End If

            dt.Rows.Add(grvCotizaciones.Rows.Count + 1, hdnCodigo.Value, hdnConcepto.Value, hdnNumeroProveedor.Value,
                      hdnNombreProveedor.Value, txtPrecio.Text, hdnRequisicion.Value, hdnDetallerequisicion.Value)
            ViewState("Detalle") = dt
            grvCotizaciones.DataSource = dt
            grvCotizaciones.DataBind()

            hdnNumeroProveedor.Value = ""
            hdnNombreProveedor.Value = ""
            txtNumProveedor.Text = ""
            txtNombreBuscar.Text = ""
            txtPrecio.Text = ""
        Else
            Alert("Debe espeificar un precio ")
        End If
    End Sub

    Protected Sub grvCotizaciones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvCotizaciones.RowCommand
        If (e.CommandName = "Eliminar") Then
            ' Obtenemos la fila seleccionada
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            'Cargamos los articulos a un datatable para eliminarlo
            Dim dt As New DataTable()
            If Not ViewState("Detalle") Is Nothing Then
                dt = ViewState("Detalle")
            End If
            dt.Rows.RemoveAt(CInt(e.CommandArgument))
            '   dt.Rows(index).Delete()
            ViewState("Detalle") = dt
            grvCotizaciones.DataSource = dt
            grvCotizaciones.DataBind()
        End If
    End Sub

    Protected Sub grvProveedor_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvProveedor.RowCommand

        If (e.CommandName = "Seleccionar") Then

            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)

            txtNumProveedor.Text = ViewState("Proveedores").Rows(intIndex).Item(0).ToString
            txtNombreBuscar.Text = ViewState("Proveedores").Rows(intIndex).Item(1).ToString
            hdnNumeroProveedor.Value = ViewState("Proveedores").Rows(intIndex).Item(0)
            hdnNombreProveedor.Value = ViewState("Proveedores").Rows(intIndex).Item(1).ToString
            'Response.Redirect(Session("caller") & "?key=" & Request.QueryString("key").Replace("+", "%2B") & _
            '   "&noProveedor=" & ViewState("Proveedores").Rows(intIndex).Item(0) & _
            '   "&nombre=" & ViewState("Proveedores").Rows(intIndex).Item(1) & _
            '   "&domicilio=" & ViewState("Proveedores").Rows(intIndex).Item(2) & _
            '   "&ciudad=" & ViewState("Proveedores").Rows(intIndex).Item(3) & _
            '   "&rfc=" & ViewState("Proveedores").Rows(intIndex).Item(4) & _
            '    "&nomfis=" & ViewState("Proveedores").Rows(intIndex).Item(5))

            pnlProveedor.Visible = False

        End If

    End Sub

    Protected Sub grvRequisiciones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvRequisiciones.RowCommand
        If (e.CommandName = "Seleccionar") Then
            ' Retrieve the row index stored in the CommandArgument property.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            ViewState("Detalle") = Nothing

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
            grvRequisiciones.Rows(index).BackColor = Drawing.Color.LightGreen

            'Se cargan las cotizaciones ya guardadas
            Dim strMensaje As String = ""
            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(7) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("numeroproveedor"), New DataColumn("nombreproveedor"), New DataColumn("precio"), New DataColumn("requisicion"), New DataColumn("detallerequisicion")})
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

    Protected Sub btnGuardar_click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim strMensaje As String = ""
        Dim clsFunciones As New clsFunciones
        Dim dtsCotiza As New DataSet
        Dim dtTmp As New DataTable()
        Try
            If grvCotizaciones.Rows.Count > 0 Then


                Dim dt As New DataTable()
                If Not ViewState("Detalle") Is Nothing Then
                    dt = ViewState("Detalle")
                End If
                'dt.Columns.AddRange(New DataColumn(7) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("numeroproveedor"), New DataColumn("nombreproveedor"), New DataColumn("precio"), New DataColumn("requisicion"), New DataColumn("detallerequisicion")})

                'Se borran las cotizaciones para volver a agregar la lista
                strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Elimina_Cotizaciones", _
                hdnRequisicion.Value & "," & hdnDetallerequisicion.Value, _
                   "@intIdRequisicion,@intIdDetRequisicion")

                If strMensaje <> "OK" Then Exit Sub


                'Se recorren las cotizaciones para guardar cada una
                For Each rowen As DataRow In dt.Rows
                    'Mandamos guardar la solicitud con sus articulos
                    strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsCotiza, "Cot", "Inserta_detRequisicionCompraCotizacion", _
                                rowen.ItemArray(6).ToString & "," & rowen.ItemArray(7).ToString & "," & rowen.ItemArray(1).ToString & "," & rowen.ItemArray(5).ToString & "," & rowen.ItemArray(3).ToString, _
                                 "@intIdRequi,@intIdDetRequi,@intIdArticulo,@decPrecio,@intIdProveedor", _
                                 dtTmp)
                    If strMensaje = "OK" Then


                    Else
                        Alert("Error al guardar cotizacion: " & strMensaje)
                        Exit Sub
                    End If
                Next

                If cbFinalizar.Checked Then
                    'Se actualiza el estatus del detalle de la requisicion
                    strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Actualiza_Estatus_RequisicionCompra", _
                     hdnRequisicion.Value & "," & hdnDetallerequisicion.Value & ",5," & Session("IdUsuario") & ",Cotizado,0", _
                       "@intRequisicion,@intDetRequisicion,@intEstatus,@intIdUsuarioCaptura,@vchComentarios,@decPrecio")
                End If

                If strMensaje = "OK" Then
                    Alert("Se han guardado las cotizaciones para la requisicion" & hdnRequisicion.Value.ToString & "")

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
    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Se redirecciona para aprobar otra requisicion de almacen
        Response.Redirect("wfrmCotizarDetRequisicionesCompra.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("wfrmCotizarDetRequisicionesCompra.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
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
