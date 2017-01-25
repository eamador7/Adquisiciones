Imports System.Data
Partial Class reportes_rptSalidasImportexMes
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load        
        If Not Page.IsPostBack Then
            Dim clsFunciones As New clsFunciones
            Dim mensaje As String
            Dim strMensaje As String = ""
            Dim i As Integer
            Session("label1") = ""
            Session("dato1") = ""
            Session("label2") = ""
            Session("dato2") = ""
            Session("fechadesde") = Request.QueryString("FechaDesde")
            Session("fechahasta") = Request.QueryString("FechaHasta")
            Session("filtroDepartamento") = Request.QueryString("Departamento")
            Session("total") = 0
            Dim dtsRequisiciones As DataSet = New DataSet

            strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsRequisiciones, "Requisiciones", "Cargar_ImporteComparativoMes", _
                        Request.QueryString("Departamento") & "," & Request.QueryString("FechaDesde") & "," & Request.QueryString("FechaHasta") & ",0", _
                        "@intDepartamento,@vchFechaDesde,@vchFechaHasta,@intDepartamentoSesion", dtsRequisiciones.Tables("Requisiciones"))

            If strMensaje = "OK" Then
                Dim dt As New DataTable()
                dt.Columns.AddRange(New DataColumn(4) {New DataColumn("mes"), New DataColumn("anterior"), New DataColumn("actual"), New DataColumn("diferencia"), New DataColumn("porcentaje")})
                If Not ViewState("Requisiciones") Is Nothing Then
                    dt = ViewState("Requisiciones")
                End If
                Dim totalAnterior As Double = 0
                Dim totalActual As Double = 0
                If dtsRequisiciones.Tables("Requisiciones").Rows.Count > 0 Then
                    Session("total") = dtsRequisiciones.Tables("Requisiciones").Rows.Count
                    For i = 0 To dtsRequisiciones.Tables("Requisiciones").Rows.Count - 1
                        dt.Rows.Add(MonthName(dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(0), True).ToUpper, String.Format("{0:C}", dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(1)), String.Format("{0:C}", dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(2)), String.Format("{0:C}", dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(2) - dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(1)), Math.Round(((dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(2) / dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(1)) * 100) - 100, 2))
                        totalAnterior = totalAnterior + dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(1)
                        totalActual = totalActual + dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(2)
                        If i = 0 Then
                            Session("label1") = "" & MonthName(dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(0), True).ToUpper & ""
                            Session("dato1") = "" & Math.Round(dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(1), 2) & ""
                            Session("dato2") = "" & Math.Round(dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(2), 2) & ""
                        Else
                            Session("label1") = Session("label1") & "," & MonthName(dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(0), True).ToUpper & ""
                            Session("dato1") = Session("dato1") & "," & Math.Round(dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(1), 2) & ""
                            Session("dato2") = Session("dato2") & "," & Math.Round(dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(2), 2) & ""
                        End If
                    Next
                    dt.Rows.Add("ANUAL", String.Format("{0:C}", totalAnterior), String.Format("{0:C}", totalActual), String.Format("{0:C}", totalActual - totalAnterior), Math.Round(((totalActual / totalAnterior) * 100) - 100, 2))
                Else
                    mensaje = "No existen datos para mostrar"
                End If

                ViewState("Requisiciones") = dt
                grvRequisiciones.DataSource = dt
                grvRequisiciones.DataBind()
                ViewState("Depto") = Nothing
                grvDepartamentos.DataSource = Nothing
                grvDepartamentos.DataBind()
                For i = 0 To dtsRequisiciones.Tables("Requisiciones").Rows.Count - 1
                    If (dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(2) - dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(1)) > 0 Then
                        grvRequisiciones.Rows(i).Cells(3).ForeColor = Drawing.Color.Red
                        grvRequisiciones.Rows(i).Cells(4).ForeColor = Drawing.Color.Red
                    End If
                Next
                If totalActual > totalAnterior Then
                    grvRequisiciones.Rows(i).Cells(3).ForeColor = Drawing.Color.Red
                    grvRequisiciones.Rows(i).Cells(4).ForeColor = Drawing.Color.Red
                End If

            End If
            CargaDepartamentos("", "")
            Session("FechaDesdeDepto") = "09/07/" + CStr(DatePart(DateInterval.Year, Today) - 1)
            Session("FechaHastaDepto") = CDate(Today).ToShortDateString
        End If

    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Response.Redirect("rptSalidasImportexMes.aspx?FechaDesde=" & Session("fechadesde") & "&FechaHasta=" & Session("fechahasta") & "&Departamento=" & Session("filtroDepartamento"))
    End Sub

    'Protected Sub grvRequisiciones_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvRequisiciones.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(grvRequisiciones, "Select$" & e.Row.RowIndex)
    '        e.Row.Attributes("style") = "cursor:pointer"
    '    End If
    'End Sub

    'Protected Sub grvRequisiciones_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grvRequisiciones.SelectedIndexChanged
    '    Dim index As Integer = grvRequisiciones.SelectedRow.RowIndex
    '    Dim name As String = grvRequisiciones.SelectedRow.Cells(0).Text
    '    Dim country As String = grvRequisiciones.SelectedRow.Cells(1).Text
    '    Dim message As String = "Row Index: " & index & "\nName: " & name + "\nCountry: " & country
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('" + message + "');", True)
    'End Sub
    Public Sub CargaDepartamentos(strFechaDesde As String, strFechaHasta As String)
        ViewState("Depto") = Nothing
        grvDepartamentos.DataSource = Nothing
        grvDepartamentos.DataBind()
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn(3) {New DataColumn("iddepartamento"), New DataColumn("departamento"), New DataColumn("salidas"), New DataColumn("actual")})
        Dim clsFunciones As New clsFunciones
        Dim dtsDepartamentos As New DataSet
        Dim strMensaje As String        

        strMensaje = clsFunciones.Llena_Grid(grvDepartamentos, "MIGRACION", dtsDepartamentos, "Depto", "Cargar_ImporteComparativoDepto", _
                     "0," & strFechaDesde & "," & strFechaHasta & ",0", "@intDepartamento,@vchFechaDesde,@vchFechaHasta,@intDepartamentoSesion")
        If strMensaje = "OK" Then
            Dim i As Integer            
            If dtsDepartamentos.Tables("Depto").Rows.Count > 0 Then

                For i = 0 To dtsDepartamentos.Tables("Depto").Rows.Count - 1
                    dt.Rows.Add(dtsDepartamentos.Tables("Depto").Rows(i).Item(0),
                                dtsDepartamentos.Tables("Depto").Rows(i).Item(1),
                                dtsDepartamentos.Tables("Depto").Rows(i).Item(2),
                                String.Format("{0:C}", dtsDepartamentos.Tables("Depto").Rows(i).Item(3)))
                Next
                grvDepartamentos.DataSource = dt
                grvDepartamentos.DataBind()                

            End If
        Else
            'Alert("Error al cargar el detalle de la Requisicion: " & strMensaje.Replace("'", "") & "');")
        End If

    End Sub

    Protected Sub grvRequisiciones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvRequisiciones.RowCommand
        If (e.CommandName = "Seleccionar") Then
            ' Obtenemos la fila seleccionada
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            
            Dim strFechaDesde, strFechaHasta As String

            strFechaDesde = CStr(DatePart(DateInterval.Year, Today))
            strFechaHasta = CStr(DatePart(DateInterval.Year, Today))
            If index < 9 Then
                strFechaDesde = strFechaDesde & "0" & CStr(index + 1) & "01"
                strFechaHasta = strFechaHasta & "0" & CStr(index + 1) & CStr(Date.DaysInMonth(DatePart(DateInterval.Year, Today), index + 1))
            Else
                strFechaDesde = strFechaDesde & CStr(index + 1) & "01"
                strFechaHasta = strFechaHasta & CStr(index + 1) & CStr(Date.DaysInMonth(DatePart(DateInterval.Year, Today), index + 1))
            End If
            Session("FechaDesdeDepto") = strFechaDesde
            Session("FechaHastaDepto") = strFechaHasta
            Dim i As Integer
            For i = 0 To ViewState("Requisiciones").Rows.Count - 1
                grvRequisiciones.Rows(i).BackColor = Drawing.Color.White
            Next
            CargaDepartamentos(strFechaDesde, strFechaHasta)

            grvRequisiciones.Rows(index).BackColor = Drawing.Color.LightGreen       
        End If
    End Sub

    'Protected Sub grvRequisiciones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvRequisiciones.RowCommand
    '    If (e.CommandName = "Seleccionar") Then
    '        ' Obtenemos la fila seleccionada
    '        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
    '        'Si el importe del mes seleccionado es mayor a 0
    '        If CDbl(ViewState("Requisiciones").Rows(index).Item(2).ToString) <> "$0.00" Then
    '            Dim strFechaDesde, strFechaHasta As String

    '            strFechaDesde = CStr(DatePart(DateInterval.Year, Today))
    '            strFechaHasta = CStr(DatePart(DateInterval.Year, Today))
    '            If index < 9 Then
    '                strFechaDesde = strFechaDesde & "0" & CStr(index + 1) & "01"
    '                strFechaHasta = strFechaHasta & "0" & CStr(index + 1) & CStr(Date.DaysInMonth(DatePart(DateInterval.Year, Today), index + 1))
    '            Else
    '                strFechaDesde = strFechaDesde & CStr(index + 1) & "01"
    '                strFechaHasta = strFechaHasta & CStr(index + 1) & CStr(Date.DaysInMonth(DatePart(DateInterval.Year, Today), index + 1))
    '            End If

    '            Response.Redirect("../generadorReportes.aspx?intRequisicion=&intReporte=2&intDepartamento=0" & _
    '                "&dtmFechaDesde=" & strFechaDesde & "&dtmFechaHasta=" & strFechaHasta)
    '            'ifrOrdenSalida.Src = "../generadorReportes.aspx?intRequisicion=&intReporte=2&intDepartamento=0" & _
    '            '    "&dtmFechaDesde=" & CStr(DatePart(DateInterval.Year, Today)) & CStr(index + 1) & "01&dtmFechaHasta="
    '        Else
    '            ClientScript.RegisterStartupScript(Me.GetType(), "script234", "alert('No existen salidas de almacen todavia en el mes seleccionado');", True)
    '        End If

    '    End If
    'End Sub
End Class