Imports System.Data
Imports System.Drawing
Imports clsFunciones


Partial Class wfrmAutorizarPresupuesto
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Usuario y perfil de prueba
        'Session("IdUsuario") = 4
        ''Session("idPerfil") = 24
        'Session("departamento") = 506
        Session("caller") = "wfrmAutorizarPresupuesto.aspx"

        If Not Page.IsPostBack Then
            grvSolicitudes.Columns(6).Visible = False
            'Alert("Recuerde que todas los artículos de las requisiciones utilizadas que no sean autorizados se cancelaran automáticamente.", Me, 1, 5)
            Dim clsFunciones As New clsFunciones
            Dim dtsEstatus As New DataSet
            Dim strMensaje As String

            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(14) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus"), New DataColumn("idEstatus")})


            Dim filterEstatus As String = "11-12"



            Dim dtsSolicitudes As New DataSet


            strMensaje = clsFunciones.Llena_Grid(grvSolicitudes, "MIGRACION", dtsSolicitudes, "Solicitudes", "Cargar_RequisicionesCompra",
             "," & filterEstatus & ",,," & Session("departamento") & ",1",
            "@intIdRequisicion,@vchIdEstatus,@vchFechaDesde,@vchFechaHasta,@intDepartamento")

            If strMensaje = "OK" Then
                dtsSolicitudes.Tables("Solicitudes").Columns.Add("total").DefaultValue = 0
                ViewState("Solicitudes") = dtsSolicitudes.Tables("Solicitudes")
                grvSolicitudes.DataSource = dtsSolicitudes.Tables("Solicitudes")
                grvSolicitudes.DataBind()
                ViewState("Requisiciones") = Nothing
                grvRequisiciones.DataSource = Nothing
                grvRequisiciones.DataBind()
                'Materiales
                ViewState("Detalle") = Nothing
                grvAprobadas.DataSource = Nothing
                grvAprobadas.DataBind()
                'Servicios
                ViewState("Detalle1") = Nothing
                grvServicios.DataSource = Nothing
                grvServicios.DataBind()
                'Canceladas manual
                ViewState("Detalle2") = Nothing
                grvCanceladas.DataSource = Nothing
                grvCanceladas.DataBind()
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

            ViewState("Requisiciones") = Nothing
            grvRequisiciones.DataSource = Nothing
            grvRequisiciones.DataBind()
            'Materiales
            ViewState("Detalle") = Nothing
            grvAprobadas.DataSource = Nothing
            grvAprobadas.DataBind()
            'Servicios
            ViewState("Detalle1") = Nothing
            grvServicios.DataSource = Nothing
            grvServicios.DataBind()
            'Canceladas manual
            ViewState("Detalle2") = Nothing
            grvCanceladas.DataSource = Nothing
            grvCanceladas.DataBind()
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
            Dim dtsPresupuesto As New DataSet
            Dim dtPresupuesto As New DataTable


            'Presupuesto disponible
            strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsPresupuesto, "Presupuesto", "Cargar_Presupuesto_Depto",
                        Session("departamento").ToString() + "0",
                        "@intIdDepartamento", dtPresupuesto)
            If strMensaje = "OK" Then
                lblDepartamento.Text = dtsPresupuesto.Tables("Presupuesto").Rows(0).Item(1).ToString().Trim()
                lblMateriales.Text = "$" + CDec(dtsPresupuesto.Tables("Presupuesto").Rows(0).Item(2)).ToString("###,###,##0.00") 'materiales
                lblServicios.Text = "$" + CDec(dtsPresupuesto.Tables("Presupuesto").Rows(0).Item(3)).ToString("###,###,##0.00") 'servicios
                lblTotal.Text = "$" + (CDec(dtsPresupuesto.Tables("Presupuesto").Rows(0).Item(2)) + CDec(dtsPresupuesto.Tables("Presupuesto").Rows(0).Item(3))).ToString("###,###,##0.00") 'Total
                pnlInfo.Visible = True

                lblMateriales.ForeColor = If(CDec(lblMateriales.Text) < 0, Color.Red, Color.Black)
                lblTotal.ForeColor = If(CDec(lblTotal.Text) < 0, Color.Red, Color.Black)
                lblServicios.ForeColor = If(CDec(lblServicios.Text) < 0, Color.Red, Color.Black)
            End If

            'Detalle de requisiciones
            Dim filterEstatus As String = "11-12"



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
        If (e.CommandName = "Material") Then
            Dim dtAprobadas As New DataTable()
            dtAprobadas.Columns.AddRange(New DataColumn(14) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus"), New DataColumn("idEstatus")})

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
            'Servicios
            For i = 0 To grvServicios.Rows.Count - 1
                If ViewState("Requisiciones").Rows(index).Item(0) = grvServicios.Rows(i).Cells(0).Text And ViewState("Requisiciones").Rows(index).Item(1) = grvServicios.Rows(i).Cells(1).Text Then
                    Alert("El elemento ya fue agregado previamente", Me, 1, 3)
                    Exit Sub
                End If
            Next
            '   Canceladas manualmente
            For i = 0 To grvCanceladas.Rows.Count - 1
                If ViewState("Requisiciones").Rows(index).Item(0) = grvCanceladas.Rows(i).Cells(0).Text And ViewState("Requisiciones").Rows(index).Item(1) = grvCanceladas.Rows(i).Cells(1).Text Then
                    Alert("El elemento ya fue agregado previamente", Me, 1, 3)
                    Exit Sub
                End If
            Next

            'Incrementar lo seleccionado
            Dim indiceUsar As Integer = 0
            indiceUsar = hdnRequiIndex.Value
            ViewState("Solicitudes").rows(indiceUsar).item(12) += CDec(ViewState("Requisiciones").Rows(index).Item(4) * ViewState("Requisiciones").Rows(index).Item(5))

            'Dim dt As DataTable = ViewState("Solicitudes")
            'grvSolicitudes.DataSource = dt
            'grvSolicitudes.DataBind()

            dtAprobadas.Rows.Add(dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray)
            ViewState("Detalle") = dtAprobadas
            grvAprobadas.DataSource = dtAprobadas
            grvAprobadas.DataBind()
            pnlMateriales.Visible = True

            'Afecta total
            lblMateriales.Text = "$" + (CDec(lblMateriales.Text) - CDec(dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray(4) * dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray(5))).ToString("###,###,##0.00")
            lblTotal.Text = "$" + (CDec(lblTotal.Text) - CDec(dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray(4) * dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray(5))).ToString("###,###,##0.00")
            'Marcar en rojo si es menor a cero
            lblMateriales.ForeColor = If(CDec(lblMateriales.Text) < 0, Color.Red, Color.Black)
            lblTotal.ForeColor = If(CDec(lblTotal.Text) < 0, Color.Red, Color.Black)

            dtRequis.Rows.RemoveAt(CInt(e.CommandArgument))
            ViewState("Requisiciones") = dtRequis
            grvRequisiciones.DataSource = dtRequis
            grvRequisiciones.DataBind()
            'Servicios
        ElseIf (e.CommandName = "Service") Then

            Dim dtService As New DataTable()
            dtService.Columns.AddRange(New DataColumn(14) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus"), New DataColumn("idEstatus")})

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim dtRequis As New DataTable()
            If Not ViewState("Requisiciones") Is Nothing Then
                dtRequis = ViewState("Requisiciones")
            End If

            If Not ViewState("Detalle1") Is Nothing Then
                dtService = ViewState("Detalle1")
            End If

            'Recorrer el detalle verificando que no se haya duplicado ningun elemento
            Dim i As Integer
            For i = 0 To grvAprobadas.Rows.Count - 1
                If ViewState("Requisiciones").Rows(index).Item(0) = grvAprobadas.Rows(i).Cells(0).Text And ViewState("Requisiciones").Rows(index).Item(1) = grvAprobadas.Rows(i).Cells(1).Text Then
                    Alert("El elemento ya fue agregado previamente", Me, 1, 3)
                    Exit Sub
                End If
            Next
            'Servicios
            For i = 0 To grvServicios.Rows.Count - 1
                If ViewState("Requisiciones").Rows(index).Item(0) = grvServicios.Rows(i).Cells(0).Text And ViewState("Requisiciones").Rows(index).Item(1) = grvServicios.Rows(i).Cells(1).Text Then
                    Alert("El elemento ya fue agregado previamente", Me, 1, 3)
                    Exit Sub
                End If
            Next
            '   Canceladas manualmente
            For i = 0 To grvCanceladas.Rows.Count - 1
                If ViewState("Requisiciones").Rows(index).Item(0) = grvCanceladas.Rows(i).Cells(0).Text And ViewState("Requisiciones").Rows(index).Item(1) = grvCanceladas.Rows(i).Cells(1).Text Then
                    Alert("El elemento ya fue agregado previamente", Me, 1, 3)
                    Exit Sub
                End If
            Next

            'Incrementar lo seleccionado
            '    Dim indiceUsar As Integer = 0
            '    indiceUsar = hdnRequiIndex.Value
            '  ViewState("Solicitudes").rows(indiceUsar).item(12) += CDec(ViewState("Requisiciones").Rows(index).Item(4) * ViewState("Requisiciones").Rows(index).Item(5))

            'Dim dt As DataTable = ViewState("Solicitudes")
            'grvSolicitudes.DataSource = dt
            'grvSolicitudes.DataBind()

            dtService.Rows.Add(dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray)
            ViewState("Detalle1") = dtService
            grvServicios.DataSource = dtService
            grvServicios.DataBind()
            pnlServicios.Visible = True
            'Afecta total
            lblServicios.Text = "$" + (CDec(lblServicios.Text) - CDec(dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray(4) * dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray(5))).ToString("###,###,##0.00")
            lblTotal.Text = "$" + (CDec(lblTotal.Text) - CDec(dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray(4) * dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray(5))).ToString("###,###,##0.00")

            'Marcar en rojo si es menor a cero
            lblServicios.ForeColor = If(CDec(lblServicios.Text) < 0, Color.Red, Color.Black)
            lblTotal.ForeColor = If(CDec(lblTotal.Text) < 0, Color.Red, Color.Black)

            dtRequis.Rows.RemoveAt(CInt(e.CommandArgument))
            ViewState("Requisiciones") = dtRequis
            grvRequisiciones.DataSource = dtRequis
            grvRequisiciones.DataBind()
            'Canceladas
        ElseIf (e.CommandName = "Cancelar") Then

            Dim dtCanceladas As New DataTable()
            dtCanceladas.Columns.AddRange(New DataColumn(14) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus"), New DataColumn("idEstatus")})

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim dtRequis As New DataTable()
            If Not ViewState("Requisiciones") Is Nothing Then
                dtRequis = ViewState("Requisiciones")
            End If

            If Not ViewState("Detalle2") Is Nothing Then
                dtCanceladas = ViewState("Detalle2")
            End If

            'Recorrer el detalle verificando que no se haya duplicado ningun elemento
            Dim i As Integer
            For i = 0 To grvAprobadas.Rows.Count - 1
                If ViewState("Requisiciones").Rows(index).Item(0) = grvAprobadas.Rows(i).Cells(0).Text And ViewState("Requisiciones").Rows(index).Item(1) = grvAprobadas.Rows(i).Cells(1).Text Then
                    Alert("El elemento ya fue agregado previamente", Me, 1, 3)
                    Exit Sub
                End If
            Next
            'Servicios
            For i = 0 To grvServicios.Rows.Count - 1
                If ViewState("Requisiciones").Rows(index).Item(0) = grvServicios.Rows(i).Cells(0).Text And ViewState("Requisiciones").Rows(index).Item(1) = grvServicios.Rows(i).Cells(1).Text Then
                    Alert("El elemento ya fue agregado previamente", Me, 1, 3)
                    Exit Sub
                End If
            Next
            '   Canceladas manualmente
            For i = 0 To grvCanceladas.Rows.Count - 1
                If ViewState("Requisiciones").Rows(index).Item(0) = grvCanceladas.Rows(i).Cells(0).Text And ViewState("Requisiciones").Rows(index).Item(1) = grvCanceladas.Rows(i).Cells(1).Text Then
                    Alert("El elemento ya fue agregado previamente", Me, 1, 3)
                    Exit Sub
                End If
            Next

            'Incrementar lo seleccionado
            '    Dim indiceUsar As Integer = 0
            '    indiceUsar = hdnRequiIndex.Value
            '  ViewState("Solicitudes").rows(indiceUsar).item(12) += CDec(ViewState("Requisiciones").Rows(index).Item(4) * ViewState("Requisiciones").Rows(index).Item(5))

            'Dim dt As DataTable = ViewState("Solicitudes")
            'grvSolicitudes.DataSource = dt
            'grvSolicitudes.DataBind()

            dtCanceladas.Rows.Add(dtRequis.Rows.Item(CInt(e.CommandArgument)).ItemArray)
            ViewState("Detalle2") = dtCanceladas
            grvCanceladas.DataSource = dtCanceladas
            grvCanceladas.DataBind()
            pnlCancelados.Visible = True

            dtRequis.Rows.RemoveAt(CInt(e.CommandArgument))
            ViewState("Requisiciones") = dtRequis
            grvRequisiciones.DataSource = dtRequis
            grvRequisiciones.DataBind()
        End If

    End Sub

    Protected Sub grvAprobadas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvAprobadas.RowCommand
        If (e.CommandName = "Eliminar") Then
            Dim dtRequis As New DataTable()
            dtRequis.Columns.AddRange(New DataColumn(14) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus"), New DataColumn("idEstatus")})


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
            'Decrementar lo seleccionado
            Dim indiceUsar As Integer = 0
            indiceUsar = hdnRequiIndex.Value
            ViewState("Solicitudes").rows(indiceUsar).item(12) -= CDec(ViewState("Detalle").Rows(index).Item(4) * ViewState("Detalle").Rows(index).Item(5))

            'Dim dt As DataTable = ViewState("Solicitudes")
            'grvSolicitudes.DataSource = dt
            'grvSolicitudes.DataBind()

            'Afecta total
            lblMateriales.Text = "$" + (CDec(lblMateriales.Text) + CDec(dtAprobadas.Rows.Item(CInt(e.CommandArgument)).ItemArray(4) * dtAprobadas.Rows.Item(CInt(e.CommandArgument)).ItemArray(5))).ToString("###,###,##0.00")
            lblTotal.Text = "$" + (CDec(lblTotal.Text) + CDec(dtAprobadas.Rows.Item(CInt(e.CommandArgument)).ItemArray(4) * dtAprobadas.Rows.Item(CInt(e.CommandArgument)).ItemArray(5))).ToString("###,###,##0.00")

            'Marcar en rojo si es menor a cero
            lblMateriales.ForeColor = If(CDec(lblMateriales.Text) < 0, Color.Red, Color.Black)
            lblTotal.ForeColor = If(CDec(lblTotal.Text) < 0, Color.Red, Color.Black)

            dtAprobadas.Rows.RemoveAt(CInt(e.CommandArgument))
            ViewState("Detalle") = dtAprobadas
            grvAprobadas.DataSource = dtAprobadas
            grvAprobadas.DataBind()



        End If
    End Sub

    Protected Sub grvServicios_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvServicios.RowCommand
        If (e.CommandName = "Eliminar") Then
            Dim dtRequis As New DataTable()
            dtRequis.Columns.AddRange(New DataColumn(14) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus"), New DataColumn("idEstatus")})


            Dim dtService As New DataTable()
            If Not ViewState("Detalle1") Is Nothing Then
                dtService = ViewState("Detalle1")
            End If

            If Not ViewState("Requisiciones") Is Nothing Then
                dtRequis = ViewState("Requisiciones")
            End If

            'Regresa a la lista de la parte superior si se trata de la misma requisicion, de lo contrario, unicamente se remueve (simulando que volvio a la req origen)
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim aRemoverId As String = grvServicios.Rows(index).Cells(0).Text
            If hdnIdRequisicionSel.Value = aRemoverId Then
                dtRequis.Rows.Add(dtService.Rows.Item(CInt(e.CommandArgument)).ItemArray)
                ViewState("Requisiciones") = dtRequis
                grvRequisiciones.DataSource = dtRequis
                grvRequisiciones.DataBind()
            End If
            'Decrementar lo seleccionado
            Dim indiceUsar As Integer = 0
            indiceUsar = hdnRequiIndex.Value
            ViewState("Solicitudes").rows(indiceUsar).item(12) -= CDec(ViewState("Detalle1").Rows(index).Item(4) * ViewState("Detalle1").Rows(index).Item(5))

            'Dim dt As DataTable = ViewState("Solicitudes")
            'grvSolicitudes.DataSource = dt
            'grvSolicitudes.DataBind()


            'Afecta total
            lblServicios.Text = "$" + (CDec(lblServicios.Text) + CDec(dtService.Rows.Item(CInt(e.CommandArgument)).ItemArray(4) * dtService.Rows.Item(CInt(e.CommandArgument)).ItemArray(5))).ToString("###,###,##0.00")
            lblTotal.Text = "$" + (CDec(lblTotal.Text) + CDec(dtService.Rows.Item(CInt(e.CommandArgument)).ItemArray(4) * dtService.Rows.Item(CInt(e.CommandArgument)).ItemArray(5))).ToString("###,###,##0.00")

            'Marcar en rojo si es menor a cero
            lblServicios.ForeColor = If(CDec(lblServicios.Text) < 0, Color.Red, Color.Black)
            lblTotal.ForeColor = If(CDec(lblTotal.Text) < 0, Color.Red, Color.Black)

            dtService.Rows.RemoveAt(CInt(e.CommandArgument))
            ViewState("Detalle1") = dtService
            grvServicios.DataSource = dtService
            grvServicios.DataBind()


        End If
    End Sub

    Protected Sub grvCanceladas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvCanceladas.RowCommand
        If (e.CommandName = "Eliminar") Then
            Dim dtRequis As New DataTable()
            dtRequis.Columns.AddRange(New DataColumn(14) {New DataColumn("numrequisicion"), New DataColumn("indice"), New DataColumn("codigo"), New DataColumn("concepto"), New DataColumn("cantidad"), New DataColumn("precio"), New DataColumn("ccoid"), New DataColumn("ccoNumero"), New DataColumn("contrato"), New DataColumn("proveedor"), New DataColumn("solicitante"), New DataColumn("departamento"), New DataColumn("fecha"), New DataColumn("estatus"), New DataColumn("idEstatus")})


            Dim dtCanceladas As New DataTable()
            If Not ViewState("Detalle2") Is Nothing Then
                dtCanceladas = ViewState("Detalle2")
            End If

            If Not ViewState("Requisiciones") Is Nothing Then
                dtRequis = ViewState("Requisiciones")
            End If

            'Regresa a la lista de la parte superior si se trata de la misma requisicion, de lo contrario, unicamente se remueve (simulando que volvio a la req origen)
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim aRemoverId As String = grvCanceladas.Rows(index).Cells(0).Text
            If hdnIdRequisicionSel.Value = aRemoverId Then
                dtRequis.Rows.Add(dtCanceladas.Rows.Item(CInt(e.CommandArgument)).ItemArray)
                ViewState("Requisiciones") = dtRequis
                grvRequisiciones.DataSource = dtRequis
                grvRequisiciones.DataBind()
            End If

            dtCanceladas.Rows.RemoveAt(CInt(e.CommandArgument))
            ViewState("Detalle2") = dtCanceladas
            grvCanceladas.DataSource = dtCanceladas
            grvCanceladas.DataBind()
        End If
    End Sub


    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Response.Redirect("wfrmAutorizarPresupuesto.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim strMensaje As String
        Dim clsFunciones As New clsFunciones
        Dim dtsCotiza As New DataSet
        Dim dtTmp As New DataTable()
        Dim listaCancelar As String = ""
        Try
            'If grvAprobadas.Rows.Count > 0 Then
            If grvAprobadas.Rows.Count > 0 Or grvCanceladas.Rows.Count > 0 Or grvServicios.Rows.Count > 0 Then
                Dim filterEstatus As String = ""



                For Each rowen As GridViewRow In grvAprobadas.Rows
                    ' Si está indicado para pasar a comité
                    If ViewState("Detalle").Rows(rowen.RowIndex).Item(14) = 11 Then
                        filterEstatus = "4"

                    Else
                        filterEstatus = "5"
                    End If

                    strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Actualiza_Estatus_RequisicionCompra",
                           rowen.Cells(0).Text & "," & rowen.Cells(1).Text & "," & filterEstatus.Trim & "," & Session("IdUsuario") & ",Normal,0,0",
                            "@intRequisicion,@intDetRequisicion,@intEstatus,@intIdUsuarioCaptura,@vchComentarios,@decPrecio,@intIdProveedor")

                    If strMensaje = "OK" Then
                        ' Se guarda cotizacion en caso de ser de contrato
                        ' If filterEstatus = "5" Then
                        '     strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsCotiza, "Cot", "Inserta_detRequisicionCompraCotizacion",
                        'rowen.Cells(0).Text & "," & rowen.Cells(1).Text & "," & rowen.Cells(2).Text & "," & ViewState("Detalle").Rows(rowen.RowIndex).Item(5).ToString & "," & ViewState("Detalle").Rows(rowen.RowIndex).Item(9).ToString,
                        ' "@intIdRequi,@intIdDetRequi,@intIdArticulo,@decPrecio,@intIdProveedor",
                        ' dtTmp)
                        '     If strMensaje = "OK" Then


                        '     Else
                        '         Alert("Error al guardar cotizacion: " & strMensaje, Me, 1, 4)
                        '         Exit Sub
                        '     End If
                        ' End If

                    Else
                        Alert("Error al guardar cambios " & strMensaje, Me, 1, 4)
                        Exit Sub
                    End If

                Next
                ' Servicios
                For Each rowen As GridViewRow In grvServicios.Rows
                    ' Si está indicado para pasar a comité
                    If ViewState("Detalle1").Rows(rowen.RowIndex).Item(14) = 11 Then
                        filterEstatus = "4"

                    Else
                        filterEstatus = "5"
                    End If

                    strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Actualiza_Estatus_RequisicionCompra",
                           rowen.Cells(0).Text & "," & rowen.Cells(1).Text & "," & filterEstatus.Trim & "," & Session("IdUsuario") & ",Normal,0,0",
                            "@intRequisicion,@intDetRequisicion,@intEstatus,@intIdUsuarioCaptura,@vchComentarios,@decPrecio,@intIdProveedor")

                    If strMensaje = "OK" Then
                        ' Se guarda cotizacion en caso de ser de contrato
                        ' If filterEstatus = "5" Then
                        '     strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsCotiza, "Cot", "Inserta_detRequisicionCompraCotizacion",
                        'rowen.Cells(0).Text & "," & rowen.Cells(1).Text & "," & rowen.Cells(2).Text & "," & ViewState("Detalle").Rows(rowen.RowIndex).Item(5).ToString & "," & ViewState("Detalle").Rows(rowen.RowIndex).Item(9).ToString,
                        ' "@intIdRequi,@intIdDetRequi,@intIdArticulo,@decPrecio,@intIdProveedor",
                        ' dtTmp)
                        '     If strMensaje = "OK" Then


                        '     Else
                        '         Alert("Error al guardar cotizacion: " & strMensaje, Me, 1, 4)
                        '         Exit Sub
                        '     End If
                        ' End If

                    Else
                        Alert("Error al guardar cambios " & strMensaje, Me, 1, 4)
                        Exit Sub
                    End If

                Next
                '  Canceladas manualmente
                For Each rowen As GridViewRow In grvCanceladas.Rows
                    strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Actualiza_Estatus_RequisicionCompra",
                           rowen.Cells(0).Text & "," & rowen.Cells(1).Text & ",10," & Session("IdUsuario") & ",,0,0",
                            "@intRequisicion,@intDetRequisicion,@intEstatus,@intIdUsuarioCaptura,@vchComentarios,@decPrecio,@intIdProveedor")

                    If strMensaje = "OK" Then


                    Else
                        Alert("Error al guardar cambios " & strMensaje, Me, 1, 4)
                        Exit Sub
                    End If

                Next
                'strMensaje = "OK"

                'Cancelar articulos no autorizados automaticamente
                'If listaCancelar <> "" Then
                '    strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Cancela_DetRequisicionCompra",
                '             Session("IdUsuario") & "," & listaCancelar,
                '                "@intIdUsuarioCaptura,@vchListaRequisiciones")

                '    If strMensaje = "OK" Then


                '    Else
                '        Alert("Error al guardar cambios " & strMensaje, Me)
                '        Exit Sub
                '        '    End If
                '    End If
                'End If


                Alert("Se han guardado los cambios", Me, 1, 2)
                Timer1.Enabled = True

            Else
                Alert("Favor de seleccionar Requisicion de compra a aprobar", Me, 1, 3)
            End If

        Catch ex As Exception
            Alert("Error al guardar cambios: " & ex.Message & ": ", Me, 1, 4)
        End Try
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("wfrmAutorizarPresupuesto.aspx?key=" & Request.QueryString("key").Replace("+", "%2B"))
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
