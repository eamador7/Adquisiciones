Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports System.IO
Imports System.Collections
Imports CrystalDecisions.Shared
Imports KeepAutomation

Partial Class wfrmReporteOrdenCompra
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim reporte As ReportDocument = New ReportDocument()
            Try
                Dim strQuery As String = ""
                Dim strMensaje As String = ""
                Dim dstReportes As DataSet = New DataSet
                Dim clsFunciones As New clsFunciones
                Dim ids As String = Request.QueryString("intOrden")
                strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dstReportes, "Reporte", "Cargar_OrdenCompra", _
                             ids, "@intIdOrden", dstReportes.Tables("Reporte"))
                If dstReportes.Tables("Reporte").Rows.Count > 0 Then

                    Dim barcode As New Barcode.Crystal.BarCode
                    barcode.Symbology = KeepAutomation.Barcode.Symbology.QRCode
                    barcode.X = 6
                    barcode.Y = 6
                    barcode.LeftMargin = 24
                    barcode.RightMargin = 24
                    barcode.TopMargin = 24
                    barcode.BottomMargin = 24
                    barcode.ImageFormat = System.Drawing.Imaging.ImageFormat.Png
                    barcode.CodeToEncode = "Orden=" & dstReportes.Tables(0).Rows(0).Item("numOrden") & vbCr & _
                      "Solicitante=" & dstReportes.Tables(0).Rows(0).Item("solicitante") & vbCr & _
                      "Departamento=" & dstReportes.Tables(0).Rows(0).Item("Departamento") & vbCr & _
                      "Proveedor=" & dstReportes.Tables(0).Rows(0).Item("nombreProveedor") & vbCr & _
                      "Fecha=" & dstReportes.Tables(0).Rows(0).Item("fecha")
                    Dim i, y As Integer
                    y = 1
                    For i = 0 To dstReportes.Tables("Reporte").Rows.Count - 1
                        barcode.CodeToEncode = barcode.CodeToEncode & vbCr & "Codigo" & (y) & "=" & dstReportes.Tables(0).Rows(i).Item("codigo")
                        barcode.CodeToEncode = barcode.CodeToEncode & vbCr & "Cantidad" & (y) & "=" & dstReportes.Tables(0).Rows(i).Item("cantidad")
                        barcode.CodeToEncode = barcode.CodeToEncode & vbCr & "PrecioU" & (y) & "=" & dstReportes.Tables(0).Rows(i).Item("precio")
                        barcode.CodeToEncode = barcode.CodeToEncode & vbCr & "ImporteArt" & (y) & "=" & dstReportes.Tables(0).Rows(i).Item("totalart")
                        i = i + 1
                        y = y + 1
                    Next
                
                    Dim Imagen As Byte() = barcode.generateBarcodeToByteArray()

                    For i = 0 To dstReportes.Tables("Reporte").Rows.Count - 1
                        dstReportes.Tables(0).Rows(i).Item("qrcode") = Imagen
                    Next

                    reporte.Load(Server.MapPath("reportes/OrdenDeCompra.rpt"))
                    reporte.SetDataSource(dstReportes.Tables(0))
                    reporte.DataSourceConnections(0).SetConnection("MIGRACION", "Almacen", "INTERNET", "13110721Ocampo")
                    reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "OrdenDeCompra_E" & ids)
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
