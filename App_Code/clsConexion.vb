Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Public Class clsConexion
    Public conexionsql As New SqlConnection

#Region "Funciones"

    'Se obtiene el Connection String solicitado
    Public Function GetConnectionString(ByVal Conexion As String) As String
        Return WebConfigurationManager.ConnectionStrings(Conexion).ConnectionString
    End Function

    'Abre conexion a una base de datos determinada en el web.config
    Public Function Open(ByVal Conexion As String) As String
        Try
            'Se evalua si esta abierta la conexion para cerrarla por si se desea conectar a un servidor diferente
            If conexionsql.State = ConnectionState.Open Then
                conexionsql.Close()
            End If
            'Obtenemos el string de conexion y la abrimos
            conexionsql.ConnectionString = GetConnectionString(Conexion)
            conexionsql.Open()
            Return "OK"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

#End Region

#Region "Procedimientos"

    'Cierra la conexion actual de base de datos
    Public Sub Close()
        conexionsql.Close()
        conexionsql.Dispose()
    End Sub

#End Region


End Class
