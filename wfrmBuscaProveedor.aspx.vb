Imports System.Data
Imports System.Data.SqlClient
Partial Class wfrmBuscaProveedor
    Inherits System.Web.UI.Page
#Region "Eventos"

    Protected Sub btnBuscarProveedor_Click(sender As Object, e As EventArgs) Handles btnBuscarProveedor.Click

        Try
            If txtNumProveedor.Text.Replace(" ", "") <> "" Or txtNombreBuscar.Text.Replace(" ", "") <> "" Then ' si se capturo el numero o nombre de proveedor

                If txtNombreBuscar.Text.Replace(" ", "") <> "" And txtNombreBuscar.Text.Length < 3 Then ' si se capturonombre y  menos de tres letras del proveedor
                    Alert("Favor de capturar almenos tres letras del proveedor ")
                End If

                '' busco el proveedor
                Dim strMensaje As String
                Dim dtsProveedores As New DataSet
                Dim clsFuncion As New clsFunciones
                strMensaje = clsFuncion.Llena_Dataset("Migracion", dtsProveedores, "Proveedores", "Cargar_Proveedores", txtNumProveedor.Text & "," & txtNombreBuscar.Text.Replace(" ", "%"), "intProveedor,vchNombre", dtsProveedores.Tables("Proveedores"))
                If strMensaje = "OK" Then '' se realizo la busqueda  con exito
                    If dtsProveedores.Tables("Proveedores").Rows.Count > 0 Then ' si existen proveedores
                        If dtsProveedores.Tables("Proveedores").Rows.Count = 1 Then ' solo existe un proveedor se cargan los datos en la forma del contrato
                            Response.Redirect(Session("caller") & "?key=" & Request.QueryString("key").Replace("+", "%2B") & _
                               "&noProveedor=" & dtsProveedores.Tables("Proveedores").Rows(0).Item(0) & _
                               "&nombre=" & dtsProveedores.Tables("Proveedores").Rows(0).Item(1) & _
                               "&domicilio=" & dtsProveedores.Tables("Proveedores").Rows(0).Item(2) & _
                               "&ciudad=" & dtsProveedores.Tables("Proveedores").Rows(0).Item(3) & _
                               "&rfc=" & dtsProveedores.Tables("Proveedores").Rows(0).Item(4) & _
                                "&nomfis=" & dtsProveedores.Tables("Proveedores").Rows(0).Item(5))
                        End If

                        ' lleno el grid de proveedores
                        Dim dtResultado As New DataTable
                        Dim contador As Integer
                        '' se agreagan las columnas con los datos del proveedor para recibir el dataset
                        dtResultado.Columns.AddRange(New DataColumn(6) {New DataColumn("noproveedor"), New DataColumn("nombre"), New DataColumn("domicilio"), New DataColumn("cidudad"), New DataColumn("rfc"), New DataColumn("nomfis"), New DataColumn("Seleccionar")})
                        ' se llena la tabla con los datos del dataset
                        For Contador = 0 To dtsProveedores.Tables("Proveedores").Rows.Count - 1

                            dtResultado.Rows.Add(dtsProveedores.Tables("Proveedores").Rows(Contador).Item(0), _
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(1), _
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(2), _
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(3), _
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(4), _
                                                 dtsProveedores.Tables("Proveedores").Rows(Contador).Item(5)
                                                 )

                        Next
                        ' se agrega la tabla al viewstate y al datagrid
                        ViewState("Proveedores") = dtResultado
                        grvProveedor.DataSource = dtResultado
                        grvProveedor.DataBind()


                    Else

                        Alert("No se encontraron proveedores con este nombre o no. de proveedor")
                    End If


                Else
                    Alert("No se pudo buscar el proveedor " & txtNombreBuscar.Text.Replace("'", "") & ": " & strMensaje.Replace("'", "") & "")

                End If

            Else

                Alert("Favor de buscar con el nombre o numero de proveedor ")
            End If


        Catch ex As Exception
            Alert("Error: " & ex.Message.Replace("'", "") & "")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script234", "document.getElementById('dialog').style.display='none';alert('');", True)
        End Try
    End Sub


    Protected Sub grvProveedor_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvProveedor.RowCommand
        Dim _caller As String = Session("caller")

        If (e.CommandName = "Seleccionar") Then

            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)

            Response.Redirect(Session("caller") & "?key=" & Request.QueryString("key").Replace("+", "%2B") & _
               "&noProveedor=" & ViewState("Proveedores").Rows(intIndex).Item(0) & _
               "&nombre=" & ViewState("Proveedores").Rows(intIndex).Item(1) & _
               "&domicilio=" & ViewState("Proveedores").Rows(intIndex).Item(2) & _
               "&ciudad=" & ViewState("Proveedores").Rows(intIndex).Item(3) & _
               "&rfc=" & ViewState("Proveedores").Rows(intIndex).Item(4) & _
                "&nomfis=" & ViewState("Proveedores").Rows(intIndex).Item(5))

        End If

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
