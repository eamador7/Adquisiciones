Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Web

Public Class clsAcceso

#Region "Funciones"
    'Valida las pantallas a la que tiene permiso el usuario con la pantalla en la que se trate de entrar
    Public Function Valida_PantallaUsuario(ByVal strLlave As String, ByVal strPantalla As String, ByRef dtsUsuario As DataSet) As String
        Try
            'Obtenemos la llave de desencriptacion
            Dim Llave As String = "Jmas" & CStr(Today)
            'Se cifra la llave de desencriptacion
            Dim LlaveCifrada As New clsCifrado(Llave)
            Dim idUsuario As String = ""
            
            'DecryptData throws if the wrong password is used. 
            Try
                'Se desencripta y asigna la variable "key" recibida del menu
                idUsuario = LlaveCifrada.DecryptData(strLlave)
            Catch ex As System.Security.Cryptography.CryptographicException
                'Si es incorrecto alguno de los valores de desencriptacion
                Return "Datos incorrectos para validar permisos de la pantalla que trata de ingresar: " & ex.Message
            End Try

            Dim strMensaje As String = ""
            Dim clsFunciones As New clsFunciones
            'Se consultan todas las pantallas a las que tiene permisos el usuario
            strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsUsuario, "Usuario", "Valida_PantallaUsuario", _
                                                    idUsuario & "," & strPantalla, "intIdUsuario,vchUrl", dtsUsuario.Tables("Usuario"))
            If strMensaje = "OK" Then
                'Si cuenta con permisos de ver alguna pantalla
                If dtsUsuario.Tables("Usuario").Rows.Count < 1 Then
                    
                    strMensaje = "No tiene permisos de entrar a esta pantalla"
                    
                End If
            End If
            Return strMensaje
        Catch ex As Exception
            Return "Error al tratar de validar pantalla con el usuario: " & ex.Message
        End Try
    End Function


    'Valida los datos de acceso del login y carga un dataset con los datos del usuario, regresa mensaje
    Public Function ValidaAcceso(ByVal usr As String, ByVal pass As String, ByRef dtsUsuario As DataSet) As String
        Dim strMensaje As String = ""
        Dim clsFunciones As New clsFunciones
        Try
            If usr <> "" Then ' Si usuario esta en blanco
                If pass <> "" Then ' Si password esta en blanco
                    'Se llena el dataset con los datos del usuario
                    strMensaje = clsFunciones.Llena_Dataset("MIGRACION", dtsUsuario, "Usuario", "Cargar_Usuario", usr.Trim.Replace("'", ""), "vchUsuario", dtsUsuario.Tables("Usuario"))
                    If strMensaje = "OK" Then 'Si no hubo errores en la consulta
                        If dtsUsuario.Tables("Usuario").Rows.Count > 0 Then 'Si existe el usuario
                            If pass.Trim.Replace("'", "") = dtsUsuario.Tables("Usuario").Rows(0).Item(2) Then 'Si el password esta correcto
                                If dtsUsuario.Tables("Usuario").Rows(0).Item(4) Then 'Si el usuario esta activo
                                    If dtsUsuario.Tables("Usuario").Rows(0).Item(5) = "Activo" Then 'Si el usuario no expiro su vigencia
                                        strMensaje = "OK"
                                    Else
                                        strMensaje = dtsUsuario.Tables("Usuario").Rows(0).Item(5)
                                    End If
                                Else
                                    strMensaje = "Usuario inactivo"
                                End If
                            Else
                                strMensaje = "Password incorrecto, favor de verificarlo"
                            End If
                        Else
                            strMensaje = "Usuario no existente, favor de verificarlo"
                        End If
                    Else
                        strMensaje = "Error al buscar el usuario " & usr.Replace("'", "") & ": " & strMensaje.Replace("'", "") & ""
                    End If
                Else
                    strMensaje = "Favor de capturar el password"
                End If
            Else
                strMensaje = "Favor de capturar el usuario"
            End If
        Catch ex As Exception
            strMensaje = ex.Message
        Finally
            ValidaAcceso = strMensaje
        End Try
    End Function

#End Region
End Class
