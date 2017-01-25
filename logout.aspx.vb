
Partial Class logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session.Abandon()
        Session.Clear()
        Response.Redirect("login.aspx")
    End Sub
End Class
