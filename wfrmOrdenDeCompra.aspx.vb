Imports System.Data
Imports System.Net.Mail.MailMessage

Partial Class wfrmOrdenDeCompra
    Inherits System.Web.UI.Page

#Region "Eventos"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("caller") = "wfrmOrdenDeCompra.aspx"
        If Not Page.IsPostBack Then

            Dim clsFunciones As New clsFunciones
            'Creacion del encabezado de la tabla de articulos y se asigna al ViewState
            Dim dtArticulos As New DataTable()
            dtArticulos.Columns.AddRange(New DataColumn(5) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("totalart")})
            ViewState("DetOrden") = dtArticulos
            grvArticulos.DataSource = dtArticulos
            grvArticulos.DataBind()
            If Request.QueryString("idOrden") <> "" Then 'Si se recibe una orden para modificar

            ElseIf Request.QueryString("idrequisicion") <> "" Then 'Si viene de una requisicion de compra
                Dim test As String = ""
            Else
                txtNombre.Text = Session("nombre")
                txtnoEmpleado.Text = Session("intEmpleado")
                txtProveedor.Text = Request.QueryString("noProveedor")
                txtProveedorNombre.Text = Request.QueryString("nombre")
                txtFecha.Text = Now
                txtTotal.Text = "0.00"
            End If
        End If
    End Sub


    Protected Sub btnBuscarProducto_Click(sender As Object, e As EventArgs) Handles btnBuscarProducto.Click
        If txtCodigo.Text <> "" Or txtDescripcion.Text <> "" Then 'Si se capturó codigo o nombre del articulo
            Dim strMensaje As String = ""
            Dim strQuery As String = ""
            Dim dtsArticulos As New DataSet
            Dim clsFunciones As New clsFunciones

            'Se busca el producto en base al codigo o concepto capturado
            strMensaje = clsFunciones.Llena_Dataset("FILTROS", dtsArticulos, "Productos", "Cargar_Productos", txtCodigo.Text & "," & txtDescripcion.Text & ",", _
                                                    "intCodigo,vchProducto,vchordencompra", dtsArticulos.Tables("Productos"))

            If strMensaje = "OK" Then 'Si se realizo la consulta sin error
                If dtsArticulos.Tables("Productos").Rows.Count > 0 Then 'Si existe el folio
                    'La existencia y el tipo de salida se almacenan en campos ocultos
                    If dtsArticulos.Tables("Productos").Rows.Count = 1 Then 'Si es un solo resultado de la busqueda
                        txtCodigo.Text = dtsArticulos.Tables("Productos").Rows(0).Item(0)
                        txtDescripcion.Text = dtsArticulos.Tables("Productos").Rows(0).Item(1)
                        txtPrecio.Text = CDec(dtsArticulos.Tables("Productos").Rows(0).Item(8)).ToString("###,###,##0.00")
                        pnlArticulos.Visible = False

                 
                    Else
                        Dim dtArticulos As New DataTable()
                        Dim intContadorArticulos As Integer
                        'Agregamos la cabecera del gridview
                        dtArticulos.Columns.AddRange(New DataColumn(3) {New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("udm"), New DataColumn("precio")})
                        'Recorremos el dataset para llenar el gridView
                        For intContadorArticulos = 0 To dtsArticulos.Tables("Productos").Rows.Count - 1
                            'Se agrega el articulo al gridView
                            dtArticulos.Rows.Add(dtsArticulos.Tables("Productos").Rows(intContadorArticulos).Item(0), _
                                                 dtsArticulos.Tables("Productos").Rows(intContadorArticulos).Item(1), _
                                                 dtsArticulos.Tables("Productos").Rows(intContadorArticulos).Item(2), _
                                                 dtsArticulos.Tables("Productos").Rows(intContadorArticulos).Item(8))
                        Next
                        'Mandamos el resultado de la busqueda al ViewState y llenamos el gridView
                        ViewState("BuscarArticulos") = dtArticulos
                        grvArticulos.DataSource = dtArticulos
                        grvArticulos.DataBind()
                        'Se pone visible el panel del grid con los resultados de la busqueda
                        pnlArticulos.Visible = True

                    End If
                Else
                    Alert("Codigo o producto no existe")
                End If
            Else
                Alert("No se pudo buscar el codigo " & txtCodigo.Text.Replace("'", "") & ": " & strMensaje.Replace("'", "") & "")
            End If
        End If
    End Sub

    Protected Sub grvArticulos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvArticulos.RowCommand
        If (e.CommandName = "Seleccionar") Then
            ' Obtenemos la fila seleccionada
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            'Si se tiene en existencia  -- O que sea de la familia de papeleria (añadido el 04/08/2015 a peticion de almacen)
            txtDescripcion.Text = ViewState("BuscarArticulos").Rows(index).Item(1)
            txtCodigo.Text = ViewState("BuscarArticulos").Rows(index).Item(0)
            txtPrecio.Text = CDec(ViewState("BuscarArticulos").Rows(index).Item(3)).ToString("###,###,##0.00")
            pnlArticulos.Visible = False

            txtCodigo.ReadOnly = True
            txtDescripcion.ReadOnly = True
        End If
    End Sub

    Protected Sub btnAgregarProducto_Click(sender As Object, e As EventArgs) Handles btnAgregarProducto.Click
        Try
            If txtCodigo.Text <> "" And txtDescripcion.Text <> "" Then ' Si se capturo codigo del articulo
                If txtCantidad.Text <> "" Then ' Si se capturo cantidad


                    'Preguntar: validar por base datos o solo por la orden actual
                    Dim strMensaje As String = "OK"
                    'Dim dtsIntentos As New DataSet
                    Dim clsFunciones As New clsFunciones
                    Dim total As Decimal = CDec(txtTotal.Text)


                    Dim dt As New DataTable()
                    If Not ViewState("DetOrden") Is Nothing Then
                        dt = ViewState("DetOrden")
                    End If
                    Dim i As Integer
                    For i = 0 To grvDetOrden.Rows.Count - 1
                        If txtCodigo.Text = grvDetOrden.Rows(i).Cells(0).Text Then
                            Alert("El artículo " & txtDescripcion.Text & " ya fue agregado previamente")
                            Exit Sub
                        End If
                    Next

                    'Se agrega el articulo al gridView
                    dt.Rows.Add(grvDetOrden.Rows.Count + 1, txtCodigo.Text, txtDescripcion.Text & " - " & txtAdicional.Text, txtCantidad.Text, txtPrecio.Text, CDec(txtCantidad.Text) * CDec(txtPrecio.Text))
                    ViewState("DetOrden") = dt
                    grvDetOrden.DataSource = dt
                    grvDetOrden.DataBind()
                    total += (CDec(txtCantidad.Text) * CDec(txtPrecio.Text))
                    txtTotal.Text = total.ToString("###,###,##0.00")
                    txtPrecio.Text = ""
                    txtCantidad.Text = ""
                    txtCodigo.Text = ""
                    txtDescripcion.Text = ""
                    txtAdicional.Text = ""
                    txtCodigo.ReadOnly = False
                    txtDescripcion.ReadOnly = False

                Else
                    Alert("Favor de anotar la cantidad")
                End If
            Else
                'txtDestino.Text = ""
                Alert("Favor de seleccionar el producto")
            End If

        Catch ex As Exception
            Alert("Error al tratar de agregar articulo: " & ex.Message)
        End Try
    End Sub


    Protected Sub grvDetOrden_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvDetOrden.RowCommand
        If (e.CommandName = "Eliminar") Then
            ' Obtenemos la fila seleccionada
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            'Cargamos los articulos a un datatable para eliminarlo
            Dim dt As New DataTable()
            If Not ViewState("DetOrden") Is Nothing Then
                dt = ViewState("DetOrden")
            End If
            dt.Rows(index).Delete()
            ViewState("DetOrden") = dt
            grvDetOrden.DataSource = dt
            grvDetOrden.DataBind()
        End If
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Session.Contents.Count <= 0 Then
            Alert("Se requiere entre de nuevo al sistema por superar 20 minutos sin usarlo")

            Exit Sub
        End If
        Try
            If ViewState("DetOrden").Rows.Count > 0 Then 'Si existen articulos para la requisicion
                Dim intContador As Integer
                Dim strMensaje, strModificacion As String
                strModificacion = ""
                Dim strMensajeModificar As String = "Se creo la "
                Dim clsFunciones As New clsFunciones
                Dim dtsOrden As New DataSet

                Dim dtTmp As New DataTable()

              
                'Creamos un rango de columnas para la datatable temporal que almacenara los articulos
                dtTmp.Columns.AddRange(New DataColumn(7) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("impuesto"), New DataColumn("totalart"), New DataColumn("recibido")})

                For intContador = 0 To ViewState("DetOrden").Rows.Count - 1 'Recorremos el ViewState para obtener los articulos
                    dtTmp.Rows.Add(CInt(ViewState("DetOrden").Rows(intContador).Item(0)), _
                                   CInt(ViewState("DetOrden").Rows(intContador).Item(1)), _
                                   ViewState("DetOrden").Rows(intContador).Item(2), _
                                   CDec(ViewState("DetOrden").Rows(intContador).Item(3)), _
                                   CDec(ViewState("DetOrden").Rows(intContador).Item(4)), _
                                   0, _
                                   CDec(ViewState("DetOrden").Rows(intContador).Item(5)), _
                                   0)

                Next
                btnGuardar.Enabled = False
                'Mandamos guardar la solicitud con sus articulos
                strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsOrden, "Orden", "Inserta_Orden", _
                             hdnOrden.Value & "," & Session("IdUsuario") & "," & txtProveedor.Text.Replace(",", "") & ",A," & _
                             "E," & txtObservaciones.Text.ToUpper.Replace(",", "*") & "," & txtTotal.Text, _
                             "@intIdOrden,@intSolicitante,@intProveedor,@chEstatus,@chSerie,@vchObservaciones,@decTotal", _
                             dtTmp)
                If strMensaje = "OK" Then ' Si la solicitud se guardó correctamente mostramos el numero de requisicion
                    Alert(strMensajeModificar & "orden de compra: E" & dtsOrden.Tables("Orden").Rows(0).Item(0) & "")
                    ClearFields()
                    'If Request.QueryString("intidestatus") = "" Or Request.QueryString("intidestatus") = "2" Then 'Solo se envia mensaje en nuevas o modificaciones del jefe de departamento
                    '    Dim dtsDepartamentos As New DataSet
                    '    strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsDepartamentos, "Telefono", "Cargar_Departamentos", _
                    '                ddlDepartamento.SelectedValue, "@intDepartamento", dtsDepartamentos.Tables("Telefono"))
                    '    If strMensaje = "OK" Then 'Si se realizo la consulta del telefono
                    '        Dim LlaveCifrada As New clsCifrado("Jmas")
                    '        Dim OrdenCifrada As String = LlaveCifrada.EncryptData(dtsRequisicion.Tables("Requisicion").Rows(0).Item(0))
                    '        If dtsDepartamentos.Tables("Telefono").Rows(1).Item(2) <> "" Then
                    '            strMensaje = clsFunciones.EjecutaProcedimiento("SMS", "Agrega_BitacoraMensajes_Usuario", _
                    '                        "5,-" & dtsDepartamentos.Tables("Telefono").Rows(1).Item(2) & "-,,," & _
                    '                        "Avisos " & strMensajeModificar & "salida de almacen R" & dtsRequisicion.Tables("Requisicion").Rows(0).Item(0) & " pedida para " & txtSolictado.Text.Replace(",", "") & " esperando aprobacion en http://201.147.15.182/,0", _
                    '                        "@intidOrigen,@vchTelefono,@vchFecha,@vchHora,@vchComando,@intidUsuario")
                    '        End If
                    '        If dtsDepartamentos.Tables("Telefono").Rows(1).Item(3) <> "" Then
                    '            EnviarCorreo(strMensajeModificar & "orden de salida de almacen R" & dtsRequisicion.Tables("Requisicion").Rows(0).Item(0) & " pedida para entrega al empleado  " & txtSolictado.Text.Replace(",", "") & " esperando su aprobacion en http://201.147.15.182/", dtsDepartamentos.Tables("Telefono").Rows(1).Item(3))
                    '        End If
                    '        'If dtsDepartamentos.Tables("Telefono").Rows(1).Item(2) <> "" Then
                    '        '    strMensaje = clsFunciones.EjecutaProcedimiento("SMS", "Agrega_BitacoraMensajes_Usuario", _
                    '        '                "5,-" & dtsDepartamentos.Tables("Telefono").Rows(1).Item(2) & "-,,," & _
                    '        '                "Avisos " & strMensajeModificar & "salida de almacen R" & dtsRequisicion.Tables("Requisicion").Rows(0).Item(0) & " esperando aprobacion en http://201.147.15.182/almacen/wfrmAprobarExpress.aspx?id=" & OrdenCifrada & ",0", _
                    '        '                "@intidOrigen,@vchTelefono,@vchFecha,@vchHora,@vchComando,@intidUsuario")
                    '        'End If
                    '        'If dtsDepartamentos.Tables("Telefono").Rows(1).Item(3) <> "" Then
                    '        '    EnviarCorreo(strMensajeModificar & "orden de salida de almacen R" & dtsRequisicion.Tables("Requisicion").Rows(0).Item(0) & " esperando su aprobacion en http://201.147.15.182/almacen/wfrmAprobarExpress.aspx?id=" & OrdenCifrada & "", dtsDepartamentos.Tables("Telefono").Rows(1).Item(3))
                    '        'End If
                    '    End If
                    'End If

                    'Timer1.Enabled = True
                Else
                    Alert("Error al guardar orden: " & strMensaje & "")
                End If
            Else
                Alert("Favor de agregar algún artículo a la orden")
            End If

        Catch ex As Exception
            Alert("Error al tratar de guardar orden: " & ex.Message)
        End Try

    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

    End Sub

    Protected Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        txtCodigo.Text = ""
        txtDescripcion.Text = ""
        txtAdicional.Text = ""
        txtPrecio.Text = ""
        txtCantidad.Text = ""
        pnlArticulos.Visible = False
    End Sub

#End Region

#Region "Procedimientos"

    Private Sub Alert(ByVal strMensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('" & strMensaje & "');", True)
    End Sub

    Public Sub ClearFields()
        txtFolio.Text = ""
        txtFecha.Text = Now
        txtCodigo.Text = ""
        txtProveedor.Text = ""
        txtProveedorNombre.Text = ""
        txtTotal.Text = "0.00"
        txtDescripcion.Text = ""
        txtAdicional.Text = ""
        txtPrecio.Text = ""
        txtCantidad.Text = ""

        pnlArticulos.Visible = False
        txtObservaciones.Text = ""

        Dim dtArticulos As New DataTable()
        dtArticulos.Columns.AddRange(New DataColumn(5) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("totalart")})
        ViewState("DetOrden") = dtArticulos
        grvArticulos.DataSource = dtArticulos
        grvArticulos.DataBind()
        grvDetOrden.DataSource = dtArticulos
        grvDetOrden.DataBind()
        btnGuardar.Enabled = True
    End Sub

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
            _Message.Subject = "Orden de Compra JMAS Chihuahua"
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


#End Region

#Region "Funciones"
    Public Function CheckValidation(ByRef mensaje As String) As Boolean
        Dim OK As Boolean = True

        If txtProveedor.Text = "" Then
            OK = False
            mensaje &= "Debe especificar un proveedor."
        End If

        Return OK
    End Function


#End Region




End Class
