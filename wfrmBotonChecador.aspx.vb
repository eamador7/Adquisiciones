Imports System.Data
Imports System.Data.SqlClient
Partial Class wfrmBotonChecador
    Inherits System.Web.UI.Page
#Region "Eventos"

    Protected Sub imagebutton1_Click(sender As Object, e As EventArgs) Handles imagebutton1.Click
        Dim strMensaje As String
        Dim clsFunciones As New clsFunciones
        Try
     
            strMensaje = clsFunciones.EjecutaProcedimiento("MIGRACION", "Inserta_espCheck", _
                             Session("IdUsuario"), _
                            "@intidEmpleadoCheck")
                'strMensaje = "OK"
            If strMensaje = "OK" Then
                Alert("Ha checado con exito")

                Timer1.Enabled = True
            Else
                Alert("Error al checar" & strMensaje)
            End If


        Catch ex As Exception
            Alert("Error : " & ex.Message & ": ")
        End Try
    End Sub



    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Se redirecciona para aprobar otra requisicion de almacen

    End Sub

#End Region

#Region "Procedimientos"

    Private Sub Alert(ByVal strMensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "alert('" & strMensaje & "');", True)
    End Sub
#End Region

#Region "Funciones"

#End Region




End Class
