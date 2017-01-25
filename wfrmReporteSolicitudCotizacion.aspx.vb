Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports System.IO
Imports System.Collections
Imports CrystalDecisions.Shared
Imports KeepAutomation

Partial Class wfrmReporteSolicitudCotizacion
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim reporte As ReportDocument = New ReportDocument()
            Try
                Dim strQuery As String = ""
                Dim strMensaje As String = ""
                Dim clsFunciones As New clsFunciones

                Dim dtCotizacion As New DataTable()

                dtCotizacion = Session("Cotizacion")

                If dtCotizacion IsNot Nothing And dtCotizacion.Rows.Count > 0 Then


                    reporte.Load(Server.MapPath("reportes/SolicitudCotizacion.rpt"))
                    reporte.SetDataSource(dtCotizacion)
                    reporte.DataSourceConnections(0).SetConnection("MIGRACION", "Almacen", "INTERNET", "13110721Ocampo")
                    reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Solicitud_" & Today.ToString)
                    reporte.Dispose()
                    reporte = Nothing
                Else

                End If
            Catch ex As Exception
                Response.Write("<script>alert('Error al imprimir solicitud: " & ex.Message & "');</script>")
            Finally
                reporte.Dispose()
                reporte = Nothing
            End Try

        End If
    End Sub
End Class
