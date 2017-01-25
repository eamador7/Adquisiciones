Imports System.Data
Partial Class wfrmSolicitarCotizacion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Session("caller") = "wfrmSolicitarCotizacion.aspx"
        If Not Page.IsPostBack Then

            Dim clsFunciones As New clsFunciones
            Dim dtsEstatus As New DataSet
            Dim strMensaje As String

            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(13) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus")})




            Dim dtsRequisiciones As New DataSet
            strMensaje = clsFunciones.Llena_Grid(grvRequisiciones, "MIGRACION", dtsRequisiciones, "Requisiciones", "Cargar_DetallesRequisicionCompra",
                        ", , ,4,",
                        "@intIdRequisicion,@vchFechaDesde,@vchFechaHasta,@vchIdEstatus,@intIdSolicitante")
            If strMensaje = "OK" Then

                ViewState("Requisiciones") = dtsRequisiciones.Tables("Requisiciones")
                grvRequisiciones.DataSource = dtsRequisiciones.Tables("Requisiciones")
                grvRequisiciones.DataBind()
                ViewState("Detalle") = Nothing
                grvAprobadas.DataSource = Nothing
                grvAprobadas.DataBind()

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

                            dtResultado.Rows.Add(dtsProveedores.Tables("Proveedores").Rows(Contador).Item(0),
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(1),
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(2),
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(3),
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(4),
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(5)
                                                 )

                        Next
                        ' se agrega la tabla al viewstate y al datagrid
                        ViewState("BuscaProv") = dtResultado
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

    Protected Sub grvProveedor_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvProveedor.RowCommand

        If (e.CommandName = "AgregarProveedor") Then
            Dim dtProveedores As New DataTable()
            dtProveedores.Columns.AddRange(New DataColumn(6) {New DataColumn("noProveedor"), New DataColumn("nombre"), New DataColumn("domicilio"), New DataColumn("ciudad"), New DataColumn("rfc"), New DataColumn("nomfis"), New DataColumn("seleccionado")})
            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)


            Dim dtBuscaProv As New DataTable()
            If Not ViewState("BuscaProv") Is Nothing Then
                dtBuscaProv = ViewState("BuscaProv")
            End If

            If Not ViewState("Proveedores") Is Nothing Then
                dtProveedores = ViewState("Proveedores")
            End If

            dtProveedores.Rows.Add(dtBuscaProv.Rows.Item(intIndex).ItemArray)
            ViewState("Proveedores") = dtProveedores
            grvProveedores.DataSource = dtProveedores
            grvProveedores.DataBind()

            dtBuscaProv.Rows.RemoveAt(CInt(e.CommandArgument))
            ViewState("BuscaProv") = dtBuscaProv
            grvProveedor.DataSource = dtBuscaProv
            grvProveedor.DataBind()

            'txtNumProveedor.Text = ViewState("Proveedores").Rows(intIndex).Item(0).ToString
            'txtNombreBuscar.Text = ViewState("Proveedores").Rows(intIndex).Item(1).ToString
            'hdnNumeroProveedor.Value = ViewState("Proveedores").Rows(intIndex).Item(0)
            'hdnNombreProveedor.Value = ViewState("Proveedores").Rows(intIndex).Item(1).ToString
            'Response.Redirect(Session("caller") & "?key=" & Request.QueryString("key").Replace("+", "%2B") & _
            '   "&noProveedor=" & ViewState("Proveedores").Rows(intIndex).Item(0) & _
            '   "&nombre=" & ViewState("Proveedores").Rows(intIndex).Item(1) & _
            '   "&domicilio=" & ViewState("Proveedores").Rows(intIndex).Item(2) & _
            '   "&ciudad=" & ViewState("Proveedores").Rows(intIndex).Item(3) & _
            '   "&rfc=" & ViewState("Proveedores").Rows(intIndex).Item(4) & _
            '    "&nomfis=" & ViewState("Proveedores").Rows(intIndex).Item(5))

            'pnlProveedor.Visible = True

        End If

    End Sub

    Protected Sub grvRequisiciones_rowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvRequisiciones.RowCommand
        If (e.CommandName = "Aprobar") Then
            Dim dtAprobadas As New DataTable()
            dtAprobadas.Columns.AddRange(New DataColumn(15) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus"), New DataColumn("fechaSolicitud"), New DataColumn("nombreProveedor")})


            Dim dtRequis As New DataTable()
            If Not ViewState("Requisiciones") Is Nothing Then
                dtRequis = ViewState("Requisiciones")
            End If

            If Not ViewState("Detalle") Is Nothing Then
                dtAprobadas = ViewState("Detalle")
            End If

            dtAprobadas.Rows.Add(dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray)
            ViewState("Detalle") = dtAprobadas
            grvAprobadas.DataSource = dtAprobadas
            grvAprobadas.DataBind()

            dtRequis.Rows.RemoveAt(CInt(e.CommandArgument))
            ViewState("Requisiciones") = dtRequis
            grvRequisiciones.DataSource = dtRequis
            grvRequisiciones.DataBind()

       
        End If

    End Sub

    Protected Sub grvAprobadas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvAprobadas.RowCommand
        If (e.CommandName = "Eliminar") Then
            'Dim dtRequis As New DataTable()
            'dtRequis.Columns.AddRange(New DataColumn(16) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus"), New DataColumn("fechaSolicitud"), New DataColumn("nombreProveedor"), New DataColumn("dummy")})


            Dim dtAprobadas As New DataTable()
            If Not ViewState("Detalle") Is Nothing Then
                dtAprobadas = ViewState("Detalle")
            End If

            'If Not ViewState("Requisiciones") Is Nothing Then
            '    dtRequis = ViewState("Requisiciones")
            'End If

            'Dim rowww As Object = dtAprobadas.Rows.Item(CInt(e.CommandArgument)).ItemArray
            'dtRequis.Rows.Add(rowww)
            'ViewState("Requisiciones") = dtRequis
            'grvRequisiciones.DataSource = dtRequis
            'grvRequisiciones.DataBind()

            dtAprobadas.Rows.RemoveAt(CInt(e.CommandArgument))
            ViewState("Detalle") = dtAprobadas
            grvAprobadas.DataSource = dtAprobadas
            grvAprobadas.DataBind()

        End If
    End Sub

    Protected Sub grvProveedores_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvProveedores.RowCommand
        If (e.CommandName = "Eliminar") Then


            Dim dtProveedores As New DataTable()
            If Not ViewState("Proveedores") Is Nothing Then
                dtProveedores = ViewState("Proveedores")
            End If


            dtProveedores.Rows.RemoveAt(CInt(e.CommandArgument))
            ViewState("Proveedores") = dtProveedores
            grvProveedores.DataSource = dtProveedores
            grvProveedores.DataBind()

        End If
    End Sub




    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Response.Redirect("wfrmSolicitarCotizacion.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click

        Dim clsFunciones As New clsFunciones
        Dim dtsCotiza As New DataSet
        Dim strMensaje As String
        Dim okCount As String = ""

        Try
            If txtDias.Text <> "" Then

                If grvProveedores.Rows.Count > 0 Then
                    If grvAprobadas.Rows.Count > 0 Then

                        Dim dtAprobadas As New DataTable()
                        dtAprobadas.Columns.AddRange(New DataColumn(6) {New DataColumn("intIdCotizacion"), New DataColumn("intIdRequi"), New DataColumn("intIdDetRequi"), New DataColumn("vchMarca"), New DataColumn("decPrecio"), New DataColumn("decIva"), New DataColumn("intTiempoEntrega")})

                        For intContador = 0 To ViewState("Detalle").Rows.Count - 1 'Recorremos el ViewState para obtener los articulos
                            dtAprobadas.Rows.Add(-1,
                                                 CInt(ViewState("Detalle").Rows(intContador).Item(0)),
                                                 CInt(ViewState("Detalle").Rows(intContador).Item(1)),
                                                 "",
                                                 0,
                                                 0,
                                                 0)
                            ' cco_id ---> codigo de departamento item5
                        Next



                        Dim dtProveedores As New DataTable()
                        If Not ViewState("Proveedores") Is Nothing Then
                            dtProveedores = ViewState("Proveedores")
                        End If

                        For Each rowen As DataRow In dtProveedores.Rows
                            strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsCotiza, "Cotizacion", "Inserta_Cotizacion",
                                   Session("IdUsuario").ToString() & "," & rowen.Item(0).ToString() & "," & txtDias.Text.Trim(),
                                   "@intSolicitante,@intProveedor,@intDiasEntrega",
                                   dtAprobadas)

                            If strMensaje <> "OK" Then


                                Alert("Error al guardar solicitudes")
                            Else
                                okCount &= If(okCount = "", dtsCotiza.Tables("Cotizacion").Rows(0).Item(0).ToString, " - " & dtsCotiza.Tables("Cotizacion").Rows(0).Item(0).ToString)
                            End If


                        Next

                        If okCount <> "" Then
                            Alert("Se han generado las siguientes solicitudes correctamente: " & okCount & "")
                        Else
                            Alert("Error al guardar solicitudes")
                        End If

                        'Session("Cotizacion") = dtAprobadas

                        'ifrSolicitud.Src = "wfrmReporteSolicitudCotizacion.aspx?key=" & Request.QueryString("key").Replace("+", "%2B")
                        'Alert("Espere mientras se genera la solicitud")

                        Timer1.Enabled = True
                    Else
                        Alert("Favor de seleccionar articulos para cotizar")
                    End If
                Else
                    Alert("Favor de seleccionar proveedores ")
                End If
            Else
                Alert("Debe especificar un tiempo de entrega ")
            End If

        Catch ex As Exception
            Alert("Error al guardar cambios: " & ex.Message & ": ")
        End Try
    End Sub

    Protected Sub btnSiguiente_Click(sender As Object, e As EventArgs) Handles btnSiguiente.Click


        Try
            If btnSiguiente.Text = "Siguiente" Then
                mainArtPanel.Visible = False
                mainProvPanel.Visible = True
                btnGuardar.Visible = True
                btnSiguiente.Text = "Anterior"
            Else
                mainArtPanel.Visible = True
                mainProvPanel.Visible = False
                btnGuardar.Visible = False
                btnSiguiente.Text = "Siguiente"
            End If

        Catch ex As Exception
            Alert("Error al guardar cambios: " & ex.Message & ": ")
        End Try
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("wfrmSolicitarCotizacion.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
    End Sub

    Protected Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        pnlProveedor.Visible = False
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
