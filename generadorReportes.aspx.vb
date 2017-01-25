Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports System.IO
Imports System.Collections
Imports CrystalDecisions.Shared

Partial Class generadorReportes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                Dim strQuery As String = ""
                Dim strMensaje As String = ""
                Dim dstReportes As DataSet = New DataSet
                Dim dstDatos As DataSet = New DataSet
                Dim clsFunciones As New clsFunciones

                strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dstReportes, "Reportes", "Cargar_Reportes", _
                             Request.QueryString("intReporte") & "," & Session("IdUsuario"), "@intReporte,@intIdUsuario", dstReportes.Tables("Reportes"))
                If strMensaje = "OK" Then
                    If dstReportes.Tables("Reportes").Rows.Count > 0 Then
                        If InStr(dstReportes.Tables("Reportes").Rows(0).Item("vchUrl"), ".rpt", CompareMethod.Text) > 0 Then
                            strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dstDatos, "Datos", dstReportes.Tables("Reportes").Rows(0).Item("vchQuery"), _
                                                                Request.QueryString("intIdRequisicion") & "," & Request.QueryString("intDepartamento") & "," & _
                                                                Request.QueryString("dtmFechaDesde") & "," & Request.QueryString("dtmFechaHasta") & "," & Session("departamento"), _
                                                                "@intIdRequisicion,@intDepartamento,@vchFechaDesde,@vchFechaHasta,@intDepartamentoSesion", dstDatos.Tables("Datos"))
                            If strMensaje = "OK" Then
                                If dstDatos.Tables("Datos").Rows.Count > 0 Then
                                    Dim reporte As ReportDocument = New ReportDocument()
                                    reporte.Load(Server.MapPath(dstReportes.Tables("Reportes").Rows(0).Item("vchUrl")))
                                    reporte.SetDataSource(dstDatos.Tables(0))
                                    reporte.DataSourceConnections(0).SetConnection("MIGRACION", "Almacen", "INTERNET", "13110721Ocampo")
                                    reporte.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, True, dstReportes.Tables("Reportes").Rows(0).Item("vchNombreArchivo"))
                                Else
                                    Response.Write("<script>alert('No se encontraron datos, favor de buscar con otros filtros');</script>")
                                    Response.End()
                                End If
                            Else
                                Response.Write("<script>alert('Error al generar reporte: " & strMensaje & "');</script>")
                                Response.End()
                            End If
                        ElseIf InStr(dstReportes.Tables("Reportes").Rows(0).Item("vchUrl"), ".aspx", CompareMethod.Text) > 0 Then
                            Response.Redirect(dstReportes.Tables("Reportes").Rows(0).Item("vchUrl") & "?key=" & Request.QueryString("key").Replace("+", "%2B") & _
                              "&FechaDesde=" & Request.QueryString("dtmFechaDesde") & "&FechaHasta=" & Request.QueryString("dtmFechaHasta") & "&Departamento=" & Request.QueryString("intDepartamento"))
                        End If

                    Else
                        Response.Write("<script>alert('Error al generar reporte: Reporte no encontrado');</script>")
                        Response.End()
                    End If
                Else
                    Response.Write("<script>alert('Error al generar reporte: " & strMensaje & "');</script>")
                    Response.End()
                End If
            Catch ex As Exception
                Response.Write("<script>alert('Error al generar reporte: " & ex.Message & "');</script>")
                Response.End()
            End Try
        End If
    End Sub

End Class
