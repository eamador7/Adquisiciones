Imports System.Data
Partial Class reportes_rptSalidasDepartamento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Not Page.IsPostBack Then
        '    Dim con As New clsConexion
        '    Dim mensaje As String
        '    Dim strQuery As String = ""
        '    Dim i As Integer
        '    Session("label1") = ""
        '    Session("dato1") = ""
        '    Session("label2") = ""
        '    Session("dato2") = ""
        '    Dim dstDependencia As DataSet = New DataSet
        '    strQuery = "select top 10 dependencia,count(*) votaron from votos v, goente g " & _
        '               "where g.clave_electoral = v.clave group by dependencia order by 1 "
        '    If Request.QueryString("Dep") <> "" Then
        '        strQuery = "select top 10 descripcion dependencia,count(*) votaron from votos v, goente g " & _
        '               "where g.clave_electoral = v.clave group by dependencia order by 1 "
        '    End If
        '    dstDependencia = con.ConsultaDataset(strQuery, "Dependencia1")

        '    Dim dt As New DataTable()
        '    dt.Columns.AddRange(New DataColumn(1) {New DataColumn("dependencia"), New DataColumn("votaron")})
        '    If Not ViewState("Dep") Is Nothing Then
        '        dt = ViewState("Dep")
        '    End If
        '    If dstDependencia.Tables("Dependencia1").Rows.Count > 0 Then
        '        For i = 0 To dstDependencia.Tables("Dependencia1").Rows.Count - 1
        '            dt.Rows.Add(dstDependencia.Tables("Dependencia1").Rows(i).Item(0), dstDependencia.Tables("Dependencia1").Rows(i).Item(1))
        '            If i = 0 Then
        '                Session("label1") = "'" & dstDependencia.Tables("Dependencia1").Rows(i).Item(0) & "'"
        '                Session("dato1") = "" & dstDependencia.Tables("Dependencia1").Rows(i).Item(1) & ""
        '            Else
        '                Session("label1") = Session("label1") & ",'" & dstDependencia.Tables("Dependencia1").Rows(i).Item(0) & "'"
        '                Session("dato1") = Session("dato1") & "," & dstDependencia.Tables("Dependencia1").Rows(i).Item(1) & ""
        '            End If
        '        Next
        '    Else
        '        mensaje = "No existen datos para mostrar"
        '    End If
        '    strQuery = "select * from(select top 7 dependencia,count(*) votaron from votos v, goente g where g.clave_electoral = v.clave " & _
        '                "group by dependencia order by 1 desc) t order by 1 asc"
        '    dstDependencia = con.ConsultaDataset(strQuery, "Dependencia2")
        '    If dstDependencia.Tables("Dependencia2").Rows.Count > 0 Then
        '        For i = 0 To dstDependencia.Tables("Dependencia2").Rows.Count - 1
        '            dt.Rows.Add(dstDependencia.Tables("Dependencia2").Rows(i).Item(0), dstDependencia.Tables("Dependencia2").Rows(i).Item(1))
        '            If i = 0 Then
        '                Session("label2") = "'" & dstDependencia.Tables("Dependencia2").Rows(i).Item(0) & "'"
        '                Session("dato2") = "" & dstDependencia.Tables("Dependencia2").Rows(i).Item(1) & ""
        '            Else
        '                Session("label2") = Session("label2") & ",'" & dstDependencia.Tables("Dependencia2").Rows(i).Item(0) & "'"
        '                Session("dato2") = Session("dato2") & "," & dstDependencia.Tables("Dependencia2").Rows(i).Item(1) & ""
        '            End If
        '        Next
        '    Else
        '        mensaje = "No existen datos para mostrar"
        '    End If
        '    Session("label2") = Session("label2") & "," & Session("label1") & "" & "," & Session("label2") & ""
        '    Session("dato2") = Session("dato2") & "," & Session("dato1") & "" & "," & Session("dato2") & ""
        '    ViewState("Dep") = dt
        '    grvDependencia.DataSource = dt
        '    grvDependencia.DataBind()
        'End If

    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Response.Redirect("rptSalidasDepartamento.aspx")
    End Sub

    Protected Sub btnActualizar_Click(sender As Object, e As ImageClickEventArgs) Handles btnActualizar.Click
        Response.Redirect("rptSalidasDepartamento.aspx")
    End Sub
End Class
