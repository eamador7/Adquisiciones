Imports System.Data

Partial Class reportes_rptTotalSalidasDia
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

            strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsRequisiciones, "Requisiciones", "Cargar_TotalSalidasDiarias", _
                        Request.QueryString("Departamento") & "," & Request.QueryString("FechaDesde") & "," & Request.QueryString("FechaHasta") & ",0", _
                        "@intDepartamento,@vchFechaDesde,@vchFechaHasta,@intDepartamentoSesion", dtsRequisiciones.Tables("Requisiciones"))

            If strMensaje = "OK" Then
                Dim dt As New DataTable()
                dt.Columns.AddRange(New DataColumn(1) {New DataColumn("fecha"), New DataColumn("total")})
                If Not ViewState("Requisiciones") Is Nothing Then
                    dt = ViewState("Requisiciones")
                End If

                If dtsRequisiciones.Tables("Requisiciones").Rows.Count > 0 Then
                    Session("total") = dtsRequisiciones.Tables("Requisiciones").Rows.Count
                    For i = 0 To dtsRequisiciones.Tables("Requisiciones").Rows.Count - 1
                        dt.Rows.Add(dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(0).ToString.Replace("12:00:00 a.m.", ""), dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(1))
                        If i = 0 Then
                            Session("label1") = "'" & dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(0) & "'"
                            Session("dato1") = "" & dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(1) & ""
                        ElseIf i < 10 Then
                            Session("label1") = Session("label1") & ",'" & dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(0) & "'"
                            Session("dato1") = Session("dato1") & "," & dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(1) & ""
                        ElseIf i = 10 Then
                            Session("label2") = "'" & dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(0) & "'"
                            Session("dato2") = "" & dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(1) & ""
                        Else
                            Session("label2") = Session("label2") & ",'" & dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(0) & "'"
                            Session("dato2") = Session("dato2") & "," & dtsRequisiciones.Tables("Requisiciones").Rows(i).Item(1) & ""
                        End If
                    Next
                Else
                    mensaje = "No existen datos para mostrar"
                End If
                
                Session("label2") = Session("label2") & "," & Session("label1") & "" & "," & Session("label2") & ""
                Session("dato2") = Session("dato2") & "," & Session("dato1") & "" & "," & Session("dato2") & ""
                ViewState("Requisiciones") = dt
                grvRequisiciones.DataSource = dt
                grvRequisiciones.DataBind()
            End If


        End If

    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Response.Redirect("rptTotalSalidasDia.aspx?FechaDesde=" & Session("fechadesde") & "&FechaHasta=" & Session("fechahasta") & "&Departamento=" & Session("filtroDepartamento"))
    End Sub

End Class
