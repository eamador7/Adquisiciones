Imports System.Data
Imports System.Net.Mail.MailMessage

Partial Class wfrmContrato
    Inherits System.Web.UI.Page

#Region "Eventos"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim clsFunciones As New clsFunciones
            
            
            'Creacion del encabezado de la tabla de articulos y se asigna al ViewState
            Dim dtArticulos As New DataTable()
            dtArticulos.Columns.AddRange(New DataColumn(2) {New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("udm")})
            'dtPartidas.Columns.AddRange(New DataColumn(5) {New DataColumn("partida"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidadmin"), New DataColumn("cantidadmax"), New DataColumn("udm")})
            ViewState("Articulos") = dtArticulos
            grvArticulos.DataSource = dtArticulos
            grvArticulos.DataBind()

            
        End If
    End Sub

    'Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
    '    'Mandamos a la pagina de busqueda de empleados
    '    Response.Redirect("wfrmBuscaEmpleado.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
    'End Sub

    

    Protected Sub btnBuscarProducto_Click(sender As Object, e As EventArgs) Handles btnBuscarProducto.Click
        If txtCodigo.Text <> "" Or txtConcepto.Text <> "" Then 'Si se capturó codigo o nombre del articulo
            Dim strMensaje As String = ""
            Dim strQuery As String = ""
            Dim dtsProductos As New DataSet
            Dim clsFunciones As New clsFunciones

            'Se busca el producto en base al codigo o concepto capturado
            strMensaje = clsFunciones.Llena_Dataset("FILTROS", dtsProductos, "Productos", "Cargar_Productos", txtCodigo.Text & "," & txtConcepto.Text & ",", _
                                                    "intCodigo,vchProducto,vchordencompra", dtsProductos.Tables("Productos"))

            If strMensaje = "OK" Then 'Si se realizo la consulta sin error
                If dtsProductos.Tables("Productos").Rows.Count > 0 Then 'Si existe el folio
                    'La existencia y el tipo de salida se almacenan en campos ocultos
                    If dtsProductos.Tables("Productos").Rows.Count = 1 Then 'Si es un solo resultado de la busqueda
                        txtCodigo.Text = dtsProductos.Tables("Productos").Rows(0).Item(0)
                        txtConcepto.Text = dtsProductos.Tables("Productos").Rows(0).Item(1)
                        pnlArticulos.Visible = False
                    Else
                        Dim dtArticulos As New DataTable()
                        Dim intContadorArticulos As Integer
                        'Agregamos la cabecera del gridview
                        dtArticulos.Columns.AddRange(New DataColumn(2) {New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("udm")})
                        'Recorremos el dataset para llenar el gridView
                        For intContadorArticulos = 0 To dtsProductos.Tables("Productos").Rows.Count - 1
                            'Se agrega el articulo al gridView
                            dtArticulos.Rows.Add(dtsProductos.Tables("Productos").Rows(intContadorArticulos).Item(0), _
                                                 dtsProductos.Tables("Productos").Rows(intContadorArticulos).Item(1), _
                                                 dtsProductos.Tables("Productos").Rows(intContadorArticulos).Item(2))
                        Next
                        'Mandamos el resultado de la busqueda al ViewState y llenamos el gridView
                        ViewState("BuscarArticulos") = dtArticulos
                        grvArticulos.DataSource = dtArticulos
                        grvArticulos.DataBind()
                        'Se pone visible el panel del grid con los resultados de la busqueda
                        pnlArticulos.Visible = True
                        'Validamos si se tienen en existencia y los marcamos de gris
                        'Exceptuando de la familia de papeleria (añadido el 04/08/2015 a peticion de almacen)
                        Dim intContador As Integer
                        For intContador = 0 To dtArticulos.Rows.Count - 1
                            If CDbl(dtArticulos.Rows(intContador).Item(3).ToString) <= 0 And dtArticulos.Rows(intContador).Item(4).ToString <> "0291" Then
                                grvArticulos.Rows(intContador).BackColor = Drawing.Color.LightSlateGray
                            End If
                        Next

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
            If CDbl(ViewState("BuscarArticulos").Rows(index).Item(3).ToString) > 0 Or ViewState("BuscarArticulos").Rows(index).Item(4).ToString = "0291" Then
                'Cargamos el articulo a los textbox y hidden del Viewstate

                txtConcepto.Text = ViewState("BuscarArticulos").Rows(index).Item(1)
                txtCodigo.Text = ViewState("BuscarArticulos").Rows(index).Item(0)
                pnlArticulos.Visible = False
            Else
                Alert("No existen suficientes existencias de " & ViewState("BuscarArticulos").Rows(index).Item(1) & " para hacer salida")
            End If

        End If
    End Sub

    Protected Sub btnAgregarProducto_Click(sender As Object, e As EventArgs) Handles btnAgregarProducto.Click
        Try

        Catch ex As Exception
            Alert("Error al tratar de agregar articulo: " & ex.Message)
        End Try
    End Sub


    Protected Sub grvPartidas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvPartidas.RowCommand
        If (e.CommandName = "Eliminar") Then
            ' Obtenemos la fila seleccionada
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            'Cargamos los articulos a un datatable para eliminarlo
            Dim dt As New DataTable()
            If Not ViewState("Partidas") Is Nothing Then
                dt = ViewState("Partidas")
            End If
            dt.Rows(index).Delete()
            ViewState("Partidas") = dt
            grvPartidas.DataSource = dt
            grvPartidas.DataBind()
        End If
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Session.Contents.Count <= 0 Then
            Alert("Se requiere entre de nuevo al sistema por superar 20 minutos sin usarlo")

            Exit Sub
        End If
        Try

        Catch ex As Exception
            Alert("Error al tratar de guardar contrato: " & ex.Message)
        End Try

    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        
    End Sub

    Protected Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        txtCodigo.Text = ""

        txtConcepto.Text = ""

        pnlArticulos.Visible = False
    End Sub

#End Region

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

#Region "Funciones"

#End Region




End Class
