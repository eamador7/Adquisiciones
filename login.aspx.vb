Imports System.Web
Imports System.Data
Partial Class login
    Inherits Page

#Region "Eventos"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


        Response.Redirect("../login.aspx")
        Dim clsAcceso As New clsAcceso
        Dim dtsPermisos, dtsUsuario As New DataSet
        Dim clsFunciones As New clsFunciones
        Dim strMensaje As String
        If Page.IsPostBack Then
            'Validamos el usuario que pretende entrar
            strMensaje = clsAcceso.ValidaAcceso(Request.Form("usr"), Request.Form("pass"), dtsUsuario)
            If strMensaje = "OK" Then 'Si el usuario es valido
                If CargaPantallas(dtsUsuario.Tables("Usuario").Rows(0).Item(0)) = "OK" Then 'Si tiene perfil asignado
                    Session("Valido") = 1 'Se utiliza para poder entrar a pantallas
                    Session("usuario") = dtsUsuario.Tables("Usuario").Rows(0).Item(1) 'Usuario
                    Session("nombre") = dtsUsuario.Tables("Usuario").Rows(0).Item(3) 'Nombre del usuario
                    Session("departamento") = dtsUsuario.Tables("Usuario").Rows(0).Item(6) 'Departamento del usuario
                    Session("IdUsuario") = dtsUsuario.Tables("Usuario").Rows(0).Item(0) 'Id del usuario
                    Session("idempleado") = dtsUsuario.Tables("Usuario").Rows(0).Item(7) 'Id del empleado
                    Response.Redirect("wfrmMenu.aspx")
                    'Else
                    '    Session.Clear()
                    '    Session.Abandon()
                    '    Session.RemoveAll()
                End If
            Else
                Response.Write("<script>alert('Error al validar usuario: " & strMensaje.Replace("'", "") & "')</script>")
            End If            
        End If
        'Siempre que se llegue al login se limpian las variables de sesion
        Session.Clear()
        Session.Abandon()
        Session.RemoveAll()
    End Sub

#End Region

#Region "Funciones"

    'Se buscan permisos de acceso en base al usuario recibido y se generan variables de sesion con los permisos
    Private Function CargaPantallas(intIdUsuario As Integer) As String
        Dim strMensaje As String = ""
        Dim dtsPantallas, dtsUsuario As New DataSet
        Dim clsFunciones As New clsFunciones
        Dim intContadorModulos, intContadorBloques As Integer
        Try
            'Carga Modulos a los que tiene permiso el usuario
            strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsPantallas, "Modulos", "Cargar_Pantallas", _
                                                    intIdUsuario & ",0", "intIdUsuario,intIdPadre", dtsPantallas.Tables("Modulos"))
            If strMensaje = "OK" Then 'Si fue correcta la consulta de modulos
                If dtsPantallas.Tables("Modulos").Rows.Count > 0 Then 'Si tiene permiso de ver algun modulo
                    'Se asigna a una variable de sesion un arreglo con el id, nombre y url de los modulos a los que se tengan permisos
                    Session("0") = dtsPantallas.Tables("Modulos")
                    For intContadorModulos = 0 To Session("0").Rows.Count - 1 'Se recorre cada modulo para buscar los bloque a los que se tiene permiso
                        'Carga Bloques para cada Modulo
                        strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsPantallas, "Bloques" & CStr(intContadorModulos), "Cargar_Pantallas", _
                                         intIdUsuario & "," & Session("0").rows(intContadorModulos).item(0), "intIdUsuario,intIdPadre", dtsPantallas.Tables("Bloques"))
                        If strMensaje = "OK" Then 'Si fue correcta la consulta de bloques
                            If dtsPantallas.Tables("Bloques" & CStr(intContadorModulos)).Rows.Count > 0 Then 'Si tiene permiso de ver algun bloque
                                'Se asigna a una variable de sesion un arreglo con el id, nombre y url de los bloques a los que se tengan permisos por modulo
                                Session("" & Session("0").rows(intContadorModulos).item(0) & "") = dtsPantallas.Tables("Bloques" & CStr(intContadorModulos))
                                For intContadorBloques = 0 To Session("" & Session("0").rows(intContadorModulos).item(0) & "").Rows.Count - 1 'Se recorre cada bloque para buscar las pantallas a los que se tiene permiso
                                    'Carga las pantallas de cada Bloque
                                    strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsPantallas, "Pantallas_" & CStr(intContadorModulos) & CStr(intContadorBloques), "Cargar_Pantallas", _
                                                    intIdUsuario & "," & Session("" & Session("0").rows(intContadorModulos).item(0) & "").rows(intContadorBloques).item(0), "intIdUsuario,intIdPadre", dtsPantallas.Tables("Pantallas"))
                                    If strMensaje = "OK" Then 'Si fue correcta la consulta de pantallas
                                        If dtsPantallas.Tables("Pantallas_" & CStr(intContadorModulos) & CStr(intContadorBloques)).Rows.Count > 0 Then 'Si tiene permiso de ver alguna pantalla
                                            'Se asigna a una variable de sesion un arreglo con el id, nombre y url de las pantallas a los que se tengan permisos por bloque
                                            Session("" & Session("" & Session("0").rows(intContadorModulos).item(0) & "").rows(intContadorBloques).item(0) & "") = dtsPantallas.Tables("Pantallas_" & CStr(intContadorModulos) & CStr(intContadorBloques))
                                        End If
                                    Else
                                        Response.Write("<script>alert('Error al cargar el menu, favor de reportar al Departamento de Sistemas: " & strMensaje.Replace("'", "") & "')</script>")
                                        Return strMensaje
                                        Exit Function
                                    End If
                                Next 'Recorrer cada bloque para buscar las pantallas a las que se tiene permiso
                            End If
                        Else
                            Response.Write("<script>alert('Error al cargar el menu, favor de reportar al Departamento de Sistemas: " & strMensaje.Replace("'", "") & "')</script>")
                            Return strMensaje
                            Exit Function
                        End If
                    Next 'Recorrer cada modulo para buscar los bloque a los que se tiene permiso
                Else
                    strMensaje = "Usuario sin permisos para ver modulos"
                    Response.Write("<script>alert(' " & strMensaje.Replace("'", "") & "')</script>")
                    Return strMensaje
                    Exit Function
                End If
            Else
                Response.Write("<script>alert('Error al cargar el menu, favor de reportar al Departamento de Sistemas: " & strMensaje.Replace("'", "") & "')</script>")
            End If
        Catch ex As Exception
            Response.Write("<script>alert('Error al cargar el menu, favor de reportar al Departamento de Sistemas: " & ex.Message.Replace("'", "") & "')</script>")
            strMensaje = ex.Message
        Finally
            CargaPantallas = strMensaje
        End Try
    End Function

#End Region

End Class