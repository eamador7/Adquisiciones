Imports System.Data
Partial Class wfrmReportesOrdenesCompra
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim clsFunciones As New clsFunciones
            Dim dtsDepartamentos, dtsReportes, dtsArticulos As New DataSet
            Dim strMensaje As String
            strMensaje = clsFunciones.LlenarDropDownList(ddlDepartamento, "MIGRACION", dtsDepartamentos, "intDepartamento", "vchDepartamento", _
                         "Departamento", "Cargar_Departamentos", Session("departamento"), "@intDepartamento")

            If strMensaje <> "OK" Then 'Si no se realizo la consulta
                Alert("Error al cargar los Departamentos: " & strMensaje.Replace("'", "") & "');")
            End If

            strMensaje = clsFunciones.LlenarDropDownList(ddlReporte, "MIGRACION", dtsReportes, "intReporte", "vchReporte", _
                         "Reportes", "Cargar_Reportes", "0," & Session("IdUsuario"), "@intReporte,@intIdUsuario")

            If strMensaje <> "OK" Then 'Si no se realizo la consulta
                Alert("Error al cargar los Reportes: " & strMensaje.Replace("'", "") & "');")
            End If

            'strMensaje = clsFunciones.LlenarDropDownList(ddlArticulo, "MIGRACION", dtsArticulos, "articulo", "descrip", _
            '            "Articulos", "Cargar_Articulos", "", "")

            'If strMensaje <> "OK" Then 'Si no se realizo la consulta
            '    Alert("Error al cargar los Articulos: " & strMensaje.Replace("'", "") & "');")
            'End If

        End If
    End Sub

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        If ddlReporte.SelectedValue > 0 Then
            If ddlReporte.SelectedValue = 1 Then
                If txtRequisicion.Text <> "" Then
                    ifrOrdenSalida.Src = "wfrmReporteOrdenSalida.aspx?key=" & Request.QueryString("key").Replace("+", "%2B") & _
                              "&intRequisicion=" & txtRequisicion.Text
                Else
                    Alert("Favor de anotar numero de Requisicion para la Salida del Almacén")
                End If
            Else
                Dim strFechaDesde, strFechaHasta As String
                Dim dteFechaDesde, dteFechaHasta As Date
                If txtFechaDesde.Text <> "" Then
                    dteFechaDesde = CDate(txtFechaDesde.Text)
                    'strFechaDesde = CStr(dteFechaDesde.Day) & "/" & CStr(dteFechaDesde.Month) & "/" & CStr(dteFechaDesde.Year)
                    If dteFechaDesde.Month.ToString.Length = 1 Then
                        strFechaDesde = CStr(dteFechaDesde.Year) & "0" & CStr(dteFechaDesde.Month)
                    Else
                        strFechaDesde = CStr(dteFechaDesde.Year) & CStr(dteFechaDesde.Month)
                    End If
                    If dteFechaDesde.Day.ToString.Length = 1 Then
                        strFechaDesde = strFechaDesde & "0" & CStr(dteFechaDesde.Day)
                    Else
                        strFechaDesde = strFechaDesde & CStr(dteFechaDesde.Day)
                    End If
                End If
                If txtFechaHasta.Text <> "" Then
                    dteFechaHasta = CDate(txtFechaHasta.Text)
                    'strFechaHasta = CStr(dteFechaHasta.Day) & "/" & CStr(dteFechaHasta.Month) & "/" & CStr(dteFechaHasta.Year)
                    If dteFechaHasta.Month.ToString.Length = 1 Then
                        strFechaHasta = CStr(dteFechaHasta.Year) & "0" & CStr(dteFechaHasta.Month)
                    Else
                        strFechaHasta = CStr(dteFechaHasta.Year) & CStr(dteFechaHasta.Month)
                    End If
                    If dteFechaHasta.Day.ToString.Length = 1 Then
                        strFechaHasta = strFechaHasta & "0" & CStr(dteFechaHasta.Day)
                    Else
                        strFechaHasta = strFechaHasta & CStr(dteFechaHasta.Day)
                    End If
                    'strFechaHasta = CStr(dteFechaHasta.Year) & CStr(dteFechaHasta.Month) & CStr(dteFechaHasta.Day)
                    'strFechaHasta = "20150616"
                End If

                ifrOrdenSalida.Src = "generadorReportes.aspx?key=" & Request.QueryString("key").Replace("+", "%2B") & _
                              "&intRequisicion=" & txtRequisicion.Text & _
                    "&intReporte=" & ddlReporte.SelectedValue & "&intDepartamento=" & ddlDepartamento.SelectedValue & _
                    "&dtmFechaDesde=" & strFechaDesde & "&dtmFechaHasta=" & strFechaHasta
            End If
        Else
            Alert("Favor de seleccionar el tipo de reporte a generar")
        End If

    End Sub
#End Region


#Region "Procedimientos"

    Private Sub Alert(ByVal strMensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('" & strMensaje & "');", True)
    End Sub

#End Region


End Class
