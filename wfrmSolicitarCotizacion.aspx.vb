Imports System.Data
Imports clsFunciones
Partial Class wfrmSolicitarCotizacion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Usuario prueba
        Session("IdUsuario") = 4
        Session("caller") = "wfrmSolicitarCotizacion.aspx"
        If Not Page.IsPostBack Then

            Dim clsFunciones As New clsFunciones
            Dim dtsEstatus As New DataSet
            Dim strMensaje As String

            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(13) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus")})
            txtDias.Text = 3



            Dim dtsRequisiciones As New DataSet
            strMensaje = clsFunciones.Llena_Grid(grvRequisiciones, "MIGRACION", dtsRequisiciones, "Requisiciones", "Cargar_DetallesRequisicionCompra",
                        ", , ,2,",
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
                    Alert("Favor de capturar almenos tres letras del proveedor ", Me, 1, 3)
                End If

                Dim strMensaje As String
                Dim dtsProveedores As New DataSet
                Dim clsFuncion As New clsFunciones
                strMensaje = clsFuncion.Llena_Dataset("Migracion", dtsProveedores, "Proveedores", "Cargar_Proveedores", txtNumProveedor.Text & "," & txtNombreBuscar.Text.Replace(" ", "%"), "intProveedor,vchNombre", dtsProveedores.Tables("Proveedores"))
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
                        dtResultado.Columns.AddRange(New DataColumn(9) {New DataColumn("noproveedor"), New DataColumn("nombre"), New DataColumn("domicilio"), New DataColumn("cidudad"), New DataColumn("rfc"), New DataColumn("nomfis"), New DataColumn("mail"), New DataColumn("contra"), New DataColumn("telefono"), New DataColumn("Seleccionar")})
                        ' se llena la tabla con los datos del dataset
                        For Contador = 0 To dtsProveedores.Tables("Proveedores").Rows.Count - 1

                            dtResultado.Rows.Add(dtsProveedores.Tables("Proveedores").Rows(Contador).Item(0),
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(1),
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(2),
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(3),
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(4),
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(5),
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(6),
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(7)
                                                 )

                        Next
                        ' se agrega la tabla al viewstate y al datagrid
                        ViewState("BuscaProv") = dtResultado
                        grvProveedor.DataSource = dtResultado
                        grvProveedor.DataBind()

                        pnlProveedor.Visible = True

                    Else
                        Alert("No se encontraron proveedores con este nombre o no. de proveedor", Me, 1, 3)
                    End If


                Else
                    Alert("No se pudo buscar el proveedor " & txtNombreBuscar.Text.Replace("'", "") & ": " & strMensaje.Replace("'", "") & "", Me, 1, 3)

                End If

            Else

                Alert("Favor de buscar con el nombre o numero de proveedor ", Me, 1, 3)
            End If


        Catch ex As Exception
            Alert("Error: " & ex.Message.Replace("'", "") & "", Me, 1, 4)

        End Try
    End Sub

    Protected Sub grvProveedor_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvProveedor.RowCommand

        If (e.CommandName = "AgregarProveedor") Then
            Dim dtProveedores As New DataTable()
            dtProveedores.Columns.AddRange(New DataColumn(9) {New DataColumn("noProveedor"), New DataColumn("nombre"), New DataColumn("domicilio"), New DataColumn("ciudad"), New DataColumn("rfc"), New DataColumn("nomfis"), New DataColumn("mail"), New DataColumn("contra"), New DataColumn("telefono"), New DataColumn("seleccionado")})
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
            dtAprobadas.Columns.AddRange(New DataColumn(16) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus"), New DataColumn("fechaSolicitud"), New DataColumn("nombreProveedor"), New DataColumn("total")})


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

                                Alert("Error al guardar solicitudes", Me, 1, 4)
                            Else

                                If rowen.Item(6) <> "" Then
                                    'Si tiene email

                                    Dim contenido As String = ""
                                    contenido = "Por este medio solicitamos la cotización de artículos y servicios. " & vbCrLf _
                                  & "Favor de iniciar sessión en el portal de proveedores de JMAS o de acceder al siguiente enlace: " & vbCrLf
                                    'id = idCotizacion key = idProveedor
                                    Dim llave As String = "Jmas"
                                    Dim llaveCifrada As New clsCifrado(llave)
                                    Dim aCifrarId = dtsCotiza.Tables("Cotizacion").Rows(0).Item(0).ToString
                                    Dim aCifrarKey = rowen.Item(0)
                                    Dim param1 As String = "?id=" & llaveCifrada.EncryptData(aCifrarId)
                                    Dim param2 As String = "&key=" & llaveCifrada.EncryptData(aCifrarKey)
                                    '       Dim decrypted As String = llaveCifrada.DecryptData(param1.Substring(4))
                                    Dim enlace As String = ("http://201.147.15.182/Compras/wfrmCapturarCotizacion.aspx" & param1 & param2).Replace("+", "%2B")

                                    contenido &= enlace

                                    Dim mensaje As String = EnviarCorreo(contenido, rowen.Item(6).ToString.Trim, "Solicitud de cotización")
                                End If
                                okCount &= If(okCount = "", dtsCotiza.Tables("Cotizacion").Rows(0).Item(0).ToString, " - " & dtsCotiza.Tables("Cotizacion").Rows(0).Item(0).ToString)
                            End If


                        Next



                        If okCount <> "" Then
                            Alert("Se han generado las siguientes solicitudes correctamente: " & okCount & "", Me, 1, 4)
                        Else
                            Alert("Error al guardar solicitudes", Me, 1, 4)
                        End If

                        'Session("Cotizacion") = dtAprobadas

                        'ifrSolicitud.Src = "wfrmReporteSolicitudCotizacion.aspx?key=" & Request.QueryString("key").Replace("+", "%2B")
                        'Alert("Espere mientras se genera la solicitud")

                        Timer1.Enabled = True
                    Else
                        Alert("Favor de seleccionar articulos para cotizar", Me, 1, 3)
                    End If
                Else
                    Alert("Favor de seleccionar proveedores ", Me, 1, 3)
                End If
            Else
                Alert("Debe especificar un tiempo de entrega ", Me, 1, 3)
            End If

        Catch ex As Exception
            Alert("Error al guardar cambios: " & ex.Message & ": ", Me, 1, 4)
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
            Alert("Error al guardar cambios: " & ex.Message & ": ", Me, 1, 4)
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




#End Region



End Class
