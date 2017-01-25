Partial Class wfrmMenu
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("Valido") <> 1 Then
            Response.Redirect("login.aspx")
        End If
    End Sub

End Class
