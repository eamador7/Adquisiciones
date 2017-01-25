Imports System.Data
Imports System.IO
Partial Class MPCompras
    Inherits System.Web.UI.MasterPage


#Region "Eventos"

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        'Try
        '    Dim dtsUsuario As New DataSet
        '    Dim clsAcceso As New clsAcceso
        '    Dim special As String = ""
        '    special = Request.QueryString("spec")
        '    If special = "" Then
        '        Dim strMensajeValidacion As String = clsAcceso.Valida_PantallaUsuario(Request.QueryString("key").Replace("%2B", "+"),
        '             Request.RawUrl, dtsUsuario)

        '        If strMensajeValidacion = "OK" Then
        '            'Asignamos variables de sesion para uso general
        '            Session("IdUsuario") = dtsUsuario.Tables("Usuario").Rows(0).Item("intIdUsuario")
        '            Session("usuario") = dtsUsuario.Tables("Usuario").Rows(0).Item("usuario")
        '            Session("nombre") = dtsUsuario.Tables("Usuario").Rows(0).Item("nombreusuario")
        '            Session("departamento") = dtsUsuario.Tables("Usuario").Rows(0).Item("departamento")
        '            Session("intEmpleado") = dtsUsuario.Tables("Usuario").Rows(0).Item("intEmpleado")
        '            Session("idPerfil") = dtsUsuario.Tables("Usuario").Rows(0).Item("intIdPerfil")
        '            Session("Valido") = 1
        '        Else
        '            Response.Write("<script>alert('" & strMensajeValidacion & "');</script>")
        '            Response.End()
        '        End If
        '    Else
        '        Session("IdUsuario") = special
        '    End If
        'Catch ex As Exception
        '    Response.Write("<script>alert('" & ex.Message & "');</script>")
        '    Response.End()
        'End Try
    End Sub

#End Region




End Class

