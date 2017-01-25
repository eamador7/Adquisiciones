Imports System.Data
Imports System.Net.Mail.MailMessage
Imports CrystalDecisions.CrystalReports.Engine
Imports System.IO
Imports System.Collections
Imports CrystalDecisions.Shared
Imports KeepAutomation
Imports clsFunciones

Partial Class wfrmRequisicionCompra
    Inherits System.Web.UI.Page

#Region "Eventos"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            'txtCodigo.Attributes.Add("onkeypress", "button_click(this,'" + btnBuscarProducto.ClientID + "')")
            'txtDescripcion.Attributes.Add("onkeypress", "button_click(this,'" + btnBuscarProducto.ClientID + "')")

            Dim clsFunciones As New clsFunciones
            Dim strMensaje As String
            'Creacion del encabezado de la tabla de articulos y se asigna al ViewState
            Dim dtArticulos As New DataTable()
            dtArticulos.Columns.AddRange(New DataColumn(8) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("cco_id"), New DataColumn("cco_numero"), New DataColumn("contrato"), New DataColumn("proveedor")})
            ViewState("DetRequi") = dtArticulos
            grvArticulos.DataSource = dtArticulos
            grvArticulos.DataBind()
            If Request.QueryString("idrequisicion") <> "" Then 'Si se recibe una requi para modificar
                If Request.QueryString("intidestatus") <> "5" Then 'Si no está cancelada
                    Dim dtsRequisiciones As New DataSet
                    If Request.QueryString("intidestatus") <> "0" Then
                        hdnMotivoCambio.Value = Request.QueryString("Motivo")
                        hdnRequisicionCompra.Value = Request.QueryString("idrequisicion")
                        'txtFolio.Text = Request.QueryString("idrequisicion")
                    End If

                    'Cargamos el maestro de la requisicion
                    strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsRequisiciones, "Requisiciones", "Cargar_RequisicionesCompra",
                Request.QueryString("idrequisicion") & ",0,,," & Session("departamento"),
                 "@intIdRequisicion,@vchIdEstatus,@vchFechaDesde,@vchFechaHasta,@intDepartamento", dtsRequisiciones.Tables("Requisiciones"))

                    If strMensaje = "OK" Then
                        If dtsRequisiciones.Tables("Requisiciones").Rows.Count > 0 Then
                            'Se asignan valores de la busqueda de la requisicion para modificarla
                            txtnoEmpleado.Text = dtsRequisiciones.Tables("Requisiciones").Rows(0).Item("numEmpleado")
                            txtNombre.Text = dtsRequisiciones.Tables("Requisiciones").Rows(0).Item("nombreEmpleado")
                            txtFecha.Text = dtsRequisiciones.Tables("Requisiciones").Rows(0).Item("fecha")


                            Dim dtsProductos As New DataSet

                            strMensaje = clsFunciones.Llena_Grid(grvDetRequi, "MIGRACION", dtsProductos, "Productos", "Cargar_DetalleRequisicionCompra",
                                     Request.QueryString("idrequisicion"),
                                     "@intIdRequisicion")

                            If strMensaje = "OK" Then
                                If dtsProductos.Tables("Productos").Rows.Count > 0 Then
                                    For i = 0 To dtsProductos.Tables("Productos").Rows.Count - 1
                                        dtArticulos.Rows.Add(dtsProductos.Tables("Productos").Rows(i).Item(1),
                                                    dtsProductos.Tables("Productos").Rows(i).Item(2),
                                                    dtsProductos.Tables("Productos").Rows(i).Item(3),
                                                    dtsProductos.Tables("Productos").Rows(i).Item(4),
                                                    dtsProductos.Tables("Productos").Rows(i).Item(5),
                                                    dtsProductos.Tables("Productos").Rows(i).Item(6),
                                                    dtsProductos.Tables("Productos").Rows(i).Item(7))

                                    Next
                                    ViewState("Productos") = dtArticulos
                                    grvDetRequi.DataSource = dtArticulos
                                    grvDetRequi.DataBind()
                                End If

                            Else
                                Alert("Error al cargar la requisicion de salida de almacen RE" & Request.QueryString("idrequisicion") & ": " & strMensaje.Replace("'", "") & "", Me, 1, 4)
                            End If
                        Else
                            Alert("Problemas al cargar la requisicion de salida de almacen RE" & Request.QueryString("idrequisicion") & ": No se encontró en consulta", Me, 1, 4)
                        End If
                    Else
                        Alert("Error al cargar la requisicion: " & strMensaje.Replace("'", "") & "", Me, 1, 4)
                    End If
                Else
                    If Request.QueryString("intidestatus") = "5" Then
                        Alert("La requisicion RE " & Request.QueryString("idrequisicion") & " fue cancelada, no puede modificarse", Me, 1, 4)
                    End If
                End If
            Else
                txtNombre.Text = Session("nombre")
                txtnoEmpleado.Text = Session("intEmpleado")
                txtFecha.Text = Now
            End If
        End If
    End Sub

    Protected Sub btnBuscarProducto_Click(sender As Object, e As EventArgs) Handles btnBuscarProducto.Click
        If txtCodigo.Text <> "" Or txtDescripcion.Text <> "" Then 'Si se capturó codigo o nombre del articulo
            Dim strMensaje As String = ""
            Dim strQuery As String = ""
            Dim dtsArticulos As New DataSet
            Dim dtsContratos As New DataSet
            Dim clsFunciones As New clsFunciones


            '----------------------------------------------POR ALMACEN ----------------------------------------------------------
            'Se busca el producto en base al codigo o concepto capturado
            strMensaje = clsFunciones.Llena_Dataset("FILTROS", dtsArticulos, "Productos", "Cargar_ProductosPermitidos", txtCodigo.Text & "," & txtDescripcion.Text & "," & Session("departamento") & ",", _
                                                    "intCodigo,vchProducto,vchDepartamento,vchordencompra", dtsArticulos.Tables("Productos"))

            If strMensaje = "OK" Then 'Si se realizo la consulta sin error
                If dtsArticulos.Tables("Productos").Rows.Count > 0 Then 'Si existe el folio
                    ''La existencia y el tipo de salida se almacenan en campos ocultos
                    'If dtsArticulos.Tables("Productos").Rows.Count = 1 Then 'Si es un solo resultado de la busqueda
                    '    txtCodigo.Text = dtsArticulos.Tables("Productos").Rows(0).Item(0)
                    '    txtDescripcion.Text = dtsArticulos.Tables("Productos").Rows(0).Item(1)
                    '    hdnPrecio.Value = dtsArticulos.Tables("Productos").Rows(0).Item(8)
                    '    hdnCco_id.Value = dtsArticulos.Tables("Productos").Rows(0).Item(5)
                    '    hdnCco_numero.Value = dtsArticulos.Tables("Productos").Rows(0).Item(6)
                    '    hdnExistencia.Value = dtsArticulos.Tables("Productos").Rows(0).Item(3)
                    '    hdnProveedor.Value = ""
                    '    hdnContrato.Value = ""
                    '    pnlArticulos.Visible = False


                    'Else
                    Dim dtArticulos As New DataTable()
                    Dim intContadorArticulos As Integer
                    'Agregamos la cabecera del gridview
                    dtArticulos.Columns.AddRange(New DataColumn(8) {New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("udm"), New DataColumn("precio"), New DataColumn("cco_id"), New DataColumn("cco_numero"), New DataColumn("existencia"), New DataColumn("contrato"), New DataColumn("proveedor")})
                    'Recorremos el dataset para llenar el gridView
                    For intContadorArticulos = 0 To dtsArticulos.Tables("Productos").Rows.Count - 1
                        'Se agrega el articulo al gridView
                        dtArticulos.Rows.Add(dtsArticulos.Tables("Productos").Rows(intContadorArticulos).Item(0), _
                                             dtsArticulos.Tables("Productos").Rows(intContadorArticulos).Item(1), _
                                             dtsArticulos.Tables("Productos").Rows(intContadorArticulos).Item(2), _
                                              dtsArticulos.Tables("Productos").Rows(intContadorArticulos).Item(8), _
                                               dtsArticulos.Tables("Productos").Rows(intContadorArticulos).Item(5), _
                                                dtsArticulos.Tables("Productos").Rows(intContadorArticulos).Item(6), _
                                                dtsArticulos.Tables("Productos").Rows(intContadorArticulos).Item(3), _
                                                "", "")
                    Next
                    'Mandamos el resultado de la busqueda al ViewState y llenamos el gridView
                    ViewState("BuscarArticulos") = dtArticulos
                    grvArticulos.DataSource = dtArticulos
                    grvArticulos.DataBind()
                    'Se pone visible el panel del grid con los resultados de la busqueda
                    pnlArticulos.Visible = True

                    'End If
                Else
                    Alert("Codigo o producto no existe", Me, 1, 4)
                End If
            Else
                Alert("No se pudo buscar el codigo " & txtCodigo.Text.Replace("'", "") & ": " & strMensaje.Replace("'", "") & "", Me, 1, 4)
            End If

            '----------------------------------------------FIN POR ALMACEN ------------------------------------------------------
        End If
    End Sub

    Protected Sub grvArticulos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvArticulos.RowCommand
        If (e.CommandName = "Seleccionar") Then
            Dim dtsContratos As New DataSet
            Dim strMensaje As String = ""
            Dim clsFunciones As New clsFunciones

            ' Obtenemos la fila seleccionada
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            txtDescripcion.Text = ViewState("BuscarArticulos").Rows(index).Item(1)
            txtCodigo.Text = ViewState("BuscarArticulos").Rows(index).Item(0)
            hdnPrecio.Value = ViewState("BuscarArticulos").Rows(index).Item(3)
            hdnCco_id.Value = ViewState("BuscarArticulos").Rows(index).Item(4)
            hdnCco_numero.Value = ViewState("BuscarArticulos").Rows(index).Item(5)
            hdnExistencia.Value = ViewState("BuscarArticulos").Rows(index).Item(6)
            hdnContrato.Value = ViewState("BuscarArticulos").Rows(index).Item(7)
            hdnProveedor.Value = ViewState("BuscarArticulos").Rows(index).Item(8)
            'pnlArticulos.Visible = False
            txtUDM.Text = ViewState("BuscarArticulos").Rows(index).Item(2)
            txtCodigo.ReadOnly = True
            txtDescripcion.ReadOnly = True


            Dim dtContrato As New DataTable()
            Dim intContadorArticulos As Integer
            'Agregamos la cabecera del gridview
            dtContrato.Columns.AddRange(New DataColumn(10) {New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("udm"), New DataColumn("precio"), New DataColumn("cco_id"), New DataColumn("cco_numero"), New DataColumn("existencia"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("adicional"), New DataColumn("montoDisponible")})
            '---------------------------------------------POR CONTRATO-------------------------------------------------------


            'Se busca el producto en base al codigo o concepto capturado en los contratos
            strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsContratos, "DetalleContratos", "Buscar_DetalleContratoPorArticulo", txtCodigo.Text.Trim & "," & txtDescripcion.Text.Trim & ",", _
                                                    "@intIdCodigo,@vchConcepto", dtsContratos.Tables("DetalleContratos"))

            'Se evalua el articulo para ver si se encuentra en un contrato y si es que hay cantidad disponible
            If strMensaje = "OK" Then 'Si se realizo la consulta sin error
                If dtsContratos.Tables("DetalleContratos").Rows.Count > 0 Then 'Si existe el folio
                    For intContadorArticulos = 0 To dtsContratos.Tables("DetalleContratos").Rows.Count - 1
                        'Se agrega el articulo al gridView
                        dtContrato.Rows.Add(dtsContratos.Tables("DetalleContratos").Rows(intContadorArticulos).Item(2), _
                                             dtsContratos.Tables("DetalleContratos").Rows(intContadorArticulos).Item(3), _
                                             dtsContratos.Tables("DetalleContratos").Rows(intContadorArticulos).Item(6), _
                                              dtsContratos.Tables("DetalleContratos").Rows(intContadorArticulos).Item(10), _
                                               dtsContratos.Tables("DetalleContratos").Rows(intContadorArticulos).Item(8), _
                                                dtsContratos.Tables("DetalleContratos").Rows(intContadorArticulos).Item(9), _
                                               99999, _
                                               dtsContratos.Tables("DetalleContratos").Rows(intContadorArticulos).Item(0), _
                                               dtsContratos.Tables("DetalleContratos").Rows(intContadorArticulos).Item(7),
                                               dtsContratos.Tables("DetalleContratos").Rows(intContadorArticulos).Item(12),
                                               dtsContratos.Tables("DetalleContratos").Rows(intContadorArticulos).Item(11))

                    Next
                End If

                'Mandamos el resultado de la busqueda al ViewState y llenamos el gridView
                ViewState("Contratos") = dtContrato
                grvContrato.DataSource = dtContrato
                grvContrato.DataBind()
                'Se pone visible el panel del grid con los resultados de la busqueda
                pnlContrato.Visible = True
            End If



        End If
    End Sub


    Protected Sub grvContrato_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvContrato.RowCommand
        If (e.CommandName = "Seleccionar") Then
            Dim dtsContratos As New DataSet
            Dim strMensaje As String = ""
            Dim clsFunciones As New clsFunciones

            ' Obtenemos la fila seleccionada
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            hdnMontoContrato.Value = ViewState("Contratos").Rows(index).Item(10)
            hdnPrecio.Value = ViewState("Contratos").Rows(index).Item(3)
            txtAdicional.Text = ViewState("Contratos").Rows(index).Item(9)
            hdnContrato.Value = ViewState("Contratos").Rows(index).Item(7)
            hdnProveedor.Value = ViewState("Contratos").Rows(index).Item(8)
            pnlArticulos.Visible = False
            pnlContrato.Visible = False

            txtAdicional.ReadOnly = True



        End If
    End Sub

    Protected Sub btnAgregarProducto_Click(sender As Object, e As EventArgs) Handles btnAgregarProducto.Click
        Try
            If txtCodigo.Text.Trim <> "" And txtDescripcion.Text.Trim <> "" Then ' Si se capturo codigo del articulo
                If txtCantidad.Text <> "" Then ' Si se capturo cantidad

                    txtUDM.Text = ""
                    Dim dt As New DataTable()
                    If Not ViewState("DetRequi") Is Nothing Then
                        dt = ViewState("DetRequi")
                    End If
                    'Dim i As Integer
                    'For i = 0 To grvDetRequi.Rows.Count - 1
                    '    If txtCodigo.Text = grvDetRequi.Rows(i).Cells(0).Text Then
                    '        Alert("El artículo " & txtDescripcion.Text & " ya fue agregado previamente")
                    '        Exit Sub
                    '    End If
                    'Next


                    ' Si no hay monto disponible en el contrato
                    If hdnContrato.Value <> "" Then
                        If CDec(hdnMontoContrato.Value) < (CDec(txtCantidad.Text) * CDec(hdnPrecio.Value)) Then
                            Alert("No hay monto disponible en el contrato para este artículo", Me, 1, 4)
                            Exit Sub
                        End If

                    Else 'No se encontró en los contratos

                        ' El articulo no está en un contrato y hay existencia suficiente en el almacen para la operacion
                        If CDec(txtCantidad.Text) < hdnExistencia.Value Then
                            Alert("Hay existencia en el almacen para este producto. Sin embargo, se agregará el artículo a la requisición.", Me, 1, 5)

                        End If
                    End If



                    'Se agrega el articulo al gridView
                    dt.Rows.Add(grvDetRequi.Rows.Count + 1, txtCodigo.Text, txtDescripcion.Text & " - " & txtAdicional.Text, txtCantidad.Text, hdnPrecio.Value, hdnCco_id.Value, hdnCco_numero.Value, hdnContrato.Value, hdnProveedor.Value)
                    ViewState("DetRequi") = dt
                    grvDetRequi.DataSource = dt
                    grvDetRequi.DataBind()
                    hdnPrecio.Value = 0
                    hdnCco_id.Value = ""
                    hdnCco_numero.Value = ""
                    hdnExistencia.Value = 0
                    hdnMontoContrato.Value = 0
                    txtCantidad.Text = ""
                    txtCodigo.Text = ""
                    txtDescripcion.Text = ""
                    txtAdicional.Text = ""
                    hdnProveedor.Value = ""
                    hdnContrato.Value = ""
                    txtCodigo.ReadOnly = False
                    txtDescripcion.ReadOnly = False
                    txtAdicional.ReadOnly = False
                    pnlArticulos.Visible = False
                    pnlContrato.Visible = False
                    pnlRequi.Visible = True
                Else
                    Alert("Favor de anotar la cantidad", Me, 1, 5)
                End If
                Else
                'txtDestino.Text = ""
                Alert("Favor de seleccionar el producto", Me, 1, 5)
            End If

        Catch ex As Exception
            Alert("Error al tratar de agregar articulo: " & ex.Message, Me, 1, 4)
        End Try
    End Sub

    Protected Sub grvDetrequi_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvDetrequi.RowCommand
        If (e.CommandName = "Eliminar") Then
            ' Obtenemos la fila seleccionada
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            'Cargamos los articulos a un datatable para eliminarlo
            Dim dt As New DataTable()
            If Not ViewState("DetRequi") Is Nothing Then
                dt = ViewState("DetRequi")
            End If
            dt.Rows(index).Delete()
            ViewState("DetRequi") = dt
            grvDetrequi.DataSource = dt
            grvDetrequi.DataBind()
        End If
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Session.Contents.Count <= 0 Then
            Alert("Se requiere entre de nuevo al sistema por superar 20 minutos sin usarlo", Me)

            Exit Sub
        End If
        Try
            If txtAdicional.Text <> "" Then

                If ViewState("DetRequi").Rows.Count > 0 Then 'Si existen articulos para la requisicion
                    Dim okCount As String = ""
                    Dim intContador As Integer
                    Dim strMensaje, strModificacion As String
                    strModificacion = ""
                    Dim strMensajeModificar As String = "Se creo la "
                    Dim clsFunciones As New clsFunciones
                    Dim dtsrequi As New DataSet

                    Dim dtTmp As New DataTable()



                    If hdnRequisicionCompra.Value = "" Then
                        hdnRequisicionCompra.Value = "0"
                    Else
                        strMensajeModificar = "Se modifico la "
                        'strModificacion = hdnMotivoCambio.Value.Replace(",", "*")
                    End If
                    btnGuardar.Enabled = False

                    'Creamos un rango de columnas para la datatable temporal que almacenara los articulos
                    dtTmp.Columns.AddRange(New DataColumn(8) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("cco_id"), New DataColumn("cco_numero"), New DataColumn("contrato"), New DataColumn("proveedor")})

                    For intContador = 0 To ViewState("DetRequi").Rows.Count - 1 'Recorremos el ViewState para obtener los articulos
                        dtTmp.Rows.Add(CInt(ViewState("DetRequi").Rows(intContador).Item(0)),
                                       CInt(ViewState("DetRequi").Rows(intContador).Item(1)),
                                       ViewState("DetRequi").Rows(intContador).Item(2),
                                       CDec(ViewState("DetRequi").Rows(intContador).Item(3)),
                                       CDec(ViewState("DetRequi").Rows(intContador).Item(4)),
                                       Session("Departamento") & "0",
                                       ViewState("DetRequi").Rows(intContador).Item(6),
                                       ViewState("DetRequi").Rows(intContador).Item(7),
                                       ViewState("DetRequi").Rows(intContador).Item(8))
                        ' cco_id ---> codigo de departamento item5
                    Next

                    ' Se ordenan los elementos de la lista para luego formar grupos en comun para separarlos en varias requisiciones
                    Dim warray() As DataRow = dtTmp.Select("contrato =''", Nothing) ' Arreglo ordenado de renglones de la tabla
                    Dim darray() As DataRow = dtTmp.Select("contrato <>''", "contrato,proveedor") ' Arreglo ordenado de renglones de la tabla
                    Dim gSet As New DataSet ' Coleccion de tablas de detalle
                    Dim contractLessCollection As New DataTable
                    contractLessCollection.Columns.AddRange(New DataColumn(8) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("cco_id"), New DataColumn("cco_numero"), New DataColumn("contrato"), New DataColumn("proveedor")})

                    ' Se agrega la coleccion de renglones sin contrato y proveedor
                    For Each clRow As DataRow In warray
                        ' Se resetea el indice del detalle por cada requ
                        clRow.Item(0) = contractLessCollection.Rows.Count + 1
                        contractLessCollection.ImportRow(clRow)


                        'contractLessCollection.Rows.Add(clRow)
                    Next
                    If contractLessCollection.Rows.Count > 0 Then
                        gSet.Tables.Add(contractLessCollection)
                    End If
                    Dim dynTable As New DataTable 'tabla vacia
                    dynTable.Columns.AddRange(New DataColumn(8) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("cco_id"), New DataColumn("cco_numero"), New DataColumn("contrato"), New DataColumn("proveedor")})
                    Dim currentContract As String = "", currentSupplier = ""
                    If darray.Count > 0 Then
                        For Each rowen As DataRow In darray
                            If currentContract = "" And currentSupplier = "" Then ' Si es el primer elemento
                                'dynTable.ImportRow(rowen)
                                currentContract = rowen.Item(7).ToString
                                currentSupplier = rowen.Item(8).ToString
                            End If

                            If currentSupplier <> rowen.Item(8).ToString Then 'Cada que hay un cambio de proveedor se crea una tabla y se agrega la actual
                                gSet.Tables.Add(dynTable)
                                dynTable = New DataTable
                                dynTable.Columns.AddRange(New DataColumn(8) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("cco_id"), New DataColumn("cco_numero"), New DataColumn("contrato"), New DataColumn("proveedor")})
                                ' Se resetea el indice del detalle
                                rowen.Item(0) = dynTable.Rows.Count + 1
                                dynTable.ImportRow(rowen)
                                currentContract = rowen.Item(7).ToString
                                currentSupplier = rowen.Item(8).ToString
                            Else
                                dynTable.ImportRow(rowen)
                            End If
                        Next
                        gSet.Tables.Add(dynTable)
                    End If

                    For Each dtable As DataTable In gSet.Tables ' Se guarda una requisicion por cada grupo de detalle
                        hdnContrato.Value = dtable.Rows.Item(0).Item(7).ToString
                        hdnProveedor.Value = dtable.Rows.Item(0).Item(8).ToString
                        strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsrequi, "requi", "Inserta_RequisicionCompra",
                                 hdnRequisicionCompra.Value & "," & Session("IdUsuario") & ",A," & hdnContrato.Value & "," & hdnProveedor.Value & "," &
                                 "RE," & txtObservaciones.Text.ToUpper.Replace(",", "*") & "," & strModificacion,
                                 "@intIdrequi,@intSolicitante,@chEstatus,@intIdContrato,@intIdProveedor,@chSerie,@vchObservaciones,@vchMotivoModifica",
                                 dtable)

                        If strMensaje <> "OK" Then
                            If okCount <> "" Then
                                Alert(strMensajeModificar & "requisición de compra: RE" & okCount & "", Me)
                            End If

                            Alert("Error al guardar Requisición: " & strMensaje & "", Me)
                        Else
                            okCount &= If(okCount = "", dtsrequi.Tables("requi").Rows(0).Item(0).ToString, "-" & dtsrequi.Tables("requi").Rows(0).Item(0))
                        End If
                    Next

                    ' Comentado para no generar pdf de las requisiciones
                    '          If okCount <> "" Then

                    'Inicia Impresion-----------------------------------------------------------------------------------------------
                    'Se generan las requisiciones para impresión


                    'Dim requiArray() As String = okCount.Split(",")

                    'For Each idRequi As String In requiArray ' Se genera un pdf por cada requisicion
                    '   ifrRequisicion.Src = "wfrmReporteRequisicionCompra.aspx?key=" & Request.QueryString("key").Replace("+", "%2B") & _
                    '"&intRequisicion=" & okCount

                    'ifrOrdenSalida.Src = "wfrmReporteRequisicionCompra.aspx?key=" & Request.QueryString("key").Replace("+", "%2B") & _
                    '            "&intRequisicion=" & okCount


                    'Next


                    'Fin Impresion-----------------------------------------------------------------------------------------------
                    'End If


                    Alert(strMensajeModificar & "requisición de compra: RE" & okCount & "", Me)
                    Timer1.Enabled = True
                    ''Mandamos guardar la solicitud con sus articulos
                    'strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsrequi, "requi", "Inserta_RequisicionCompra", _
                    '             hdnRequisicionCompra.Value & "," & Session("IdUsuario") & ",A," & hdnContrato.Value & "," & hdnProveedor.Value & "," & _
                    '             "RE," & txtObservaciones.Text.ToUpper.Replace(",", "*") & "," & strModificacion, _
                    '             "@intIdrequi,@intSolicitante,@chEstatus,@intIdContrato,@intIdProveedor,@chSerie,@vchObservaciones,@vchMotivoModifica", _
                    '             dtTmp)
                    'If strMensaje = "OK" Then ' Si la solicitud se guardó correctamente mostramos el numero de requisicion
                    '    Alert(strMensajeModificar & "requisición de compra: RE" & dtsrequi.Tables("requi").Rows(0).Item(0) & "")
                    '    ClearFields()
                    '    'If Request.QueryString("intidestatus") = "" Or Request.QueryString("intidestatus") = "2" Then 'Solo se envia mensaje en nuevas o modificaciones del jefe de departamento
                    '    '    Dim dtsDepartamentos As New DataSet
                    '    '    strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsDepartamentos, "Telefono", "Cargar_Departamentos", _
                    '    '                ddlDepartamento.SelectedValue, "@intDepartamento", dtsDepartamentos.Tables("Telefono"))
                    '    '    If strMensaje = "OK" Then 'Si se realizo la consulta del telefono
                    '    '        Dim LlaveCifrada As New clsCifrado("Jmas")
                    '    '        Dim requiCifrada As String = LlaveCifrada.EncryptData(dtsRequisicion.Tables("Requisicion").Rows(0).Item(0))
                    '    '        If dtsDepartamentos.Tables("Telefono").Rows(1).Item(2) <> "" Then
                    '    '            strMensaje = clsFunciones.EjecutaProcedimiento("SMS", "Agrega_BitacoraMensajes_Usuario", _
                    '    '                        "5,-" & dtsDepartamentos.Tables("Telefono").Rows(1).Item(2) & "-,,," & _
                    '    '                        "Avisos " & strMensajeModificar & "salida de almacen R" & dtsRequisicion.Tables("Requisicion").Rows(0).Item(0) & " pedida para " & txtSolictado.Text.Replace(",", "") & " esperando aprobacion en http://201.147.15.182/,0", _
                    '    '                        "@intidOrigen,@vchTelefono,@vchFecha,@vchHora,@vchComando,@intidUsuario")
                    '    '        End If
                    '    '        If dtsDepartamentos.Tables("Telefono").Rows(1).Item(3) <> "" Then
                    '    '            EnviarCorreo(strMensajeModificar & "requi de salida de almacen R" & dtsRequisicion.Tables("Requisicion").Rows(0).Item(0) & " pedida para entrega al empleado  " & txtSolictado.Text.Replace(",", "") & " esperando su aprobacion en http://201.147.15.182/", dtsDepartamentos.Tables("Telefono").Rows(1).Item(3))
                    '    '        End If
                    '    '        'If dtsDepartamentos.Tables("Telefono").Rows(1).Item(2) <> "" Then
                    '    '        '    strMensaje = clsFunciones.EjecutaProcedimiento("SMS", "Agrega_BitacoraMensajes_Usuario", _
                    '    '        '                "5,-" & dtsDepartamentos.Tables("Telefono").Rows(1).Item(2) & "-,,," & _
                    '    '        '                "Avisos " & strMensajeModificar & "salida de almacen R" & dtsRequisicion.Tables("Requisicion").Rows(0).Item(0) & " esperando aprobacion en http://201.147.15.182/almacen/wfrmAprobarExpress.aspx?id=" & requiCifrada & ",0", _
                    '    '        '                "@intidOrigen,@vchTelefono,@vchFecha,@vchHora,@vchComando,@intidUsuario")
                    '    '        'End If
                    '    '        'If dtsDepartamentos.Tables("Telefono").Rows(1).Item(3) <> "" Then
                    '    '        '    EnviarCorreo(strMensajeModificar & "requi de salida de almacen R" & dtsRequisicion.Tables("Requisicion").Rows(0).Item(0) & " esperando su aprobacion en http://201.147.15.182/almacen/wfrmAprobarExpress.aspx?id=" & requiCifrada & "", dtsDepartamentos.Tables("Telefono").Rows(1).Item(3))
                    '    '        'End If
                    '    '    End If
                    '    'End If

                    '    'Timer1.Enabled = True
                    'Else
                    '    Alert("Error al guardar Requisición: " & strMensaje & "")
                    'End If
                Else
                    Alert("Favor de agregar algún artículo a la requisición de compra", Me, 1, 3)
                End If
            Else
                Alert("Debe capturar las especificaciones técnicas", Me, 1, 3)
            End If

        Catch ex As Exception
            Alert("Error al tratar de guardar requisición: " & ex.Message, Me, 1, 3)
        End Try

    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Response.Redirect("wfrmRequisicionCompra.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
    End Sub

    Protected Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        txtCodigo.Text = ""
        txtDescripcion.Text = ""
        txtAdicional.Text = ""
        txtCantidad.Text = ""
        pnlArticulos.Visible = False
        pnlContrato.Visible = False
        txtCodigo.ReadOnly = False
        txtDescripcion.ReadOnly = False
        txtAdicional.ReadOnly = False

    End Sub

#End Region

#Region "Procedimientos"



    Public Sub ClearFields()
        'txtFolio.Text = ""
        txtFecha.Text = Now
        txtCodigo.Text = ""
        txtDescripcion.Text = ""
        txtAdicional.Text = ""
        txtCantidad.Text = ""

        pnlArticulos.Visible = False
        txtObservaciones.Text = ""

        Dim dtArticulos As New DataTable()
        dtArticulos.Columns.AddRange(New DataColumn(6) {New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("cco_id"), New DataColumn("cco_numero")})
        ViewState("DetRequi") = dtArticulos
        grvArticulos.DataSource = dtArticulos
        grvArticulos.DataBind()
        grvDetrequi.DataSource = dtArticulos
        grvDetrequi.DataBind()
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
                Alert("Error al enviar el correo de confirmacion:" & ex.Message, Me)
                'MsgBox(ex.ToString)
            End Try
        Catch ex As Exception
            Alert("Error al enviar el correo de confirmacion:" & ex.Message, Me)
        End Try
    End Sub




#End Region

#Region "Funciones"
    'Public Function CheckValidation(ByRef mensaje As String) As Boolean
    '    Dim OK As Boolean = True

    '    If txtContrato.Text = "" Then
    '        OK = False
    '        mensaje &= "Debe especificar un contrato."
    '    End If

    '    If rbtConvenio.Checked And CInt(Val(txtPorcentaje.Text)) <= 0 Then
    '        OK = False
    '        mensaje &= vbCrLf & "Si selecciona 'Convenio', debe especificar un porcentaje."
    '    End If

    '    Return OK
    'End Function


#End Region




End Class
