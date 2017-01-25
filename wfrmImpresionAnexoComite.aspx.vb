Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports System.IO
Imports System.Collections
Imports CrystalDecisions.Shared
Imports KeepAutomation

Partial Class wfrmImpresionAnexoComite
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim reporte As ReportDocument = New ReportDocument()
            Try
                Dim strQuery As String = ""
                Dim strMensaje As String = ""
                Dim dstReportes As DataSet = New DataSet
                Dim clsFunciones As New clsFunciones

                strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dstReportes, "Reporte", "Cargar_DetallesRequisicionCompra",
                        ", , ,4,",
                        "@intIdRequisicion,@vchFechaDesde,@vchFechaHasta,@vchIdEstatus,@intIdSolicitante", dstReportes.Tables("Reporte"))
                If dstReportes.Tables("Reporte").Rows.Count > 0 Then



                    reporte.Load(Server.MapPath("reportes/AnexoComiteAdquisiciones.rpt"))
                    reporte.SetDataSource(dstReportes.Tables(0))
                    reporte.DataSourceConnections(0).SetConnection("MIGRACION", "Almacen", "INTERNET", "13110721Ocampo")
                    reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Anexo_" & Today.Day.ToString() & Today.Month.ToString() & Today.Year.ToString())
                    reporte.Dispose()
                    reporte = Nothing
                Else

                End If
            Catch ex As Exception
                Response.Write("<script>alert('Error al imprimir salida: " & ex.Message & "');</script>")
            Finally
                reporte.Dispose()
                reporte = Nothing
            End Try

        End If
    End Sub
End Class
